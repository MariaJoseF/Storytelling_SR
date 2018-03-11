using MetroFramework.Controls;

namespace StoryOfPersonality
{
    partial class StoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoryForm));
            this.closeButton = new MetroFramework.Controls.MetroButton();
            this.leftButton = new MetroFramework.Controls.MetroButton();
            this.rightButton = new MetroFramework.Controls.MetroButton();
            this.sceneBox = new MetroFramework.Controls.MetroTextBox();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelRightButton = new System.Windows.Forms.Label();
            this.labelLeftButton = new System.Windows.Forms.Label();
            this.btConfirm = new MetroFramework.Controls.MetroButton();
            this.lblResearcher = new MetroFramework.Controls.MetroTextBox();
            this.backImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.closeButton.Location = new System.Drawing.Point(2532, 25);
            this.closeButton.Margin = new System.Windows.Forms.Padding(6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(184, 79);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "CLOSE";
            this.closeButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.leftButton.AutoSize = true;
            this.leftButton.Location = new System.Drawing.Point(340, 595);
            this.leftButton.Margin = new System.Windows.Forms.Padding(6);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(740, 38);
            this.leftButton.TabIndex = 17;
            this.leftButton.Text = "◀";
            this.leftButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.leftButton.Visible = false;
            this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rightButton.AutoSize = true;
            this.rightButton.Location = new System.Drawing.Point(1629, 595);
            this.rightButton.Margin = new System.Windows.Forms.Padding(6);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(735, 38);
            this.rightButton.TabIndex = 18;
            this.rightButton.Text = "▶";
            this.rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.rightButton.Visible = false;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // sceneBox
            // 
            this.sceneBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneBox.Enabled = false;
            this.sceneBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.sceneBox.Location = new System.Drawing.Point(112, 30);
            this.sceneBox.Margin = new System.Windows.Forms.Padding(6);
            this.sceneBox.Multiline = true;
            this.sceneBox.Name = "sceneBox";
            this.sceneBox.ReadOnly = true;
            this.sceneBox.Size = new System.Drawing.Size(2432, 413);
            this.sceneBox.TabIndex = 22;
            this.sceneBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sceneBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(18, 64);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(4);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(75, 23);
            this.axWindowsMediaPlayer1.TabIndex = 25;
            this.axWindowsMediaPlayer1.Visible = false;
            this.axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange_1);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.labelRightButton);
            this.groupBox1.Controls.Add(this.labelLeftButton);
            this.groupBox1.Controls.Add(this.btConfirm);
            this.groupBox1.Controls.Add(this.leftButton);
            this.groupBox1.Controls.Add(this.sceneBox);
            this.groupBox1.Controls.Add(this.rightButton);
            this.groupBox1.Controls.Add(this.lblResearcher);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(40, 905);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(2660, 659);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // labelRightButton
            // 
            this.labelRightButton.BackColor = System.Drawing.Color.Transparent;
            this.labelRightButton.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRightButton.Location = new System.Drawing.Point(1624, 449);
            this.labelRightButton.Name = "labelRightButton";
            this.labelRightButton.Size = new System.Drawing.Size(740, 134);
            this.labelRightButton.TabIndex = 28;
            this.labelRightButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelRightButton.Visible = false;
            // 
            // labelLeftButton
            // 
            this.labelLeftButton.BackColor = System.Drawing.Color.Transparent;
            this.labelLeftButton.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLeftButton.Location = new System.Drawing.Point(340, 449);
            this.labelLeftButton.Name = "labelLeftButton";
            this.labelLeftButton.Size = new System.Drawing.Size(740, 134);
            this.labelLeftButton.TabIndex = 27;
            this.labelLeftButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelLeftButton.Visible = false;
            // 
            // btConfirm
            // 
            this.btConfirm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btConfirm.Location = new System.Drawing.Point(1200, 487);
            this.btConfirm.Margin = new System.Windows.Forms.Padding(6);
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(284, 146);
            this.btConfirm.TabIndex = 26;
            this.btConfirm.Text = "CONFIRM";
            this.btConfirm.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btConfirm.Visible = false;
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // lblResearcher
            // 
            this.lblResearcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResearcher.Enabled = false;
            this.lblResearcher.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.lblResearcher.Location = new System.Drawing.Point(4, 28);
            this.lblResearcher.Margin = new System.Windows.Forms.Padding(4);
            this.lblResearcher.Multiline = true;
            this.lblResearcher.Name = "lblResearcher";
            this.lblResearcher.ReadOnly = true;
            this.lblResearcher.Size = new System.Drawing.Size(2652, 627);
            this.lblResearcher.Style = MetroFramework.MetroColorStyle.Silver;
            this.lblResearcher.TabIndex = 25;
            this.lblResearcher.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblResearcher.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.lblResearcher.Visible = false;
            // 
            // backImage
            // 
            this.backImage.BackgroundImage = global::SOP.Properties.Resources.castle;
            this.backImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backImage.Location = new System.Drawing.Point(-4, 4);
            this.backImage.Margin = new System.Windows.Forms.Padding(6);
            this.backImage.Name = "backImage";
            this.backImage.Size = new System.Drawing.Size(2736, 1237);
            this.backImage.TabIndex = 14;
            this.backImage.TabStop = false;
            // 
            // StoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(2740, 1602);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.backImage);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StoryForm";
            this.Padding = new System.Windows.Forms.Padding(40, 115, 40, 38);
            this.Text = "Story";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StoryForm_Load);
            this.Resize += new System.EventHandler(this.StoryForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroButton closeButton;
        private MetroFramework.Controls.MetroButton leftButton;
        private MetroFramework.Controls.MetroButton rightButton;
        private MetroFramework.Controls.MetroTextBox sceneBox;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox backImage;
        private MetroTextBox lblResearcher;
        public MetroButton btConfirm;
        private System.Windows.Forms.Label labelLeftButton;
        private System.Windows.Forms.Label labelRightButton;
    }
}

