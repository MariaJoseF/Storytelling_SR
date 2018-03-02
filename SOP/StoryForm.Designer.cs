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
            this.playRight = new MetroFramework.Controls.MetroTile();
            this.playLeft = new MetroFramework.Controls.MetroTile();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.closeButton.Location = new System.Drawing.Point(1266, 13);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(92, 41);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "CLOSE";
            this.closeButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.leftButton.Location = new System.Drawing.Point(256, 217);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(142, 76);
            this.leftButton.TabIndex = 17;
            this.leftButton.Text = "◀";
            this.leftButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rightButton.Location = new System.Drawing.Point(954, 217);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(142, 76);
            this.rightButton.TabIndex = 18;
            this.rightButton.Text = "▶";
            this.rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // sceneBox
            // 
            this.sceneBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneBox.Enabled = false;
            this.sceneBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.sceneBox.Location = new System.Drawing.Point(170, 12);
            this.sceneBox.Multiline = true;
            this.sceneBox.Name = "sceneBox";
            this.sceneBox.ReadOnly = true;
            this.sceneBox.Size = new System.Drawing.Size(1012, 198);
            this.sceneBox.TabIndex = 22;
            this.sceneBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sceneBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(18, 64);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(75, 23);
            this.axWindowsMediaPlayer1.TabIndex = 25;
            this.axWindowsMediaPlayer1.Visible = false;
            this.axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange_1);
            // 
            // playRight
            // 
            this.playRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.playRight.Enabled = false;
            this.playRight.Location = new System.Drawing.Point(1102, 217);
            this.playRight.Name = "playRight";
            this.playRight.Size = new System.Drawing.Size(80, 76);
            this.playRight.Style = MetroFramework.MetroColorStyle.Green;
            this.playRight.TabIndex = 24;
            this.playRight.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.playRight.TileImage = global::SOP.Properties.Resources.sound;
            this.playRight.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playRight.UseTileImage = true;
            this.playRight.Click += new System.EventHandler(this.playRight_Click);
            // 
            // playLeft
            // 
            this.playLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.playLeft.Enabled = false;
            this.playLeft.Location = new System.Drawing.Point(170, 217);
            this.playLeft.Name = "playLeft";
            this.playLeft.Size = new System.Drawing.Size(80, 76);
            this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
            this.playLeft.TabIndex = 23;
            this.playLeft.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.playLeft.TileImage = global::SOP.Properties.Resources.sound;
            this.playLeft.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playLeft.UseTileImage = true;
            this.playLeft.Click += new System.EventHandler(this.playLeft_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.playLeft);
            this.groupBox1.Controls.Add(this.leftButton);
            this.groupBox1.Controls.Add(this.sceneBox);
            this.groupBox1.Controls.Add(this.playRight);
            this.groupBox1.Controls.Add(this.rightButton);
            this.groupBox1.Controls.Add(this.lblResearcher);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(20, 508);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(1330, 305);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // lblResearcher
            // 
            this.lblResearcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResearcher.Enabled = false;
            this.lblResearcher.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.lblResearcher.Location = new System.Drawing.Point(2, 15);
            this.lblResearcher.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblResearcher.Multiline = true;
            this.lblResearcher.Name = "lblResearcher";
            this.lblResearcher.ReadOnly = true;
            this.lblResearcher.Size = new System.Drawing.Size(1326, 288);
            this.lblResearcher.TabIndex = 25;
            this.lblResearcher.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblResearcher.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.lblResearcher.Visible = false;
            // 
            // backImage
            // 
            this.backImage.BackgroundImage = global::SOP.Properties.Resources.castle;
            this.backImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backImage.Location = new System.Drawing.Point(-2, 2);
            this.backImage.Name = "backImage";
            this.backImage.Size = new System.Drawing.Size(1368, 643);
            this.backImage.TabIndex = 14;
            this.backImage.TabStop = false;
            // 
            // StoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1370, 833);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.backImage);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StoryForm";
            this.Text = "Story";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StoryForm_Load);
            this.Resize += new System.EventHandler(this.StoryForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroButton closeButton;
        private MetroFramework.Controls.MetroButton leftButton;
        private MetroFramework.Controls.MetroButton rightButton;
        private MetroFramework.Controls.MetroTextBox sceneBox;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private MetroTile playRight;
        private MetroTile playLeft;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox backImage;
        private MetroTextBox lblResearcher;
    }
}

