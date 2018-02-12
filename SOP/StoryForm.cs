using SOP.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace StoryOfPersonality
{
    public partial class StoryForm : MetroFramework.Forms.MetroForm
    {

        private static StoryForm instance = null;

        public Client ThalamusClientLeft;
        public Client ThalamusClientRight;

        public Thalamus.BML.SpeechLanguages Language;
        public StoryHandler StoryHandler;

        public EventHandler ReenableButtonsEvent;
        private bool playedLeftButton, playedRightButton = false;
        public int UserId;

        public Stopwatch stopwatch = new Stopwatch();

        private List<Prosody> prosodyLvls = new List<Prosody>();
        public SelectionDP selectedDP;
        private RobotsPersonality assertiveRobot;
        private RobotsPersonality dominantRobot;

        public enum RobotsPersonality
        {
            left = 0,
            right = 1
        }

        public enum OptionSide
        {
            left = 0,
            right = 1
        }

        public static StoryForm Instance { get => instance; set => instance = value; }
        public bool PlayedLeftButton { get => playedLeftButton; set => playedLeftButton = value; }
        public bool PlayedRightButton { get => playedRightButton; set => playedRightButton = value; }
        internal List<Prosody> PersuasionLvls { get => prosodyLvls; set => prosodyLvls = value; }

        public StoryForm(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;

            Language = Thalamus.BML.SpeechLanguages.English;

            ThalamusClientRight = new Client(this, EMYS.right, Language, "Dom");
            ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");
            // ThalamusClientRight.CPublisher.SetLanguage(Language);

            ThalamusClientLeft = new Client(this, EMYS.left, Language, "Domina");
            ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");
            // ThalamusClientLeft.CPublisher.SetLanguage(Language);

            //backImage.Visible = false;
            this.ReenableButtonsEvent += new System.EventHandler(this.EnableButtons);

            StoryHandler = new StoryHandler(ThalamusClientLeft, this.UserId);

            //wait until the characters are connected
            while (!(ThalamusClientLeft.IsConnected && ThalamusClientRight.IsConnected)) { }

            //set language to English by default
            languageSelector.SelectedIndex = languageSelector.Items.IndexOf("English");

            LoadLogFiles();
            LoadPersuasionLvlIntensity();
            selectedDP = new SelectionDP();
            assertiveRobot = RobotsPersonality.left;
            dominantRobot = RobotsPersonality.right;
            instance = this;
        }

        private void LoadPersuasionLvlIntensity()
        {
            int lvl, intensity;
            string rate, pitch, volume;

            StringBuilder result = new StringBuilder();
            foreach (XElement level1Element in XElement.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"/ProsodySettings.xml").Elements("Prosody"))
            {
                lvl = intensity = 0;
                rate = pitch = volume = "";
                foreach (XElement level2Element in level1Element.Elements())
                {
                    switch (level2Element.Name.LocalName)
                    {
                        case "lvl":
                            lvl = Convert.ToInt32(level2Element.Attribute("name").Value);
                            break;
                        case "intensity":
                            intensity = Convert.ToInt32(level2Element.Attribute("name").Value);
                            break;
                        case "rate":
                            rate = level2Element.Attribute("name").Value;
                            break;
                        case "pitch":
                            pitch = level2Element.Attribute("name").Value;
                            break;
                        case "volume":
                            volume = level2Element.Attribute("name").Value;
                            break;
                    }
                }

                prosodyLvls.Add(new Prosody(lvl, intensity, rate, pitch, volume));
            }

        }

        private void LoadLogFiles()
        {
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "teste", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "teste", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "CurrentStoryNodeId;OptionSelected;RobotPersonality;PersLvl;PersIntensity;TotalDominant;TotalAssertive;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");

        }

        private void CropAndStrechBackImage()
        {
            this.backImage.Size = this.Size;

        }

        private void DisableButtons()
        {
            this.leftButton.Enabled = this.rightButton.Enabled = this.playLeft.Enabled = this.playRight.Enabled = false;
            this.playLeft.Style = this.playRight.Style = MetroFramework.MetroColorStyle.Silver;
        }

        public void EnableButtons(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)delegate () { this.leftButton.Enabled = this.rightButton.Enabled = PlayedLeftButton && PlayedRightButton; });
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

            selectedDP.OptionSelected = Convert.ToInt32(OptionSide.left);
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            ///////
            //Se os robôs dominante e assertivo ficarem num lugar fixo este if não é necessário
            //caso seja aleatório então temos que actualizar a variavel assertiveRobot
            //////
            if (EMYS.left.Equals(assertiveRobot))
            {
                selectedDP.TotalAssertive++;
            }
            else
            {
                selectedDP.TotalDominant++;
            }

            StoryHandler.NextScene(EMYS.left, selectedDP);

            stopwatch.Restart();

            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.backImage.BackgroundImage = (Image)SOP.Properties.Resources.ResourceManager.GetObject(this.StoryHandler.GetSceneLocation());
            this.CropAndStrechBackImage();
            this.leftButton.Enabled = this.rightButton.Enabled = this.PlayedLeftButton = this.PlayedRightButton = false;
            this.playRight.Enabled = this.playLeft.Enabled = true;
            this.playLeft.Style = this.playRight.Style = MetroFramework.MetroColorStyle.Green;
            if (StoryHandler.isEnding())
                this.DisableButtons();
        }

        internal void EnableBTS(string button)
        {
            switch (button)
            {
                case "R":
                    this.playRight.Enabled = true;
                    this.playRight.Style = MetroFramework.MetroColorStyle.Green;
                    break;
                case "L":
                    this.playLeft.Enabled = true;
                    this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
                    break;
                case "LR":
                    this.rightButton.Enabled = leftButton.Enabled = true;
                    this.rightButton.Theme = MetroFramework.MetroThemeStyle.Light;
                    this.leftButton.Theme = MetroFramework.MetroThemeStyle.Light;
                    break;
            }
        }


        private void RightButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();

            selectedDP.OptionSelected = Convert.ToInt32(OptionSide.right);
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            ///////
            //Se os robôs dominante e assertivo ficarem num lugar fixo este if não é necessário
            //caso seja aleatório então temos que que actualizar a variavel dominantRobot
            //////
            if (EMYS.right.Equals(dominantRobot))
            {
                
                selectedDP.TotalDominant++;
            }
            else
            {
                selectedDP.TotalAssertive++;
            }

            StoryHandler.NextScene(EMYS.right, selectedDP);


            stopwatch.Restart();
            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.backImage.BackgroundImage = (Image)SOP.Properties.Resources.ResourceManager.GetObject(this.StoryHandler.GetSceneLocation());
            this.CropAndStrechBackImage();
            this.leftButton.Enabled = this.rightButton.Enabled = this.PlayedLeftButton = this.PlayedRightButton = false;
            this.playRight.Enabled = this.playLeft.Enabled = true;
            this.playLeft.Style = this.playRight.Style = MetroFramework.MetroColorStyle.Green;
            if (StoryHandler.isEnding())
                this.DisableButtons();
        }


        private void PlayLeft_Click(object sender, EventArgs e)
        {
            this.PlayedLeftButton = true;
            this.DisableButtons();
            string[] tags = StoryHandler.GetLeftTag().Split(',');
            string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');

            ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), utterance[0]);
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + utterance[0], "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");


            // ThalamusClientLeft.StartUtteranceFromLibrary(StoryHandler.GetDecisionUtteranceId(), StoryHandler.GetDecisionUtteranceCategory(), tags, ReenableButtonsEvent);
        }

        private void PlayRight_Click(object sender, EventArgs e)
        {
            this.PlayedRightButton = true;
            this.DisableButtons();
            string[] tags = StoryHandler.GetRightTag().Split(',');
            string[] utterance = StoryHandler.GetRightUtterance(this.Language).Split(',');

            ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), utterance[0]);
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + utterance[0], "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");

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

        internal Prosody SearchProsodyLvl(int lvl, int intensity)
        {
            Prosody correct = new Prosody();
            try
            {
                correct = prosodyLvls.First(a => (a.Lvl == lvl) && (a.Intensity == intensity));

            }
            catch (Exception)
            {

                throw;
            }

            return correct;
        }
    }
}
