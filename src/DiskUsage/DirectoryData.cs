namespace DiskUsage
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;
	using Microsoft.Research.CommunityTechnologies.Treemap;

	#endregion

	public class DirectoryData
	{
		#region Private Data Members

		private const double BytesPerMegabyte = 1048576.0;

		private DirectoryInfo directoryInfo;
		private long size;
		private long fileCount;
		private long folderCount;
		private string name;
		private string fullName;
		private DirectoryData[] subData;
		private DirectoryDataType dataType = DirectoryDataType.Directory;
		private Node treeMapNode = new Node(string.Empty, 0, 0);

		#endregion

		#region Constructors

		public DirectoryData(DirectoryInfo directoryInfo, BackgroundWorker worker)
			: this(directoryInfo, worker, true)
		{
		}

		public DirectoryData(string name, string fullName, long size, long fileCount)
		{
			this.dataType = DirectoryDataType.Files;
			this.treeMapNode.Tag = this;

			this.SetName(name, fullName);
			this.SetSize(size);
			this.fileCount = fileCount;
			this.subData = new DirectoryData[0];
			this.PullNodes();
		}

		public DirectoryData(string error)
		{
			this.dataType = DirectoryDataType.Error;
			this.treeMapNode.Tag = this;

			this.SetName(error, error);
			this.SetSize(0);
			this.subData = new DirectoryData[0];
			this.PullNodes();
		}

		private DirectoryData(DirectoryInfo directoryInfo, BackgroundWorker worker, bool reportProgress)
		{
			this.dataType = DirectoryDataType.Directory;
			this.treeMapNode.Tag = this;

			this.directoryInfo = directoryInfo;
			this.SetName(this.directoryInfo.Name, this.directoryInfo.FullName);
			this.Refresh(worker, reportProgress);
		}

		#endregion

		#region Public Members

		public long Size => this.size;

		public double SizeInMegabytes => this.size / BytesPerMegabyte;

		public long FileCount => this.fileCount;

		public long FolderCount => this.folderCount;

		public string Name => this.name;

		public string FullName => this.fullName;

		public DirectoryDataType DataType => this.dataType;

		public ICollection<DirectoryData> SubData => this.subData;

		[CLSCompliant(false)]
		public Node TreeMapNode => this.treeMapNode;

		public void Refresh(BackgroundWorker worker)
		{
			this.Refresh(worker, true);
		}

		public void AdjustStats(long sizeAdjustment, long fileCountAdjustment, long folderCountAdjustment)
		{
			this.AdjustStats(sizeAdjustment, fileCountAdjustment, folderCountAdjustment, true);
		}

		public void Explore(IWin32Window owner)
		{
			if (this.directoryInfo != null)
			{
				WindowsUtility.ShellExecute(owner, this.directoryInfo.FullName);
			}
		}

		public DirectoryData Clone()
		{
			DirectoryData result = (DirectoryData)this.MemberwiseClone();
			return result;
		}

		#endregion

		#region Private Methods

		private static void AddErrorData(List<DirectoryData> subDataList, string prefix, string message)
		{
			subDataList.Add(new DirectoryData(prefix + message));
		}

		private static bool CheckCancelled(BackgroundWorker worker)
		{
			bool result = false;

			if (worker != null)
			{
				result = worker.CancellationPending;
			}

			return result;
		}

		private void Refresh(BackgroundWorker worker, bool reportProgress)
		{
			if (this.directoryInfo != null)
			{
				// Recreate the tree map node so we can update its Nodes collection.
				if (this.treeMapNode.Nodes.Count > 0)
				{
					this.RecreateTreeMapNode(false);
				}

				this.SetSize(0);
				this.fileCount = 0;
				this.folderCount = 0;

				List<DirectoryData> subDataList = new List<DirectoryData>();

				this.CalculateFileSizes(subDataList);
				if (!CheckCancelled(worker))
				{
					if (reportProgress)
					{
						this.CalculateDirectorySizesWithProgress(subDataList, worker);
					}
					else
					{
						this.CalculateDirectorySizesParallel(subDataList, worker);
					}

					if (!CheckCancelled(worker))
					{
						this.subData = new DirectoryData[subDataList.Count];
						subDataList.CopyTo(this.subData);

						// I'm doing Y - X so the sort will always be in descending order.
						// I'm using Sign rather than an int cast because these sizes are
						// 64-bit integers so an int cast would truncate and lose data
						// when the sizes were > 2GB (which happens with directories).
						Array.Sort(this.subData, (x, y) => Math.Sign(y.Size - x.Size));

						this.PopulateTreeMapSubNodes();
					}
				}
			}
		}

		private void PopulateTreeMapSubNodes()
		{
			foreach (DirectoryData data in this.subData)
			{
				if (data.DataType != DirectoryDataType.Error)
				{
					this.treeMapNode.Nodes.Add(data.TreeMapNode);
				}
			}

			this.PullNodes();
		}

		private void RecreateTreeMapNode(bool addChildNodes)
		{
			this.treeMapNode = new Node(
				this.treeMapNode.Text,
				this.treeMapNode.SizeMetric,
				this.treeMapNode.ColorMetric,
				this.treeMapNode.Tag,
				this.treeMapNode.ToolTip);

			if (addChildNodes)
			{
				this.PopulateTreeMapSubNodes();
			}
		}

		[Conditional("DEBUG")]
		private void PullNodes()
		{
			// Debug builds will assert if you don't pull the Nodes
			// collection.  So I'll just call a debug-only method
			// here on the Nodes collection.
			if (this.subData.Length == 0)
			{
				this.treeMapNode.Nodes.AssertValid();
			}
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "I don't want an error in one node to stop the whole process.")]
		private void CalculateFileSizes(List<DirectoryData> subDataList)
		{
			try
			{
				int fileCount = 0;
				long fileSize = 0;
				Parallel.ForEach(
					this.directoryInfo.EnumerateFiles(),
					info =>
					{
						Interlocked.Add(ref fileSize, info.Length);
						Interlocked.Increment(ref fileCount);
					});

				this.AdjustStats(fileSize, fileCount, 0, false);

				// Only add a [Files] node if we found some files (even 0 byte files).
				if (fileCount > 0)
				{
					// Add a faux node for files
					const string FilesNodeName = "[Files]";
					DirectoryData data = new DirectoryData(FilesNodeName, Path.Combine(this.directoryInfo.FullName, FilesNodeName), fileSize, fileCount);
					subDataList.Add(data);
				}
			}
			catch (Exception ex)
			{
				AddErrorData(subDataList, "File Error: ", ex.Message);
			}
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "I don't want an error in one node to stop the whole process.")]
		private void CalculateDirectorySizesWithProgress(List<DirectoryData> subDataList, BackgroundWorker worker)
		{
			try
			{
				DirectoryInfo[] directories = this.directoryInfo.GetDirectories();

				// Since we're reporting the directory names as our progress, let's do them in alphabetical order.
				Array.Sort(directories, (x, y) => string.Compare(x.Name, y.Name, true));

				int numDirectories = directories.Length;
				for (int i = 0; i < numDirectories; i++)
				{
					DirectoryInfo info = directories[i];

					if (CheckCancelled(worker))
					{
						worker = null;
						break;
					}

					if (worker != null)
					{
						int percentage = (int)Math.Round((100.0 * i) / numDirectories);
						worker.ReportProgress(percentage, info.FullName);
						Debug.WriteLine(string.Format("{0}% - {1}", percentage, info.FullName));
					}

					this.CalculateDirectorySize(subDataList, worker, info);
				}

				if (worker != null)
				{
					worker.ReportProgress(100, string.Empty);
				}
			}
			catch (Exception ex)
			{
				AddErrorData(subDataList, "Directory Error: ", ex.Message);
			}
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "I don't want an error in one node to stop the whole process.")]
		private void CalculateDirectorySizesParallel(List<DirectoryData> subDataList, BackgroundWorker worker)
		{
			try
			{
				Parallel.ForEach(
					this.directoryInfo.EnumerateDirectories(),
					info =>
					{
						if (CheckCancelled(worker))
						{
							return;
						}

						this.CalculateDirectorySize(subDataList, worker, info);
					});
			}
			catch (Exception ex)
			{
				AddErrorData(subDataList, "Directory Error: ", ex.Message);
			}
		}

		private void CalculateDirectorySize(List<DirectoryData> subDataList, BackgroundWorker worker, DirectoryInfo info)
		{
			// Never report progress for sub-directories.  It adds too much blocking, which makes things take forever.
			// Add 1 to the folder count to account for the current directory.
			DirectoryData data = new DirectoryData(info, worker, false);
			this.AdjustStats(data.Size, data.FileCount, 1 + data.FolderCount, false);
			lock (subDataList)
			{
				subDataList.Add(data);
			}
		}

		private void SetSize(long size)
		{
			this.size = size;

			// Sizes must be positive numbers for the tree map control
			long treeMapSize = Math.Max(size, 1);
			this.treeMapNode.SizeMetric = treeMapSize;

			switch (this.dataType)
			{
				case DirectoryDataType.Directory:
				case DirectoryDataType.Files:
					// We also have to set the color metric, so base it on the log of the
					// size.  100KB has log 5 and 1GB has log 9, so we'll subtract
					// 5 and then get the percentage from 0 to 4.
					const int LowestLog = 5;
					const int HighestLog = 9;
					double log = Math.Max(LowestLog, Math.Log10(treeMapSize));
					double metric = ((log - LowestLog) / (HighestLog - LowestLog)) * 100;
					if (this.dataType == DirectoryDataType.Files)
					{
						metric = -metric;
					}

					this.treeMapNode.ColorMetric = (float)metric;
					break;

				default:
					// Use the default Window color for Error or Unknown.
					this.treeMapNode.ColorMetric = 0;
					break;
			}

			this.UpdateToolTip();
		}

		private void SetName(string name, string fullName)
		{
			this.name = name;
			this.fullName = fullName;
			this.treeMapNode.Text = name;
			this.UpdateToolTip();
		}

		private void UpdateToolTip()
		{
			switch (this.dataType)
			{
				case DirectoryDataType.Directory:
				case DirectoryDataType.Files:
					this.treeMapNode.ToolTip = string.Format("{0}: {1:N1} MB", this.fullName ?? this.name, this.SizeInMegabytes);
					break;

				case DirectoryDataType.Error:
					this.treeMapNode.ToolTip = this.Name;
					break;
			}
		}

		private void AdjustStats(long sizeAdjustment, long fileCountAdjustment, long folderCountAdjustment, bool recreateNode)
		{
			// If multiple threads are calculating sub-directory sizes, then the parent directory size
			// can be adjusted simultaneously.  We need to lock on some member to ensure the size
			// is safely adjusted along with all dependent info (e.g., tree map size).
			lock (this.directoryInfo)
			{
				this.SetSize(this.size + sizeAdjustment);
			}

			if (fileCountAdjustment != 0)
			{
				Interlocked.Add(ref this.fileCount, fileCountAdjustment);
			}

			if (folderCountAdjustment != 0)
			{
				Interlocked.Add(ref this.folderCount, folderCountAdjustment);
			}

			if (recreateNode)
			{
				this.RecreateTreeMapNode(true);
			}
		}

		#endregion
	}
}
