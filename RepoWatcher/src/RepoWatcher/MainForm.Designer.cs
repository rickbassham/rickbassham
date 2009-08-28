namespace RepoWatcher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._checkNowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._projectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._removeProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._repoCheckWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._projectList = new System.Windows.Forms.TreeView();
            this._bindProjectListWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _trayIcon
            // 
            this._trayIcon.Text = "RepoWatcher";
            this._trayIcon.Visible = true;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(595, 227);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(595, 273);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(595, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            this._checkNowMenuItem,
            this._projectMenuItem,
            this._optionsMenuItem,
            this._helpMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(595, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exitMenuItem});
            this._fileMenuItem.Name = "_fileMenuItem";
            this._fileMenuItem.Size = new System.Drawing.Size(35, 20);
            this._fileMenuItem.Text = "File";
            // 
            // _exitMenuItem
            // 
            this._exitMenuItem.Name = "_exitMenuItem";
            this._exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this._exitMenuItem.Size = new System.Drawing.Size(152, 22);
            this._exitMenuItem.Text = "Exit";
            // 
            // _checkNowMenuItem
            // 
            this._checkNowMenuItem.Enabled = false;
            this._checkNowMenuItem.Name = "_checkNowMenuItem";
            this._checkNowMenuItem.Size = new System.Drawing.Size(72, 20);
            this._checkNowMenuItem.Text = "Check Now";
            // 
            // _projectMenuItem
            // 
            this._projectMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addProjectMenuItem,
            this._editProjectMenuItem,
            this._removeProjectMenuItem});
            this._projectMenuItem.Name = "_projectMenuItem";
            this._projectMenuItem.Size = new System.Drawing.Size(53, 20);
            this._projectMenuItem.Text = "Project";
            // 
            // _addProjectMenuItem
            // 
            this._addProjectMenuItem.Name = "_addProjectMenuItem";
            this._addProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this._addProjectMenuItem.Text = "Add";
            this._addProjectMenuItem.Click += new System.EventHandler(this._addProjectMenuItem_Click);
            // 
            // _editProjectMenuItem
            // 
            this._editProjectMenuItem.Enabled = false;
            this._editProjectMenuItem.Name = "_editProjectMenuItem";
            this._editProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this._editProjectMenuItem.Text = "Edit";
            // 
            // _removeProjectMenuItem
            // 
            this._removeProjectMenuItem.Enabled = false;
            this._removeProjectMenuItem.Name = "_removeProjectMenuItem";
            this._removeProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this._removeProjectMenuItem.Text = "Remove";
            // 
            // _optionsMenuItem
            // 
            this._optionsMenuItem.Name = "_optionsMenuItem";
            this._optionsMenuItem.Size = new System.Drawing.Size(56, 20);
            this._optionsMenuItem.Text = "Options";
            // 
            // _helpMenuItem
            // 
            this._helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutMenuItem});
            this._helpMenuItem.Name = "_helpMenuItem";
            this._helpMenuItem.Size = new System.Drawing.Size(40, 20);
            this._helpMenuItem.Text = "Help";
            // 
            // _aboutMenuItem
            // 
            this._aboutMenuItem.Name = "_aboutMenuItem";
            this._aboutMenuItem.Size = new System.Drawing.Size(152, 22);
            this._aboutMenuItem.Text = "About";
            // 
            // _repoCheckWorker
            // 
            this._repoCheckWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._repoCheckWorker_DoWork);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._projectList);
            this.splitContainer1.Size = new System.Drawing.Size(595, 227);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 0;
            // 
            // _projectList
            // 
            this._projectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._projectList.Location = new System.Drawing.Point(0, 0);
            this._projectList.Name = "_projectList";
            this._projectList.Size = new System.Drawing.Size(198, 227);
            this._projectList.TabIndex = 0;
            // 
            // _bindProjectListWorker
            // 
            this._bindProjectListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._bindProjectListWorker_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 273);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "MainForm";
            this.Text = "RepoWatcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon _trayIcon;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _checkNowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _projectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _addProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _editProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _removeProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _aboutMenuItem;
        private System.ComponentModel.BackgroundWorker _repoCheckWorker;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _projectList;
        private System.ComponentModel.BackgroundWorker _bindProjectListWorker;
    }
}

