using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Menees.Windows.Forms;
using System.IO;
using System.Text;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System.Reflection;
using Menees;

namespace DiskUsage
{
	public partial class MainForm : ExtendedForm
	{
		private Menees.Windows.Forms.FormSaver FormSave;
		private BackgroundWorker MainWorker;
		private BackgroundWorker RefreshWorker;
		private RecentItemList RecentPaths;
		private System.Windows.Forms.TreeView Tree;
		private System.Windows.Forms.ImageList Images;
		private MenuStrip Menustrip;
		private ToolStripMenuItem mnuFile;
		private ToolStripMenuItem mnuDrives;
		private ToolStripMenuItem mnuChoosePath;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem mnuRefreshBranch;
		private ToolStripMenuItem mnuOpenFolder;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem mnuExit;
		private ToolStripMenuItem mnuHelp;
		private ToolStripMenuItem mnuAbout;
		private ContextMenuStrip TreeCtxMenu;
		private ToolStripMenuItem mnuRefreshBranch2;
		private ToolStripMenuItem mnuOpenFolder2;
		private StatusStrip Status;
		private ToolStripStatusLabel lblStatus;
		private ToolStripProgressBar Progress;
		private ToolStripMenuItem mnuCancel;
		private System.ComponentModel.IContainer components;
		private ToolStripStatusLabel lblProgressImage;
		private SplitContainer Splitter;
		private TreemapControl Map;
		private ToolStripMenuItem mnuRecentPaths;
		private ToolStripSeparator toolStripMenuItem4;
		private SplitContainer DetailSplitter;
		private WebBrowser Browser;
		private Panel BrowserPanel;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStripMenuItem mnuDummyDrive;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.Tree = new System.Windows.Forms.TreeView();
			this.TreeCtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuRefreshBranch2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolder2 = new System.Windows.Forms.ToolStripMenuItem();
			this.Images = new System.Windows.Forms.ImageList(this.components);
			this.FormSave = new Menees.Windows.Forms.FormSaver(this.components);
			this.Menustrip = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDrives = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuChoosePath = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentPaths = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRefreshBranch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.Status = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblProgressImage = new System.Windows.Forms.ToolStripStatusLabel();
			this.Progress = new System.Windows.Forms.ToolStripProgressBar();
			this.MainWorker = new System.ComponentModel.BackgroundWorker();
			this.RefreshWorker = new System.ComponentModel.BackgroundWorker();
			this.Splitter = new System.Windows.Forms.SplitContainer();
			this.DetailSplitter = new System.Windows.Forms.SplitContainer();
			this.Map = new Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl();
			this.BrowserPanel = new System.Windows.Forms.Panel();
			this.Browser = new System.Windows.Forms.WebBrowser();
			this.RecentPaths = new Menees.Windows.Forms.RecentItemList(this.components);
			mnuDummyDrive = new System.Windows.Forms.ToolStripMenuItem();
			this.TreeCtxMenu.SuspendLayout();
			this.Menustrip.SuspendLayout();
			this.Status.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Splitter)).BeginInit();
			this.Splitter.Panel1.SuspendLayout();
			this.Splitter.Panel2.SuspendLayout();
			this.Splitter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DetailSplitter)).BeginInit();
			this.DetailSplitter.Panel1.SuspendLayout();
			this.DetailSplitter.Panel2.SuspendLayout();
			this.DetailSplitter.SuspendLayout();
			this.BrowserPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuDummyDrive
			// 
			mnuDummyDrive.Enabled = false;
			mnuDummyDrive.Name = "mnuDummyDrive";
			mnuDummyDrive.Size = new System.Drawing.Size(133, 22);
			mnuDummyDrive.Text = "<Dummy>";
			// 
			// Tree
			// 
			this.Tree.ContextMenuStrip = this.TreeCtxMenu;
			this.Tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tree.HideSelection = false;
			this.Tree.ImageIndex = 0;
			this.Tree.ImageList = this.Images;
			this.Tree.Location = new System.Drawing.Point(0, 0);
			this.Tree.Name = "Tree";
			this.Tree.SelectedImageIndex = 0;
			this.Tree.Size = new System.Drawing.Size(213, 388);
			this.Tree.TabIndex = 0;
			this.Tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.Tree_BeforeCollapse);
			this.Tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.Tree_BeforeExpand);
			this.Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tree_AfterSelect);
			this.Tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tree_MouseDown);
			// 
			// TreeCtxMenu
			// 
			this.TreeCtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefreshBranch2,
            this.mnuOpenFolder2});
			this.TreeCtxMenu.Name = "TreeCtxMenu";
			this.TreeCtxMenu.Size = new System.Drawing.Size(154, 48);
			// 
			// mnuRefreshBranch2
			// 
			this.mnuRefreshBranch2.Image = global::DiskUsage.Properties.Resources.Refresh;
			this.mnuRefreshBranch2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuRefreshBranch2.Name = "mnuRefreshBranch2";
			this.mnuRefreshBranch2.Size = new System.Drawing.Size(153, 22);
			this.mnuRefreshBranch2.Text = "&Refresh Branch";
			this.mnuRefreshBranch2.Click += new System.EventHandler(this.RefreshBranch_Click);
			// 
			// mnuOpenFolder2
			// 
			this.mnuOpenFolder2.Image = global::DiskUsage.Properties.Resources.OpenFolder2;
			this.mnuOpenFolder2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuOpenFolder2.Name = "mnuOpenFolder2";
			this.mnuOpenFolder2.Size = new System.Drawing.Size(153, 22);
			this.mnuOpenFolder2.Text = "&Open Folder";
			this.mnuOpenFolder2.Click += new System.EventHandler(this.OpenFolder_Click);
			// 
			// Images
			// 
			this.Images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Images.ImageStream")));
			this.Images.TransparentColor = System.Drawing.Color.Magenta;
			this.Images.Images.SetKeyName(0, "Error.bmp");
			this.Images.Images.SetKeyName(1, "Files.bmp");
			this.Images.Images.SetKeyName(2, "OpenFolder2.bmp");
			// 
			// FormSave
			// 
			this.FormSave.ContainerControl = this;
			this.FormSave.SettingsNodeName = "Form Save";
			// 
			// Menustrip
			// 
			this.Menustrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuHelp});
			this.Menustrip.Location = new System.Drawing.Point(0, 0);
			this.Menustrip.Name = "Menustrip";
			this.Menustrip.Size = new System.Drawing.Size(542, 24);
			this.Menustrip.TabIndex = 1;
			this.Menustrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDrives,
            this.mnuChoosePath,
            this.mnuRecentPaths,
            this.toolStripMenuItem4,
            this.mnuCancel,
            this.toolStripMenuItem2,
            this.mnuRefreshBranch,
            this.mnuOpenFolder,
            this.toolStripMenuItem3,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuDrives
			// 
			this.mnuDrives.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mnuDummyDrive});
			this.mnuDrives.Name = "mnuDrives";
			this.mnuDrives.Size = new System.Drawing.Size(193, 22);
			this.mnuDrives.Text = "&Drives";
			this.mnuDrives.DropDownOpening += new System.EventHandler(this.Drives_DropDownOpening);
			// 
			// mnuChoosePath
			// 
			this.mnuChoosePath.Image = global::DiskUsage.Properties.Resources.ChoosePath;
			this.mnuChoosePath.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuChoosePath.Name = "mnuChoosePath";
			this.mnuChoosePath.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuChoosePath.Size = new System.Drawing.Size(193, 22);
			this.mnuChoosePath.Text = "Choose &Path...";
			this.mnuChoosePath.Click += new System.EventHandler(this.ChoosePath_Click);
			// 
			// mnuRecentPaths
			// 
			this.mnuRecentPaths.Name = "mnuRecentPaths";
			this.mnuRecentPaths.Size = new System.Drawing.Size(193, 22);
			this.mnuRecentPaths.Text = "R&ecent Paths";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuCancel
			// 
			this.mnuCancel.Image = global::DiskUsage.Properties.Resources.Cancel;
			this.mnuCancel.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCancel.Name = "mnuCancel";
			this.mnuCancel.Size = new System.Drawing.Size(193, 22);
			this.mnuCancel.Text = "&Cancel";
			this.mnuCancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuRefreshBranch
			// 
			this.mnuRefreshBranch.Image = global::DiskUsage.Properties.Resources.Refresh;
			this.mnuRefreshBranch.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuRefreshBranch.Name = "mnuRefreshBranch";
			this.mnuRefreshBranch.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefreshBranch.Size = new System.Drawing.Size(193, 22);
			this.mnuRefreshBranch.Text = "&Refresh Branch";
			this.mnuRefreshBranch.Click += new System.EventHandler(this.RefreshBranch_Click);
			// 
			// mnuOpenFolder
			// 
			this.mnuOpenFolder.Image = global::DiskUsage.Properties.Resources.OpenFolder2;
			this.mnuOpenFolder.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuOpenFolder.Name = "mnuOpenFolder";
			this.mnuOpenFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenFolder.Size = new System.Drawing.Size(193, 22);
			this.mnuOpenFolder.Text = "&Open Folder";
			this.mnuOpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(190, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(193, 22);
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.Exit_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(44, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(180, 22);
			this.mnuAbout.Text = "&About...";
			this.mnuAbout.Click += new System.EventHandler(this.About_Click);
			// 
			// Status
			// 
			this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblProgressImage,
            this.Progress});
			this.Status.Location = new System.Drawing.Point(0, 412);
			this.Status.Name = "Status";
			this.Status.Size = new System.Drawing.Size(542, 22);
			this.Status.TabIndex = 2;
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(527, 17);
			this.lblStatus.Spring = true;
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblProgressImage
			// 
			this.lblProgressImage.AutoSize = false;
			this.lblProgressImage.Image = global::DiskUsage.Properties.Resources.FileSearch;
			this.lblProgressImage.Name = "lblProgressImage";
			this.lblProgressImage.Size = new System.Drawing.Size(30, 17);
			this.lblProgressImage.Visible = false;
			// 
			// Progress
			// 
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(200, 16);
			this.Progress.Visible = false;
			// 
			// MainWorker
			// 
			this.MainWorker.WorkerReportsProgress = true;
			this.MainWorker.WorkerSupportsCancellation = true;
			this.MainWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MainWorker_DoWork);
			this.MainWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
			this.MainWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.MainWorker_RunWorkerCompleted);
			// 
			// RefreshWorker
			// 
			this.RefreshWorker.WorkerReportsProgress = true;
			this.RefreshWorker.WorkerSupportsCancellation = true;
			this.RefreshWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RefreshWorker_DoWork);
			this.RefreshWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
			this.RefreshWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RefreshWorker_RunWorkerCompleted);
			// 
			// Splitter
			// 
			this.Splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Splitter.Location = new System.Drawing.Point(0, 24);
			this.Splitter.Name = "Splitter";
			// 
			// Splitter.Panel1
			// 
			this.Splitter.Panel1.Controls.Add(this.Tree);
			// 
			// Splitter.Panel2
			// 
			this.Splitter.Panel2.Controls.Add(this.DetailSplitter);
			this.Splitter.Size = new System.Drawing.Size(542, 388);
			this.Splitter.SplitterDistance = 213;
			this.Splitter.TabIndex = 0;
			// 
			// DetailSplitter
			// 
			this.DetailSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DetailSplitter.Location = new System.Drawing.Point(0, 0);
			this.DetailSplitter.Name = "DetailSplitter";
			this.DetailSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// DetailSplitter.Panel1
			// 
			this.DetailSplitter.Panel1.Controls.Add(this.Map);
			// 
			// DetailSplitter.Panel2
			// 
			this.DetailSplitter.Panel2.Controls.Add(this.BrowserPanel);
			this.DetailSplitter.Size = new System.Drawing.Size(325, 388);
			this.DetailSplitter.SplitterDistance = 194;
			this.DetailSplitter.TabIndex = 0;
			// 
			// Map
			// 
			this.Map.AllowDrag = false;
			this.Map.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.Map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Map.DiscreteNegativeColors = 20;
			this.Map.DiscretePositiveColors = 20;
			this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Map.EmptySpaceLocation = Microsoft.Research.CommunityTechnologies.Treemap.EmptySpaceLocation.DeterminedByLayoutAlgorithm;
			this.Map.FontFamily = "Arial";
			this.Map.FontSolidColor = System.Drawing.Color.Black;
			this.Map.IsZoomable = false;
			this.Map.LayoutAlgorithm = Microsoft.Research.CommunityTechnologies.Treemap.LayoutAlgorithm.BottomWeightedSquarified;
			this.Map.Location = new System.Drawing.Point(0, 0);
			this.Map.MaxColor = System.Drawing.Color.DeepSkyBlue;
			this.Map.MaxColorMetric = 100F;
			this.Map.MinColor = System.Drawing.Color.ForestGreen;
			this.Map.MinColorMetric = -100F;
			this.Map.Name = "Map";
			this.Map.NodeColorAlgorithm = Microsoft.Research.CommunityTechnologies.Treemap.NodeColorAlgorithm.UseColorMetric;
			this.Map.NodeLevelsWithText = Microsoft.Research.CommunityTechnologies.Treemap.NodeLevelsWithText.All;
			this.Map.PaddingDecrementPerLevelPx = 1;
			this.Map.PaddingPx = 5;
			this.Map.PenWidthDecrementPerLevelPx = 1;
			this.Map.PenWidthPx = 3;
			this.Map.SelectedBackColor = System.Drawing.SystemColors.Highlight;
			this.Map.SelectedFontColor = System.Drawing.SystemColors.HighlightText;
			this.Map.ShowToolTips = true;
			this.Map.Size = new System.Drawing.Size(325, 194);
			this.Map.TabIndex = 0;
			this.Map.TextLocation = Microsoft.Research.CommunityTechnologies.Treemap.TextLocation.Top;
			this.Map.NodeDoubleClick += new Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl.NodeEventHandler(this.Map_NodeDoubleClick);
			this.Map.SelectedNodeChanged += new System.EventHandler(this.Map_SelectedNodeChanged);
			// 
			// BrowserPanel
			// 
			this.BrowserPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.BrowserPanel.Controls.Add(this.Browser);
			this.BrowserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BrowserPanel.Location = new System.Drawing.Point(0, 0);
			this.BrowserPanel.Name = "BrowserPanel";
			this.BrowserPanel.Size = new System.Drawing.Size(325, 190);
			this.BrowserPanel.TabIndex = 0;
			// 
			// Browser
			// 
			this.Browser.AllowWebBrowserDrop = false;
			this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.Browser.Name = "Browser";
			this.Browser.ScriptErrorsSuppressed = true;
			this.Browser.Size = new System.Drawing.Size(321, 186);
			this.Browser.TabIndex = 0;
			this.Browser.WebBrowserShortcutsEnabled = false;
			// 
			// RecentPaths
			// 
			this.RecentPaths.FormSaver = this.FormSave;
			this.RecentPaths.Items = new string[0];
			this.RecentPaths.MenuItem = this.mnuRecentPaths;
			this.RecentPaths.SettingsNodeName = "Recent Paths";
			this.RecentPaths.ItemClick += new System.EventHandler<Menees.Windows.Forms.RecentItemClickEventArgs>(this.RecentPaths_ItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.ClientSize = new System.Drawing.Size(542, 434);
			this.Controls.Add(this.Splitter);
			this.Controls.Add(this.Menustrip);
			this.Controls.Add(this.Status);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.Menustrip;
			this.Name = "MainForm";
			this.Text = "Disk Usage";
			this.TreeCtxMenu.ResumeLayout(false);
			this.Menustrip.ResumeLayout(false);
			this.Menustrip.PerformLayout();
			this.Status.ResumeLayout(false);
			this.Status.PerformLayout();
			this.Splitter.Panel1.ResumeLayout(false);
			this.Splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Splitter)).EndInit();
			this.Splitter.ResumeLayout(false);
			this.DetailSplitter.Panel1.ResumeLayout(false);
			this.DetailSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DetailSplitter)).EndInit();
			this.DetailSplitter.ResumeLayout(false);
			this.BrowserPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}

