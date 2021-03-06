﻿namespace GrabPinterest
{
    partial class Pinterest
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pinterest));
            this.console = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHidenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mynotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // console
            // 
            this.console.BackColor = System.Drawing.SystemColors.InfoText;
            this.console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.console.ForeColor = System.Drawing.SystemColors.Menu;
            this.console.Location = new System.Drawing.Point(0, 24);
            this.console.Name = "console";
            this.console.Size = new System.Drawing.Size(375, 117);
            this.console.TabIndex = 0;
            this.console.Text = "";
            this.console.TextChanged += new System.EventHandler(this.console_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.startToolStripMenuItem,
            this.killToolStripMenuItem,
            this.setUserToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadCookieToolStripMenuItem,
            this.openHiddenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(375, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // killToolStripMenuItem
            // 
            this.killToolStripMenuItem.Name = "killToolStripMenuItem";
            this.killToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // setUserToolStripMenuItem
            // 
            this.setUserToolStripMenuItem.Name = "setUserToolStripMenuItem";
            this.setUserToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.setUserToolStripMenuItem.Text = "SetUser";
            this.setUserToolStripMenuItem.Click += new System.EventHandler(this.setUserToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // loadCookieToolStripMenuItem
            // 
            this.loadCookieToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openHidenToolStripMenuItem});
            this.loadCookieToolStripMenuItem.Name = "loadCookieToolStripMenuItem";
            this.loadCookieToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadCookieToolStripMenuItem.Text = "Load";
            this.loadCookieToolStripMenuItem.Click += new System.EventHandler(this.LoadCookieToolStripMenuItem_Click);
            // 
            // openHidenToolStripMenuItem
            // 
            this.openHidenToolStripMenuItem.Name = "openHidenToolStripMenuItem";
            this.openHidenToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // openHiddenToolStripMenuItem
            // 
            this.openHiddenToolStripMenuItem.Name = "openHiddenToolStripMenuItem";
            this.openHiddenToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.openHiddenToolStripMenuItem.Text = "Open Hidden";
            this.openHiddenToolStripMenuItem.Click += new System.EventHandler(this.OpenHiddenToolStripMenuItem_Click);
            // 
            // mynotifyicon
            // 
            this.mynotifyicon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.mynotifyicon.BalloonTipText = "Pin Grabber New ";
            this.mynotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("mynotifyicon.Icon")));
            this.mynotifyicon.Text = "notifyIcon1";
            this.mynotifyicon.Visible = true;
            this.mynotifyicon.Click += new System.EventHandler(this.mynotifyicon_Click);
            // 
            // Pinterest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 141);
            this.Controls.Add(this.console);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Pinterest";
            this.Text = "Pinterest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Pinterest_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox console;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon mynotifyicon;
        private System.Windows.Forms.ToolStripMenuItem killToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCookieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openHidenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openHiddenToolStripMenuItem;
    }
}

