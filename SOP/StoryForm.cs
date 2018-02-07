﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoryOfPersonality
{
    public partial class StoryForm : MetroFramework.Forms.MetroForm
    {
        public Client ThalamusClientLeft;
        public Client ThalamusClientRight;

        public Thalamus.BML.SpeechLanguages Language;
        public StoryHandler StoryHandler;

        public EventHandler ReenableButtonsEvent;
        private bool playedLeftButton, playedRightButton = false;
        public int UserId;

        public Stopwatch stopwatch = new Stopwatch();

        public StoryForm(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;

            Language = Thalamus.BML.SpeechLanguages.English;

            ThalamusClientRight = new Client(this, EMY.right, Language, "Dom");
            ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");
            // ThalamusClientRight.CPublisher.SetLanguage(Language);

            ThalamusClientLeft = new Client(this, EMY.left, Language, "Domina");
            ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");
            // ThalamusClientLeft.CPublisher.SetLanguage(Language);

            //backImage.Visible = false;
            this.ReenableButtonsEvent += new System.EventHandler(this.EnableButtons);

            StoryHandler = new StoryHandler(this.UserId);

            //wait until the characters are connected
            while (!(ThalamusClientLeft.IsConnected && ThalamusClientRight.IsConnected)) { }

            //set language to English by default
            languageSelector.SelectedIndex = languageSelector.Items.IndexOf("English");
        }

        private void CropAndStrechBackImage()
        {
            this.backImage.Size = this.Size;
            //double ratio = (double)this.backImage.BackgroundImage.Height / this.backImage.BackgroundImage.Width;
            //if (this.Width < this.backImage.BackgroundImage.Width)
            //{
            //    this.backImage.Width = this.Width;
            //    this.backImage.Height = Convert.ToInt32(this.backImage.Width * ratio);
            //    this.backImage.Location = new Point(0, (this.Height - this.backImage.Height)/2);
            //}
            //else
            //{
            //    this.backImage.Height = this.Height;
            //    this.backImage.Width = Convert.ToInt32(this.backImage.Height / ratio);
            //    this.backImage.Location = new Point((this.Width - this.backImage.Width)/2, 0);
            //}
        }

        private void DisableButtons()
        {
            this.leftButton.Enabled = this.rightButton.Enabled = this.playLeft.Enabled = this.playRight.Enabled = false;
            this.playLeft.Style = this.playRight.Style = MetroFramework.MetroColorStyle.Silver;
        }

        public void EnableButtons(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)delegate () { this.leftButton.Enabled = this.rightButton.Enabled = playedLeftButton && playedRightButton; });
            this.BeginInvoke((Action)delegate ()
            {
                this.playLeft.Enabled = this.playRight.Enabled = true;
                this.playLeft.Style = this.playRight.Style = MetroFramework.MetroColorStyle.Green;
            });
        }

        /**********************
         *    GUI EVENTS
         **********************/

        private void StoryForm_Load(object sender, EventArgs e)
        {
            this.languageSelector.Text = "English"; // put this as default
            this.leftButton.Enabled = this.rightButton.Enabled = false;
            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.CropAndStrechBackImage();
        }

        private void StoryForm_Resize(object sender, EventArgs e)
        {
            this.CropAndStrechBackImage();
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            StoryHandler.NextScene(EMY.left, stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.backImage.BackgroundImage = (Image)SOP.Properties.Resources.ResourceManager.GetObject(this.StoryHandler.GetSceneLocation());
            this.CropAndStrechBackImage();
            this.leftButton.Enabled = this.rightButton.Enabled = this.playedLeftButton = this.playedRightButton = false;
            if (StoryHandler.isEnding())
                this.DisableButtons();
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            StoryHandler.NextScene(EMY.right, stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.backImage.BackgroundImage = (Image)SOP.Properties.Resources.ResourceManager.GetObject(this.StoryHandler.GetSceneLocation());
            this.CropAndStrechBackImage();
            this.leftButton.Enabled = this.rightButton.Enabled = this.playedLeftButton = this.playedRightButton = false;
            if (StoryHandler.isEnding())
                this.DisableButtons();
        }

        private void PlayLeft_Click(object sender, EventArgs e)
        {
            this.playedLeftButton = true;
            this.DisableButtons();
            string[] tags = StoryHandler.GetLeftTag().Split(',');
            string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');

            ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), utterance[0]);

            // ThalamusClientLeft.StartUtteranceFromLibrary(StoryHandler.GetDecisionUtteranceId(), StoryHandler.GetDecisionUtteranceCategory(), tags, ReenableButtonsEvent);
        }

        private void PlayRight_Click(object sender, EventArgs e)
        {
            this.playedRightButton = true;
            this.DisableButtons();
            string[] tags = StoryHandler.GetRightTag().Split(',');
            string[] utterance = StoryHandler.GetRightUtterance(this.Language).Split(',');

            ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), utterance[0]);

            //ThalamusClientRight.StartUtteranceFromLibrary(StoryHandler.GetDecisionUtteranceId(), StoryHandler.GetDecisionUtteranceCategory(), tags, ReenableButtonsEvent);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            //ThalamusClientLeft.Disconnect("Dom");
            ThalamusClientLeft.Shutdown();
            //ThalamusClientRight.Disconnect("Domina");
            ThalamusClientRight.Shutdown();
            Application.Exit();
        }

        private void LanguageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (languageSelector.Text == "English")
            {
                this.Language = Thalamus.BML.SpeechLanguages.English;
                ThalamusClientLeft.CPublisher.SetLanguage(this.Language);
                ThalamusClientRight.CPublisher.SetLanguage(this.Language);
            }
            else
            {
                this.Language = Thalamus.BML.SpeechLanguages.Portuguese;
                ThalamusClientLeft.CPublisher.SetLanguage(this.Language);
                ThalamusClientRight.CPublisher.SetLanguage(this.Language);
            }
            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);

        }
    }
}
