﻿namespace StoryOfPersonality
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
            this.closeButton = new MetroFramework.Controls.MetroButton();
            this.leftButton = new MetroFramework.Controls.MetroButton();
            this.rightButton = new MetroFramework.Controls.MetroButton();
            this.languageSelector = new MetroFramework.Controls.MetroComboBox();
            this.sceneBox = new MetroFramework.Controls.MetroTextBox();
            this.playRight = new MetroFramework.Controls.MetroTile();
            this.playLeft = new MetroFramework.Controls.MetroTile();
            this.backImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(1688, 16);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(123, 50);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "CLOSE";
            this.closeButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.leftButton.Location = new System.Drawing.Point(604, 700);
            this.leftButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(189, 94);
            this.leftButton.TabIndex = 17;
            this.leftButton.Text = "◀";
            this.leftButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rightButton.Location = new System.Drawing.Point(1076, 700);
            this.rightButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(189, 94);
            this.rightButton.TabIndex = 18;
            this.rightButton.Text = "▶";
            this.rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // languageSelector
            // 
            this.languageSelector.FormattingEnabled = true;
            this.languageSelector.ItemHeight = 24;
            this.languageSelector.Items.AddRange(new object[] {
            "English",
            "Portuguese"});
            this.languageSelector.Location = new System.Drawing.Point(12, 16);
            this.languageSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.languageSelector.Name = "languageSelector";
            this.languageSelector.Size = new System.Drawing.Size(160, 30);
            this.languageSelector.TabIndex = 21;
            this.languageSelector.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.languageSelector.SelectedIndexChanged += new System.EventHandler(this.LanguageSelector_SelectedIndexChanged);
            // 
            // sceneBox
            // 
            this.sceneBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneBox.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.sceneBox.Location = new System.Drawing.Point(488, 449);
            this.sceneBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sceneBox.Multiline = true;
            this.sceneBox.Name = "sceneBox";
            this.sceneBox.ReadOnly = true;
            this.sceneBox.Size = new System.Drawing.Size(892, 244);
            this.sceneBox.TabIndex = 22;
            this.sceneBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sceneBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // playRight
            // 
            this.playRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.playRight.Location = new System.Drawing.Point(1273, 700);
            this.playRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.playRight.Name = "playRight";
            this.playRight.Size = new System.Drawing.Size(107, 94);
            this.playRight.Style = MetroFramework.MetroColorStyle.Green;
            this.playRight.TabIndex = 24;
            this.playRight.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.playRight.TileImage = global::SOP.Properties.Resources.sound;
            this.playRight.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playRight.UseTileImage = true;
            this.playRight.Click += new System.EventHandler(this.PlayRight_Click);
            // 
            // playLeft
            // 
            this.playLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playLeft.Location = new System.Drawing.Point(488, 700);
            this.playLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.playLeft.Name = "playLeft";
            this.playLeft.Size = new System.Drawing.Size(107, 94);
            this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
            this.playLeft.TabIndex = 23;
            this.playLeft.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.playLeft.TileImage = global::SOP.Properties.Resources.sound;
            this.playLeft.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playLeft.UseTileImage = true;
            this.playLeft.Click += new System.EventHandler(this.PlayLeft_Click);
            // 
            // backImage
            // 
            this.backImage.BackgroundImage = global::SOP.Properties.Resources.castle;
            this.backImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backImage.Location = new System.Drawing.Point(0, 0);
            this.backImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.backImage.Name = "backImage";
            this.backImage.Size = new System.Drawing.Size(1811, 826);
            this.backImage.TabIndex = 14;
            this.backImage.TabStop = false;
            // 
            // StoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1827, 838);
            this.ControlBox = false;
            this.Controls.Add(this.playRight);
            this.Controls.Add(this.playLeft);
            this.Controls.Add(this.sceneBox);
            this.Controls.Add(this.languageSelector);
            this.Controls.Add(this.rightButton);
            this.Controls.Add(this.leftButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.backImage);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StoryForm";
            this.Padding = new System.Windows.Forms.Padding(27, 74, 27, 25);
            this.Text = "Story";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.StoryForm_Load);
            this.Resize += new System.EventHandler(this.StoryForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox backImage;
        private MetroFramework.Controls.MetroButton closeButton;
        private MetroFramework.Controls.MetroButton leftButton;
        private MetroFramework.Controls.MetroButton rightButton;
        private MetroFramework.Controls.MetroComboBox languageSelector;
        private MetroFramework.Controls.MetroTextBox sceneBox;
        private MetroFramework.Controls.MetroTile playLeft;
        private MetroFramework.Controls.MetroTile playRight;
    }
}

