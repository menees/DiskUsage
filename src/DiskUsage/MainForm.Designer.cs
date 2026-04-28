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
		private ToolStripMenuItem mnuFolder;
		private ToolStripMenuItem mnuDrives;
		private ToolStripMenuItem mnuChoosePath;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem mnuRefreshBranch;
		private ToolStripMenuItem mnuOpenFolderInFileExplorer;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem mnuExit;
		private ToolStripMenuItem mnuHelp;
		private ToolStripMenuItem mnuAbout;
		private ContextMenuStrip TreeCtxMenu;
		private ToolStripMenuItem mnuRefreshBranch2;
		private ToolStripMenuItem mnuOpenFolderInFileExplorer2;
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
			this.Images = new System.Windows.Forms.ImageList(this.components);
			this.FormSave = new Menees.Windows.Forms.FormSaver(this.components);
			this.Menustrip = new System.Windows.Forms.MenuStrip();
			this.mnuFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDrives = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRecentPaths = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCopyFolderName = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyFolderPath = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.Status = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.Progress = new System.Windows.Forms.ToolStripProgressBar();
			this.MainWorker = new System.ComponentModel.BackgroundWorker();
			this.RefreshWorker = new System.ComponentModel.BackgroundWorker();
			this.Splitter = new System.Windows.Forms.SplitContainer();
			this.DetailSplitter = new System.Windows.Forms.SplitContainer();
			this.BrowserPanel = new System.Windows.Forms.Panel();
			this.Browser = new System.Windows.Forms.WebBrowser();
			this.RecentPaths = new Menees.Windows.Forms.RecentItemList(this.components);
			this.mnuCopyFolderName2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyFolderPath2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.Map = new Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl();
			this.mnuRefreshBranch2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolderInFileExplorer2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolderInTerminal2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSelectFolderInFileExplorer2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteFolder2 = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuChoosePath = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRefreshBranch = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolderInFileExplorer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenFolderInTerminal = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSelectFolderInFileExplorer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.lblProgressImage = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.mnuOpenFolderInFileExplorer2,
            this.mnuOpenFolderInTerminal2,
            this.mnuSelectFolderInFileExplorer2,
            this.toolStripMenuItem5,
            this.mnuDeleteFolder2,
            this.toolStripMenuItem1,
            this.mnuCopyFolderName2,
            this.mnuCopyFolderPath2});
			this.TreeCtxMenu.Name = "TreeCtxMenu";
			this.TreeCtxMenu.Size = new System.Drawing.Size(221, 192);
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
            this.mnuFolder,
            this.mnuHelp});
			this.Menustrip.Location = new System.Drawing.Point(0, 0);
			this.Menustrip.Name = "Menustrip";
			this.Menustrip.Size = new System.Drawing.Size(542, 24);
			this.Menustrip.TabIndex = 1;
			this.Menustrip.Text = "menuStrip1";
			// 
			// mnuFolder
			// 
			this.mnuFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDrives,
            this.mnuChoosePath,
            this.mnuRecentPaths,
            this.toolStripMenuItem4,
            this.mnuCancel,
            this.toolStripMenuItem2,
            this.mnuRefreshBranch,
            this.mnuOpenFolderInFileExplorer,
            this.mnuOpenFolderInTerminal,
            this.mnuSelectFolderInFileExplorer,
            this.toolStripMenuItem7,
            this.mnuDeleteFolder,
            this.toolStripMenuItem6,
            this.mnuCopyFolderName,
            this.mnuCopyFolderPath,
            this.toolStripMenuItem3,
            this.mnuExit});
			this.mnuFolder.Name = "mnuFolder";
			this.mnuFolder.Size = new System.Drawing.Size(52, 20);
			this.mnuFolder.Text = "&Folder";
			// 
			// mnuDrives
			// 
			this.mnuDrives.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mnuDummyDrive});
			this.mnuDrives.Name = "mnuDrives";
			this.mnuDrives.Size = new System.Drawing.Size(276, 22);
			this.mnuDrives.Text = "&Drives";
			this.mnuDrives.DropDownOpening += new System.EventHandler(this.Drives_DropDownOpening);
			// 
			// mnuRecentPaths
			// 
			this.mnuRecentPaths.Name = "mnuRecentPaths";
			this.mnuRecentPaths.Size = new System.Drawing.Size(276, 22);
			this.mnuRecentPaths.Text = "R&ecent Paths";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(273, 6);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(273, 6);
			// 
			// mnuCopyFolderName
			// 
			this.mnuCopyFolderName.Image = global::DiskUsage.Properties.Resources.Copy;
			this.mnuCopyFolderName.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCopyFolderName.Name = "mnuCopyFolderName";
			this.mnuCopyFolderName.Size = new System.Drawing.Size(276, 22);
			this.mnuCopyFolderName.Text = "Copy Folder &Name";
			this.mnuCopyFolderName.Click += new System.EventHandler(this.CopyFolderName_Click);
			// 
			// mnuCopyFolderPath
			// 
			this.mnuCopyFolderPath.Image = global::DiskUsage.Properties.Resources.CopyFolder;
			this.mnuCopyFolderPath.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCopyFolderPath.Name = "mnuCopyFolderPath";
			this.mnuCopyFolderPath.Size = new System.Drawing.Size(276, 22);
			this.mnuCopyFolderPath.Text = "Copy Folder P&ath";
			this.mnuCopyFolderPath.Click += new System.EventHandler(this.CopyFolderPath_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(273, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(276, 22);
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
			this.mnuAbout.Size = new System.Drawing.Size(116, 22);
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
			this.RecentPaths.MenuItem = this.mnuRecentPaths;
			this.RecentPaths.SettingsNodeName = "Recent Paths";
			this.RecentPaths.ItemClick += new System.EventHandler<Menees.Windows.Forms.RecentItemClickEventArgs>(this.RecentPaths_ItemClick);
			// 
			// mnuCopyFolderName2
			// 
			this.mnuCopyFolderName2.Image = global::DiskUsage.Properties.Resources.Copy;
			this.mnuCopyFolderName2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCopyFolderName2.Name = "mnuCopyFolderName2";
			this.mnuCopyFolderName2.Size = new System.Drawing.Size(220, 22);
			this.mnuCopyFolderName2.Text = "Copy Folder &Name";
			this.mnuCopyFolderName2.Click += new System.EventHandler(this.CopyFolderName_Click);
			// 
			// mnuCopyFolderPath2
			// 
			this.mnuCopyFolderPath2.Image = global::DiskUsage.Properties.Resources.CopyFolder;
			this.mnuCopyFolderPath2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCopyFolderPath2.Name = "mnuCopyFolderPath2";
			this.mnuCopyFolderPath2.Size = new System.Drawing.Size(220, 22);
			this.mnuCopyFolderPath2.Text = "Copy Folder P&ath";
			this.mnuCopyFolderPath2.Click += new System.EventHandler(this.CopyFolderPath_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 6);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(217, 6);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(273, 6);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(273, 6);
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
			// mnuRefreshBranch2
			// 
			this.mnuRefreshBranch2.Image = global::DiskUsage.Properties.Resources.Refresh;
			this.mnuRefreshBranch2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuRefreshBranch2.Name = "mnuRefreshBranch2";
			this.mnuRefreshBranch2.Size = new System.Drawing.Size(220, 22);
			this.mnuRefreshBranch2.Text = "&Refresh Branch";
			this.mnuRefreshBranch2.Click += new System.EventHandler(this.RefreshBranch_Click);
			// 
			// mnuOpenFolderInFileExplorer2
			// 
			this.mnuOpenFolderInFileExplorer2.Image = global::DiskUsage.Properties.Resources.OpenFolder;
			this.mnuOpenFolderInFileExplorer2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuOpenFolderInFileExplorer2.Name = "mnuOpenFolderInFileExplorer2";
			this.mnuOpenFolderInFileExplorer2.Size = new System.Drawing.Size(220, 22);
			this.mnuOpenFolderInFileExplorer2.Text = "Open Folder In File &Explorer";
			this.mnuOpenFolderInFileExplorer2.Click += new System.EventHandler(this.OpenFolderInFileExplorer_Click);
			// 
			// mnuOpenFolderInTerminal2
			// 
			this.mnuOpenFolderInTerminal2.Image = global::DiskUsage.Properties.Resources.Terminal;
			this.mnuOpenFolderInTerminal2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mnuOpenFolderInTerminal2.Name = "mnuOpenFolderInTerminal2";
			this.mnuOpenFolderInTerminal2.Size = new System.Drawing.Size(220, 22);
			this.mnuOpenFolderInTerminal2.Text = "Open Folder In &Terminal";
			this.mnuOpenFolderInTerminal2.Click += new System.EventHandler(this.OpenFolderInTerminal_Click);
			// 
			// mnuSelectFolderInFileExplorer2
			// 
			this.mnuSelectFolderInFileExplorer2.Image = global::DiskUsage.Properties.Resources.SelectInParent;
			this.mnuSelectFolderInFileExplorer2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuSelectFolderInFileExplorer2.Name = "mnuSelectFolderInFileExplorer2";
			this.mnuSelectFolderInFileExplorer2.Size = new System.Drawing.Size(220, 22);
			this.mnuSelectFolderInFileExplorer2.Text = "&Select Folder In File Explorer";
			this.mnuSelectFolderInFileExplorer2.Click += new System.EventHandler(this.SelectFolderInFileExplorer_Click);
			// 
			// mnuDeleteFolder2
			// 
			this.mnuDeleteFolder2.Image = global::DiskUsage.Properties.Resources.DeleteFolder;
			this.mnuDeleteFolder2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuDeleteFolder2.Name = "mnuDeleteFolder2";
			this.mnuDeleteFolder2.Size = new System.Drawing.Size(220, 22);
			this.mnuDeleteFolder2.Text = "De&lete Folder";
			this.mnuDeleteFolder2.Click += new System.EventHandler(this.DeleteFolder_Click);
			// 
			// mnuChoosePath
			// 
			this.mnuChoosePath.Image = global::DiskUsage.Properties.Resources.ChoosePath;
			this.mnuChoosePath.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuChoosePath.Name = "mnuChoosePath";
			this.mnuChoosePath.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuChoosePath.Size = new System.Drawing.Size(276, 22);
			this.mnuChoosePath.Text = "Choose &Path...";
			this.mnuChoosePath.Click += new System.EventHandler(this.ChoosePath_Click);
			// 
			// mnuCancel
			// 
			this.mnuCancel.Image = global::DiskUsage.Properties.Resources.Cancel;
			this.mnuCancel.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuCancel.Name = "mnuCancel";
			this.mnuCancel.Size = new System.Drawing.Size(276, 22);
			this.mnuCancel.Text = "&Cancel";
			this.mnuCancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// mnuRefreshBranch
			// 
			this.mnuRefreshBranch.Image = global::DiskUsage.Properties.Resources.Refresh;
			this.mnuRefreshBranch.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuRefreshBranch.Name = "mnuRefreshBranch";
			this.mnuRefreshBranch.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mnuRefreshBranch.Size = new System.Drawing.Size(276, 22);
			this.mnuRefreshBranch.Text = "&Refresh Branch";
			this.mnuRefreshBranch.Click += new System.EventHandler(this.RefreshBranch_Click);
			// 
			// mnuOpenFolderInFileExplorer
			// 
			this.mnuOpenFolderInFileExplorer.Image = global::DiskUsage.Properties.Resources.OpenFolder;
			this.mnuOpenFolderInFileExplorer.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.mnuOpenFolderInFileExplorer.Name = "mnuOpenFolderInFileExplorer";
			this.mnuOpenFolderInFileExplorer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenFolderInFileExplorer.Size = new System.Drawing.Size(276, 22);
			this.mnuOpenFolderInFileExplorer.Text = "Open Folder In &File Explorer";
			this.mnuOpenFolderInFileExplorer.Click += new System.EventHandler(this.OpenFolderInFileExplorer_Click);
			// 
			// mnuOpenFolderInTerminal
			// 
			this.mnuOpenFolderInTerminal.Image = global::DiskUsage.Properties.Resources.Terminal;
			this.mnuOpenFolderInTerminal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mnuOpenFolderInTerminal.Name = "mnuOpenFolderInTerminal";
			this.mnuOpenFolderInTerminal.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
			this.mnuOpenFolderInTerminal.Size = new System.Drawing.Size(276, 22);
			this.mnuOpenFolderInTerminal.Text = "Open Folder In &Terminal";
			this.mnuOpenFolderInTerminal.Click += new System.EventHandler(this.OpenFolderInTerminal_Click);
			// 
			// mnuSelectFolderInFileExplorer
			// 
			this.mnuSelectFolderInFileExplorer.Image = global::DiskUsage.Properties.Resources.SelectInParent;
			this.mnuSelectFolderInFileExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mnuSelectFolderInFileExplorer.Name = "mnuSelectFolderInFileExplorer";
			this.mnuSelectFolderInFileExplorer.Size = new System.Drawing.Size(276, 22);
			this.mnuSelectFolderInFileExplorer.Text = "&Select Folder In File Explorer";
			this.mnuSelectFolderInFileExplorer.Click += new System.EventHandler(this.SelectFolderInFileExplorer_Click);
			// 
			// mnuDeleteFolder
			// 
			this.mnuDeleteFolder.Image = global::DiskUsage.Properties.Resources.DeleteFolder;
			this.mnuDeleteFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mnuDeleteFolder.Name = "mnuDeleteFolder";
			this.mnuDeleteFolder.Size = new System.Drawing.Size(276, 22);
			this.mnuDeleteFolder.Text = "De&lete Folder";
			this.mnuDeleteFolder.Click += new System.EventHandler(this.DeleteFolder_Click);
			// 
			// lblProgressImage
			// 
			this.lblProgressImage.AutoSize = false;
			this.lblProgressImage.Image = global::DiskUsage.Properties.Resources.FileSearch;
			this.lblProgressImage.Name = "lblProgressImage";
			this.lblProgressImage.Size = new System.Drawing.Size(30, 17);
			this.lblProgressImage.Visible = false;
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

		private ToolStripMenuItem mnuOpenFolderInTerminal;
		private ToolStripMenuItem mnuDeleteFolder;
		private ToolStripMenuItem mnuSelectFolderInFileExplorer;
		private ToolStripMenuItem mnuOpenFolderInTerminal2;
		private ToolStripMenuItem mnuSelectFolderInFileExplorer2;
		private ToolStripMenuItem mnuDeleteFolder2;
		private ToolStripMenuItem mnuCopyFolderName;
		private ToolStripMenuItem mnuCopyFolderPath;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem mnuCopyFolderName2;
		private ToolStripMenuItem mnuCopyFolderPath2;
		private ToolStripSeparator toolStripMenuItem7;
		private ToolStripSeparator toolStripMenuItem6;
	}
}

