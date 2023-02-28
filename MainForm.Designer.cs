namespace VideoWallpaper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            siteToolStripMenuItem = new ToolStripMenuItem();
            screenToolStripMenuItem = new ToolStripMenuItem();
            switchToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            autostartToolStripMenuItem = new ToolStripMenuItem();
            donateToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "VideoWallpaper";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { siteToolStripMenuItem, screenToolStripMenuItem, switchToolStripMenuItem, toolStripSeparator1, autostartToolStripMenuItem, donateToolStripMenuItem, aboutToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(123, 164);
            // 
            // siteToolStripMenuItem
            // 
            siteToolStripMenuItem.Name = "siteToolStripMenuItem";
            siteToolStripMenuItem.Size = new Size(122, 22);
            siteToolStripMenuItem.Text = "官网";
            siteToolStripMenuItem.Click += siteToolStripMenuItem_Click;
            // 
            // screenToolStripMenuItem
            // 
            screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            screenToolStripMenuItem.Size = new Size(122, 22);
            screenToolStripMenuItem.Text = "屏幕";
            // 
            // switchToolStripMenuItem
            // 
            switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            switchToolStripMenuItem.Size = new Size(122, 22);
            switchToolStripMenuItem.Text = "切换壁纸";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(119, 6);
            // 
            // autostartToolStripMenuItem
            // 
            autostartToolStripMenuItem.Name = "autostartToolStripMenuItem";
            autostartToolStripMenuItem.Size = new Size(122, 22);
            autostartToolStripMenuItem.Text = "开机自启";
            autostartToolStripMenuItem.Click += autostartToolStripMenuItem_Click;
            // 
            // donateToolStripMenuItem
            // 
            donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            donateToolStripMenuItem.Size = new Size(122, 22);
            donateToolStripMenuItem.Text = "捐助";
            donateToolStripMenuItem.Click += donateToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(122, 22);
            aboutToolStripMenuItem.Text = "关于";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(122, 22);
            exitToolStripMenuItem.Text = "退出";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1920, 1080);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainForm";
            Opacity = 0D;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "VideoWallpaper";
            FormClosing += MainForm_FormClosing;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem siteToolStripMenuItem;
        private ToolStripMenuItem switchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem autostartToolStripMenuItem;
        private ToolStripMenuItem donateToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem screenToolStripMenuItem;
    }
}