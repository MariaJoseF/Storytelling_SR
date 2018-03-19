using System;
using AxWMPLib;
using MetroFramework.Controls;

namespace StoryOfPersonality
{
    partial class StartAdventure
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
            this.languageSelector = new MetroFramework.Controls.MetroComboBox();
            this.backImage = new System.Windows.Forms.PictureBox();
            this.metroButton4Story = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.AutoSize = true;
            this.closeButton.Location = new System.Drawing.Point(1554, 25);
            this.closeButton.Margin = new System.Windows.Forms.Padding(6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(184, 79);
            this.closeButton.TabIndex = 15;
            this.closeButton.Text = "CLOSE";
            this.closeButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // languageSelector
            // 
            this.languageSelector.FormattingEnabled = true;
            this.languageSelector.ItemHeight = 23;
            this.languageSelector.Items.AddRange(new object[] {
            "English",
            "Portuguese"});
            this.languageSelector.Location = new System.Drawing.Point(18, 25);
            this.languageSelector.Margin = new System.Windows.Forms.Padding(6);
            this.languageSelector.Name = "languageSelector";
            this.languageSelector.Size = new System.Drawing.Size(238, 29);
            this.languageSelector.TabIndex = 21;
            this.languageSelector.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.languageSelector.SelectedIndexChanged += new System.EventHandler(this.LanguageSelector_SelectedIndexChanged);
            // 
            // backImage
            // 
            this.backImage.BackgroundImage = global::SOP.Properties.Resources.castle;
            this.backImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backImage.Location = new System.Drawing.Point(0, 0);
            this.backImage.Margin = new System.Windows.Forms.Padding(6);
            this.backImage.Name = "backImage";
            this.backImage.Size = new System.Drawing.Size(976, 392);
            this.backImage.TabIndex = 14;
            this.backImage.TabStop = false;
            // 
            // metroButton4Story
            // 
            this.metroButton4Story.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroButton4Story.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.metroButton4Story.Location = new System.Drawing.Point(557, 184);
            this.metroButton4Story.Margin = new System.Windows.Forms.Padding(6);
            this.metroButton4Story.Name = "metroButton4Story";
            this.metroButton4Story.Size = new System.Drawing.Size(660, 177);
            this.metroButton4Story.TabIndex = 22;
            this.metroButton4Story.Text = "STORY";
            this.metroButton4Story.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton4Story.Visible = false;
            this.metroButton4Story.Click += new System.EventHandler(this.metroButton4_Click);
            // 
            // StartAdventure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1762, 733);
            this.ControlBox = false;
            this.Controls.Add(this.metroButton4Story);
            this.Controls.Add(this.languageSelector);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.backImage);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartAdventure";
            this.Padding = new System.Windows.Forms.Padding(40, 115, 40, 38);
            this.Text = "Story";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StoryForm_Load);
            this.Resize += new System.EventHandler(this.StoryForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.backImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox backImage;
        private MetroFramework.Controls.MetroButton closeButton;
        private MetroFramework.Controls.MetroComboBox languageSelector;
        public MetroButton metroButton4Story;
    }
}

