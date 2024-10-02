namespace OnOffBluestack
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.device_ListView = new System.Windows.Forms.ListView();
            this.log_TextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sortBluestack_Button = new System.Windows.Forms.Button();
            this.refreshDevice_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.alwaysOnTop_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countdownLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // device_ListView
            // 
            this.device_ListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.device_ListView.FullRowSelect = true;
            this.device_ListView.HideSelection = false;
            this.device_ListView.Location = new System.Drawing.Point(0, 24);
            this.device_ListView.Name = "device_ListView";
            this.device_ListView.Size = new System.Drawing.Size(362, 152);
            this.device_ListView.TabIndex = 1;
            this.device_ListView.UseCompatibleStateImageBehavior = false;
            this.device_ListView.View = System.Windows.Forms.View.Details;
            // 
            // log_TextBox
            // 
            this.log_TextBox.BackColor = System.Drawing.Color.Gainsboro;
            this.log_TextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.log_TextBox.Location = new System.Drawing.Point(0, 286);
            this.log_TextBox.Multiline = true;
            this.log_TextBox.Name = "log_TextBox";
            this.log_TextBox.ReadOnly = true;
            this.log_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log_TextBox.Size = new System.Drawing.Size(362, 93);
            this.log_TextBox.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sortBluestack_Button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 230);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 56);
            this.panel1.TabIndex = 9;
            // 
            // sortBluestack_Button
            // 
            this.sortBluestack_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.sortBluestack_Button.BackColor = System.Drawing.Color.LightGreen;
            this.sortBluestack_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortBluestack_Button.Location = new System.Drawing.Point(3, 3);
            this.sortBluestack_Button.Name = "sortBluestack_Button";
            this.sortBluestack_Button.Size = new System.Drawing.Size(356, 50);
            this.sortBluestack_Button.TabIndex = 7;
            this.sortBluestack_Button.Text = "Sắp xếp lại các tab Bluestack";
            this.sortBluestack_Button.UseVisualStyleBackColor = false;
            this.sortBluestack_Button.Click += new System.EventHandler(this.sortBluestack_Button_Click);
            // 
            // refreshDevice_ToolStripMenuItem
            // 
            this.refreshDevice_ToolStripMenuItem.Name = "refreshDevice_ToolStripMenuItem";
            this.refreshDevice_ToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.refreshDevice_ToolStripMenuItem.Text = "Refresh Device";
            this.refreshDevice_ToolStripMenuItem.Click += new System.EventHandler(this.refreshDevice_ToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshDevice_ToolStripMenuItem,
            this.alwaysOnTop_ToolStripMenuItem,
            this.dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(362, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // alwaysOnTop_ToolStripMenuItem
            // 
            this.alwaysOnTop_ToolStripMenuItem.Name = "alwaysOnTop_ToolStripMenuItem";
            this.alwaysOnTop_ToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.alwaysOnTop_ToolStripMenuItem.Text = "Always on top";
            this.alwaysOnTop_ToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTop_ToolStripMenuItem_Click);
            // 
            // dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem
            // 
            this.dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem.Name = "dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem";
            this.dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem.Size = new System.Drawing.Size(249, 20);
            this.dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem.Text = "Dùng để khởi động lại Bluestack nếu bị treo";
            // 
            // countdownLabel
            // 
            this.countdownLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countdownLabel.AutoSize = true;
            this.countdownLabel.ForeColor = System.Drawing.Color.DodgerBlue;
            this.countdownLabel.Location = new System.Drawing.Point(269, 207);
            this.countdownLabel.Name = "countdownLabel";
            this.countdownLabel.Size = new System.Drawing.Size(81, 13);
            this.countdownLabel.TabIndex = 11;
            this.countdownLabel.Text = "Waiting ... (10s)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(-3, 183);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Size = new System.Drawing.Size(363, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Dùng để sắp xếp lại các tab khi bị đè nhau, ẩn không thấy";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(362, 379);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.countdownLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.log_TextBox);
            this.Controls.Add(this.device_ListView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "On Off Bluestack by roktop.net";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView device_ListView;
        private System.Windows.Forms.TextBox log_TextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem refreshDevice_ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTop_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dùngĐểKhởiĐộngLạiBluestackNếuBịTreoToolStripMenuItem;
        private System.Windows.Forms.Button sortBluestack_Button;
        private System.Windows.Forms.Label countdownLabel;
        private System.Windows.Forms.Label label3;
    }
}

