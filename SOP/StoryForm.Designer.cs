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
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeButton.Location = new System.Drawing.Point(1838, 0);
            this.closeButton.Margin = new System.Windows.Forms.Padding(6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(89, 35);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "CLOSE";
            this.closeButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.leftButton.AutoSize = true;
            this.leftButton.Location = new System.Drawing.Point(10, 385);
            this.leftButton.Margin = new System.Windows.Forms.Padding(6);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(710, 70);
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
            this.rightButton.Location = new System.Drawing.Point(1134, 385);
            this.rightButton.Margin = new System.Windows.Forms.Padding(6);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(710, 70);
            this.rightButton.TabIndex = 18;
            this.rightButton.Text = "▶";
            this.rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.rightButton.Visible = false;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // sceneBox
            // 
            this.sceneBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sceneBox.Enabled = false;
            this.sceneBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.sceneBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sceneBox.Location = new System.Drawing.Point(6, 15);
            this.sceneBox.Margin = new System.Windows.Forms.Padding(6);
            this.sceneBox.Multiline = true;
            this.sceneBox.Name = "sceneBox";
            this.sceneBox.ReadOnly = true;
            this.sceneBox.Size = new System.Drawing.Size(1844, 242);
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
            this.groupBox1.Location = new System.Drawing.Point(36, 582);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1856, 463);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // labelRightButton
            // 
            this.labelRightButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelRightButton.BackColor = System.Drawing.Color.Transparent;
            this.labelRightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRightButton.ForeColor = System.Drawing.SystemColors.Window;
            this.labelRightButton.Location = new System.Drawing.Point(1136, 257);
            this.labelRightButton.Name = "labelRightButton";
            this.labelRightButton.Size = new System.Drawing.Size(710, 122);
            this.labelRightButton.TabIndex = 28;
            this.labelRightButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLeftButton
            // 
            this.labelLeftButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelLeftButton.BackColor = System.Drawing.Color.Transparent;
            this.labelLeftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeftButton.ForeColor = System.Drawing.SystemColors.Window;
            this.labelLeftButton.Location = new System.Drawing.Point(13, 257);
            this.labelLeftButton.Name = "labelLeftButton";
            this.labelLeftButton.Size = new System.Drawing.Size(710, 122);
            this.labelLeftButton.TabIndex = 27;
            this.labelLeftButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btConfirm
            // 
            this.btConfirm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btConfirm.AutoSize = true;
            this.btConfirm.Location = new System.Drawing.Point(788, 332);
            this.btConfirm.Margin = new System.Windows.Forms.Padding(6);
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(266, 118);
            this.btConfirm.TabIndex = 26;
            this.btConfirm.Text = "CONFIRM";
            this.btConfirm.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btConfirm.Visible = false;
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // lblResearcher
            // 
            this.lblResearcher.Enabled = false;
            this.lblResearcher.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.lblResearcher.Location = new System.Drawing.Point(8, 27);
            this.lblResearcher.Margin = new System.Windows.Forms.Padding(4);
            this.lblResearcher.Multiline = true;
            this.lblResearcher.Name = "lblResearcher";
            this.lblResearcher.ReadOnly = true;
            this.lblResearcher.Size = new System.Drawing.Size(1840, 235);
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
            this.backImage.Location = new System.Drawing.Point(0, 0);
            this.backImage.Margin = new System.Windows.Forms.Padding(6);
            this.backImage.Name = "backImage";
            this.backImage.Size = new System.Drawing.Size(1938, 640);
            this.backImage.TabIndex = 14;
            this.backImage.TabStop = false;
            // 
            // StoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1936, 1056);
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
            this.Resizable = false;
            this.Text = "Story";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StoryForm_Load);
            this.Shown += new System.EventHandler(this.StoryForm_Shown);
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
        public MetroButton btConfirm;
        private System.Windows.Forms.Label labelLeftButton;
        private System.Windows.Forms.Label labelRightButton;
        private System.Windows.Forms.PictureBox backImage;
        private MetroTextBox lblResearcher;
    }
}

