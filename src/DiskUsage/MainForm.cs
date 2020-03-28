namespace DiskUsage
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;
	using Menees;
	using Menees.Shell;
	using Menees.Windows.Forms;
	using Microsoft.Research.CommunityTechnologies.Treemap;

	#endregion

	public partial class MainForm
	{
		#region Private Data Members

		private const int ErrorImageIndex = 0;
		private const int FilesImageIndex = 1;
		private const int FolderImageIndex = 2;

		private Stopwatch stopwatch;

		#endregion

		#region Constructors

		public MainForm()
		{
			this.InitializeComponent();

			// Get the OS's closed folder icon if possible.
			Icon icon = null;
			ShellUtility.GetFileTypeInfo("Folder", false, IconOptions.Small | IconOptions.Folder, hIcon => icon = (Icon)Icon.FromHandle(hIcon).Clone());
			if (icon != null)
			{
				using (Image folderImage = icon.ToBitmap())
				{
					this.Images.Images[FolderImageIndex] = folderImage;
				}
			}
		}

		#endregion

		#region Internal Methods

		[SuppressMessage(
			"Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Windows Forms doesn't automatically handle errors in OnIdle event handlers.")]
		internal void OnIdle(object sender, EventArgs e)
		{
			try
			{
				bool scanning = this.MainWorker.IsBusy || this.RefreshWorker.IsBusy;
				this.mnuDrives.Enabled = !scanning;
				this.mnuChoosePath.Enabled = !scanning;
				this.mnuRecentPaths.Enabled = !scanning;
				this.mnuCancel.Enabled = scanning && !this.MainWorker.CancellationPending && !this.RefreshWorker.CancellationPending;

				bool directorySelected = !scanning && IsNodeADirectory(this.Tree.SelectedNode);

				this.mnuRefreshBranch.Enabled = directorySelected;
				this.mnuRefreshBranch2.Enabled = directorySelected;
				this.mnuOpenFolder.Enabled = directorySelected;
				this.mnuOpenFolder2.Enabled = directorySelected;

				this.Progress.Visible = scanning;
				this.lblProgressImage.Visible = scanning;
				this.Tree.Enabled = !scanning;
			}
			catch (Exception ex)
			{
				// We must explicitly call this because Application.Idle
				// doesn't run inside the normal ThreadException protection
				// that the Application provides for the main message pump.
				Application.OnThreadException(ex);
			}
		}

		#endregion

		#region Private Methods

		private static void AddDirectoryNode(TreeNodeCollection parentNodes, DirectoryData data)
		{
			TreeNode node = new TreeNode();

			node.Tag = data;
			node.ImageIndex = GetImageForData(data);
			node.SelectedImageIndex = node.ImageIndex;
			node.Name = data.Name;   // Map_NodeDoubleClick needs this so Nodes.IndexOfKey will work.
			SetNodeText(node, data);

			parentNodes.Add(node);

			if (data.SubData.Count > 0)
			{
				node.Nodes.Add(new DummyNode());
			}
		}

		private static int GetImageForData(DirectoryData data)
		{
			int result;

			switch (data.DataType)
			{
				case DirectoryDataType.Directory:
					result = FolderImageIndex;
					break;

				case DirectoryDataType.Files:
					result = FilesImageIndex;
					break;

				default:
					result = ErrorImageIndex; // Error or Unknown
					break;
			}

			return result;
		}

		private static void SetNodeText(TreeNode node, DirectoryData data)
		{
			node.Text = string.Format("{0}: {1:N1} MB", data.Name, data.SizeInMegabytes);
		}

		private static DirectoryData GetDataForNode(TreeNode node) => (DirectoryData)node.Tag;

		private static DirectoryData GetDataForNode(Node node) => (DirectoryData)node.Tag;

		private static string GetSuffix(double value) => Math.Round(value, 2) == 1 ? string.Empty : "s";

		private static bool IsNodeADirectory(TreeNode node)
		{
			bool result = false;

			if (node != null)
			{
				DirectoryData data = GetDataForNode(node);
				result = data.DataType == DirectoryDataType.Directory;
			}

			return result;
		}

		private static void SetDirectoryNodeImage(TreeNode node)
		{
			if (IsNodeADirectory(node))
			{
				DirectoryData data = GetDataForNode(node);
				int imageIndex = GetImageForData(data);
				node.ImageIndex = imageIndex;
				node.SelectedImageIndex = node.ImageIndex;
			}
		}

		private static string FormatLongUnits(long value, string unit)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0:N0}", value);
			sb.Append(' ').Append(unit);
			if (value != 1)
			{
				sb.Append('s');
			}

			return sb.ToString();
		}

		private void Exit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ChoosePath_Click(object sender, System.EventArgs e)
		{
			string selectedPath = WindowsUtility.SelectFolder(this, "Select a directory or drive:", null);
			if (!string.IsNullOrEmpty(selectedPath))
			{
				this.PopulateTree(selectedPath);
			}
		}

		private void PopulateTree(string directoryName)
		{
			using (new WaitCursor(this))
			{
				this.Tree.BeginUpdate();
				this.Map.BeginUpdate();
				try
				{
					this.Tree.Nodes.Clear();
					this.Map.Clear();
					this.ClearFolderView();
				}
				finally
				{
					this.Tree.EndUpdate();
					this.Map.EndUpdate();
				}

				this.Text = "Disk Usage - " + directoryName;
				this.UpdateStatusBar("Scanning");
				this.stopwatch = Stopwatch.StartNew();
				this.Progress.Value = 0;
				this.RecentPaths.Add(directoryName);
				this.MainWorker.RunWorkerAsync(directoryName);
			}
		}

		private void UpdateStatusBar(TimeSpan totalTime)
		{
			const int SecondsPerMinute = 60;
			double seconds = totalTime.TotalSeconds;
			if (seconds >= SecondsPerMinute)
			{
				int minutes = ((int)seconds) / SecondsPerMinute;
				double remainingSeconds = seconds - (minutes * SecondsPerMinute);
				this.UpdateStatusBar(string.Format(
						"Total Time: {0} minute{1} {2:F2} second{3}",
						minutes,
						GetSuffix(minutes),
						remainingSeconds,
						GetSuffix(remainingSeconds)));
			}
			else
			{
				this.UpdateStatusBar(string.Format("Total Time: {0:F2} second{1}", seconds, GetSuffix(seconds)));
			}
		}

		private void UpdateStatusBar(string message)
		{
			this.lblStatus.Text = message;
		}

		private void UpdateStatusBar(TreeNode selectedNode)
		{
			if (selectedNode != null)
			{
				DirectoryData selectedData = GetDataForNode(selectedNode);
				this.UpdateStatusBar(selectedData);
			}
			else
			{
				this.UpdateStatusBar(string.Empty);
			}
		}

		private void UpdateStatusBar(DirectoryData selectedData)
		{
			string message = string.Empty;

			if (selectedData != null)
			{
				switch (selectedData.DataType)
				{
					case DirectoryDataType.Directory:
					case DirectoryDataType.Files:
						DirectoryData rootData = GetDataForNode(this.Tree.Nodes[0]);
						double percentage = rootData.Size != 0 ? selectedData.Size / (double)rootData.Size : 0;
						message = string.Format(
							"{0:F2}% of total space usage.  {1}.  {2}.  {3}.",
							100 * percentage,
							FormatLongUnits(selectedData.Size, "byte"),
							FormatLongUnits(selectedData.FileCount, "file"),
							FormatLongUnits(selectedData.FolderCount, "folder"));
						break;
				}
			}

			this.UpdateStatusBar(message);
		}

		private void Tree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			this.UpdateStatusBar(e.Node);
			this.UpdateMap(e.Node);
			this.UpdateFolderView(e.Node);
		}

		private void UpdateMap(TreeNode treeNode)
		{
			using (new WaitCursor(this))
			{
				DirectoryData data = GetDataForNode(treeNode);
				this.Map.BeginUpdate();
				try
				{
					this.Map.Clear();
					this.Map.Nodes.Add(data.TreeMapNode);
				}
				finally
				{
					this.Map.EndUpdate();
				}
			}
		}

		private void RefreshBranch_Click(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = this.Tree.SelectedNode;
			if (selectedNode != null)
			{
				// Force it to collapse first since we're populating the tree on-demand.
				selectedNode.Collapse();

				// Do the refresh asynchronously.  Make a copy of the data now, so we can
				// adjust the stats all the way up the tree later.
				DirectoryData selectedData = GetDataForNode(selectedNode);
				DirectoryData selectedDataClone = selectedData.Clone();
				this.Progress.Value = 0;
				this.RefreshWorker.RunWorkerAsync(Tuple.Create(selectedNode, selectedData, selectedDataClone));
			}
		}

		private void OpenFolder_Click(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = this.Tree.SelectedNode;
			if (selectedNode != null)
			{
				DirectoryData selectedData = GetDataForNode(selectedNode);
				if (selectedData.DataType == DirectoryDataType.Directory)
				{
					selectedData.Explore(this);
				}
			}
		}

		private void Tree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode node = this.Tree.GetNodeAt(e.X, e.Y);
			if (node != null)
			{
				TreeNode previouslySelectedNode = this.Tree.SelectedNode;
				this.Tree.SelectedNode = node;

				// If the user re-clicks the same node, then we need to resync the
				// Folder View because they probably drilled down through it and now
				// want to go back to the folder selected in the tree.
				if (previouslySelectedNode == node)
				{
					this.UpdateFolderView(node);
				}
			}

			// Force OnIdle to fire here because a right-click will popup a
			// menu before OnIdle gets a chance to enable/disable the items.
			this.OnIdle(sender, e);
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void Tree_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			TreeNode node = e.Node;

			SetDirectoryNodeImage(node);

			if (node.FirstNode is DummyNode)
			{
				node.Nodes.Clear();

				DirectoryData data = GetDataForNode(node);
				foreach (DirectoryData childData in data.SubData)
				{
					AddDirectoryNode(node.Nodes, childData);
				}
			}
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void Tree_BeforeCollapse(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			SetDirectoryNodeImage(e.Node);
		}

		private void About_Click(object sender, System.EventArgs e)
		{
			WindowsUtility.ShowAboutBox(this, Assembly.GetExecutingAssembly());
		}

		private void Drive_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = (ToolStripMenuItem)sender;
			DriveInfo drive = (DriveInfo)mi.Tag;
			this.PopulateTree(drive.RootDirectory.FullName);
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void MainWorker_DoWork(object sender, DoWorkEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			string directoryName = (string)e.Argument;

			DirectoryInfo dirInfo = new DirectoryInfo(directoryName);

			// This does all the directory walking and sizing.
			BackgroundWorker bw = (BackgroundWorker)sender;
			DirectoryData data = new DirectoryData(dirInfo, bw);

			e.Result = data;
			e.Cancel = bw.CancellationPending;
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.Progress.Value = e.ProgressPercentage;
			string directory = (string)e.UserState;
			this.UpdateStatusBar(directory);
		}

		private void MainWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			using (new WaitCursor(this))
			{
				this.Tree.BeginUpdate();
				this.Map.BeginUpdate();
				try
				{
					DirectoryData data;
					if (e.Error != null)
					{
						data = new DirectoryData(e.Error.Message);
					}
					else if (e.Cancelled)
					{
						data = new DirectoryData("Cancelled");
					}
					else
					{
						data = (DirectoryData)e.Result;
					}

					// This populates the root of the tree.
					AddDirectoryNode(this.Tree.Nodes, data);

					// Select the first tree node so the map will update.
					if (this.Tree.SelectedNode == null)
					{
						this.Tree.SelectedNode = this.Tree.Nodes[0];

						// Expand it since that's almost always desired too.
						this.Tree.SelectedNode.Expand();
					}

					// Show the total time it took.
					this.stopwatch.Stop();
					this.UpdateStatusBar(this.stopwatch.Elapsed);
				}
				finally
				{
					this.Tree.EndUpdate();
					this.Map.EndUpdate();
				}
			}
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			if (this.MainWorker.IsBusy && !this.MainWorker.CancellationPending)
			{
				this.MainWorker.CancelAsync();
			}
			else if (this.RefreshWorker.IsBusy && !this.RefreshWorker.CancellationPending)
			{
				this.RefreshWorker.CancelAsync();
			}

			this.UpdateStatusBar("Cancelling...");
		}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
		private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
#pragma warning restore CC0091 // Use static method
		{
			e.Result = e.Argument;

			var selection = (Tuple<TreeNode, DirectoryData, DirectoryData>)e.Argument;
			DirectoryData selectedData = selection.Item2;

			BackgroundWorker bw = (BackgroundWorker)sender;
			selectedData.Refresh(bw);

			e.Cancel = bw.CancellationPending;
		}

		private void RefreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			using (new WaitCursor(this))
			{
				this.Tree.BeginUpdate();
				this.Map.BeginUpdate();
				try
				{
					if (e.Error != null || e.Cancelled)
					{
						TreeNode selectedNode = this.Tree.SelectedNode;
						TreeNodeCollection nodes;
						if (selectedNode != null)
						{
							nodes = selectedNode.Nodes;
						}
						else
						{
							nodes = this.Tree.Nodes;
						}

						nodes.Clear();
						string message = e.Error != null ? e.Error.Message : "Cancelled";
						DirectoryData data = new DirectoryData(message);
						AddDirectoryNode(nodes, data);
						this.UpdateStatusBar(message);
					}
					else
					{
						var selection = (Tuple<TreeNode, DirectoryData, DirectoryData>)e.Result;
						TreeNode selectedNode = selection.Item1;
						DirectoryData selectedData = selection.Item2;
						DirectoryData oldData = selection.Item3;

						SetNodeText(selectedNode, selectedData);

						// The nodes will have to be rebuilt below it.
						selectedNode.Nodes.Clear();
						if (selectedData.SubData.Count > 0)
						{
							selectedNode.Nodes.Add(new DummyNode());
						}

						// Recalculate the stats for everything above it.
						TreeNode parentNode = selectedNode.Parent;
						long sizeAdjustment = selectedData.Size - oldData.Size;
						long fileCountAdjustment = selectedData.FileCount - oldData.FileCount;
						long folderCountAdjustment = selectedData.FolderCount - oldData.FolderCount;
						while (parentNode != null)
						{
							DirectoryData parentData = GetDataForNode(parentNode);
							parentData.AdjustStats(sizeAdjustment, fileCountAdjustment, folderCountAdjustment);
							SetNodeText(parentNode, parentData);
							parentNode = parentNode.Parent;
						}

						// Refresh the treemap
						this.UpdateMap(selectedNode);

						// Refresh the status bar
						this.UpdateStatusBar(selectedNode);

						// Expand it if it's the root node since that's almost always desired too.
						if (selectedNode == this.Tree.Nodes[0])
						{
							selectedNode.Expand();
						}
					}
				}
				finally
				{
					this.Tree.EndUpdate();
					this.Map.EndUpdate();
				}
			}
		}

		private void Map_SelectedNodeChanged(object sender, EventArgs e)
		{
			Node node = this.Map.SelectedNode;
			if (node != null)
			{
				this.UpdateStatusBar(GetDataForNode(node));
			}
			else
			{
				this.UpdateStatusBar(string.Empty);
			}
		}

		private void Drives_DropDownOpening(object sender, EventArgs e)
		{
			using (new WaitCursor(this))
			{
				this.mnuDrives.DropDownItems.Clear();

				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					if (drive.IsReady)
					{
						long totalSize = drive.TotalSize;
						double percentFree = totalSize == 0 ? 0.0 : 100 * ((double)drive.TotalFreeSpace) / totalSize;
						string text = string.Format(
							"{0} - \"{1}\" - {2} - {3:##0.0}% free",
							drive.Name,
							drive.VolumeLabel,
							drive.DriveType,
							percentFree);
						ToolStripMenuItem mi = new ToolStripMenuItem(text, DiskUsage.Properties.Resources.Drive, this.Drive_Click)
						{
							ImageTransparentColor = Color.Magenta,
							Tag = drive,
						};
						this.mnuDrives.DropDownItems.Add(mi);
					}
				}
			}
		}

		private void RecentPaths_ItemClick(object sender, RecentItemClickEventArgs e)
		{
			this.PopulateTree(e.Item);
		}

		private void Map_NodeDoubleClick(object sender, NodeEventArgs e)
		{
			// Because the tree auto-populates as its nodes are expanded,
			// we have to drill down one level at a time by parsing the path.
			// We can't assume that a given Treemap.Node will already have
			// an associated TreeViewNode.
			Node node = e.Node;
			if (node != null && this.Tree.Nodes.Count > 0)
			{
				TreeNode rootTreeNode = this.Tree.Nodes[0];
				DirectoryData rootData = GetDataForNode(rootTreeNode);

				DirectoryData data = GetDataForNode(node);
				if (data != null && rootData != null)
				{
					string fullName = data.FullName;
					string rootFullName = rootData.FullName;
					if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(rootFullName) &&
						fullName.StartsWith(rootFullName, StringComparison.CurrentCultureIgnoreCase))
					{
						// Remove the root path first.
						fullName = fullName.Remove(0, rootFullName.Length);

						// Now get the remaining path parts, so we can find their tree nodes.
						string[] parts = fullName.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

						TreeNode currentNode = rootTreeNode;
						foreach (var partName in parts)
						{
							currentNode.Expand();
							int childIndex = currentNode.Nodes.IndexOfKey(partName);
							if (childIndex >= 0)
							{
								currentNode = currentNode.Nodes[childIndex];
							}
							else
							{
								currentNode = null;
								break;
							}
						}

						if (currentNode != null)
						{
							this.Tree.SelectedNode = currentNode;
						}
					}
				}
			}
		}

		private void UpdateFolderView(TreeNode treeNode)
		{
			using (new WaitCursor(this))
			{
				DirectoryData data = GetDataForNode(treeNode);
				string directory = null;
				bool clearView = false;
				switch (data.DataType)
				{
					case DirectoryDataType.Directory:
						directory = data.FullName;
						break;
					case DirectoryDataType.Files:
						// Show the directory where the files are located.
						directory = Path.GetDirectoryName(data.FullName);
						break;
					case DirectoryDataType.Error:
						clearView = true;
						break;
				}

				if (!string.IsNullOrEmpty(directory))
				{
					// Make sure the directory is accessible for navigation.  Otherwise, the
					// WebBrowser control pops up a modal error dialog.
					if (Directory.Exists(directory))
					{
						try
						{
							// See if we can read anything in the directory (by looking for a GUID filename that should never exist).
							// This throws an exception if we can't read from the folder.
							Directory.GetFiles(directory, "4322F6AF-27BA-419C-AB4D-5FF8862B338C", SearchOption.TopDirectoryOnly);

							// When arrowing down quickly between tree nodes, the browser may not finish navigating to one folder
							// before the next navigation request comes in (since it navigates asynchronously from the UI thread).
							// To prevent a COMException from being thrown, we'll request that any pending navigation stop first.
							this.Browser.Stop();
							this.Browser.Navigate(new Uri(directory, UriKind.Absolute), false);
						}
						catch (COMException)
						{
							clearView = true;
						}
						catch (UnauthorizedAccessException)
						{
							clearView = true;
						}
					}
					else
					{
						clearView = true;
					}
				}

				if (clearView)
				{
					this.ClearFolderView();
				}
			}
		}

		private void ClearFolderView()
		{
			try
			{
				this.Browser.Stop();
				this.Browser.Url = null;
			}
#pragma warning disable CC0004 // Catch block cannot be empty
			catch (COMException)
			{
				// There's nothing we can do here.  This occurs if the browser is still navigating to the previous URI and can't stop.
			}
#pragma warning restore CC0004 // Catch block cannot be empty
		}

		#endregion

		#region Private Types

		private class DummyNode : TreeNode
		{
		}

		#endregion
	}
}
