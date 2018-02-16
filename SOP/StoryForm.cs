using AxWMPLib;
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
        private string UserPersonalitiy;
        public Thalamus.BML.SpeechLanguages Language;
        public StoryHandler StoryHandler;

        public EventHandler ReenableButtonsEvent;
        private bool playedLeftButton, playedRightButton = false;
        public int UserId;

        public Stopwatch stopwatch = new Stopwatch();

        private List<Prosody> prosodyLvls = new List<Prosody>();
        public SelectionDP selectedDP;
        private Robot leftRobot;
        private Robot rightRobot;

        // at the end of the story this must be recorded to a log file. Then, we can note if the dominant robot was more clear than the assertive or not.
        // pos 0 left robot and 1 right robot.
        public int[] listenRobotAgain = new int[2];

        //private enum RobotsPersonality
        //{
        //    none = -1,
        //    assertive = 0,
        //    dominant = 1
        //}

        public enum OptionSide
        {
            none = -1,
            left = 0,
            right = 1
        }

        public static StoryForm Instance { get => instance; set => instance = value; }
        public bool PlayedLeftButton { get => playedLeftButton; set => playedLeftButton = value; }
        public bool PlayedRightButton { get => playedRightButton; set => playedRightButton = value; }
        internal List<Prosody> PersuasionLvls { get => prosodyLvls; set => prosodyLvls = value; }
        public string UserPersonalitiy1 { get => UserPersonalitiy; set => UserPersonalitiy = value; }

        public StoryForm(string UserId)
        {
            InitializeComponent();
            string[] aux = UserId.Split('-');
            this.UserId = Convert.ToInt32(aux[0]);

            if (Convert.ToChar(aux[1][0]).Equals('1'))
            {
                rightRobot = new Robot(Robot.RobotsPersonality.dominant);
                leftRobot = new Robot(Robot.RobotsPersonality.assertive);
            }
            else
            {
                rightRobot = new Robot(Robot.RobotsPersonality.assertive);
                leftRobot = new Robot(Robot.RobotsPersonality.dominant);
            }

            this.UserPersonalitiy = aux[2];

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
            //while (!(ThalamusClientLeft.IsConnected && ThalamusClientRight.IsConnected)) { }

            //set language to English by default
            languageSelector.SelectedIndex = languageSelector.Items.IndexOf("English");

            LoadLogFiles();
            LoadPersuasionLvlIntensity();
            selectedDP = new SelectionDP();
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
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "CurrentStoryNodeId;OptionSelected;SideSelected;RobotPersonality;PersLvl;PersIntensity;TotalDominant;TotalAssertive;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
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
            playStoryScene(this.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
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
            selectedDP.SideSelected = OptionSide.left;
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            if (EMYS.left.Equals(leftRobot.Personality))
            {
                selectedDP.RobotPersonality = leftRobot.Personality;
                selectedDP.TotalAssertive++;
            }
            else
            {
                selectedDP.RobotPersonality = rightRobot.Personality;
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
            {
                this.DisableButtons();
                recordFinalLog();
            }

            playStoryScene(this.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
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
                    this.playLeft.Enabled = playRight.Enabled = true;
                    this.playRight.Style = MetroFramework.MetroColorStyle.Green;
                    this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
                    break;
            }
        }


        private void RightButton_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();

            selectedDP.OptionSelected = Convert.ToInt32(OptionSide.right);
            selectedDP.SideSelected = OptionSide.right;
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            //last time robot played
            if (selectedDP.RobotPersonality.Equals(rightRobot.Personality))
            {
                rightRobot.ConsecutivePlays++;
                leftRobot.ConsecutivePlays = 0;

            }
            else
            {
                rightRobot.ConsecutivePlays = 0;
                leftRobot.ConsecutivePlays++;
            }

            //updates current state of the robot
            if (EMYS.right.Equals(rightRobot.Personality))
            {
                selectedDP.RobotPersonality = rightRobot.Personality;
                selectedDP.TotalDominant++;
            }
            else
            {
                selectedDP.RobotPersonality = leftRobot.Personality;
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
            {
                this.DisableButtons();
                recordFinalLog();
            }
            playStoryScene(this.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
        }


        //private void PlayLeft_Click(object sender, EventArgs e)
        internal void PlayLeft_Robot()
        {
            this.PlayedLeftButton = true;
            this.DisableButtons();
            string[] tags = StoryHandler.GetLeftTag().Split(',');
            string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');

            ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), utterance[0]);
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + utterance[0], "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");

            // ThalamusClientLeft.StartUtteranceFromLibrary(StoryHandler.GetDecisionUtteranceId(), StoryHandler.GetDecisionUtteranceCategory(), tags, ReenableButtonsEvent);
        }
        // THE NAME OF THE METHOD CHANGED
        //private void PlayRight_Click(object sender, EventArgs e)
        internal void PlayRight_Robot()
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

        internal void playStoryScene(int idScene, Thalamus.BML.SpeechLanguages language)
        {
            DisableButtons();
            string folder = "EN";
            if (language == Thalamus.BML.SpeechLanguages.Portuguese) folder = "PT";

            Console.WriteLine("=========== NEXT AUDIO ==============" + (idScene+1));

            axWindowsMediaPlayer1.URL = @"speech/"+folder+"/"+(idScene+1)+".wav";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            Console.WriteLine("URL: " + axWindowsMediaPlayer1.URL);

        }

        private void playLeft_Click(object sender, EventArgs e)
        {
            listenRobotAgain[0]++;
            PlayLeft_Robot();
        }

        private void playRight_Click(object sender, EventArgs e)
        {
            listenRobotAgain[1]++;
            PlayRight_Robot();
        }

        private void axWindowsMediaPlayer1_PlayStateChange_1(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                Console.WriteLine("=========== AUDIO FINISHED ==============");
                // saber em qual lado está o robo dominante e depois ativar o botao.
                if (leftRobot.Personality == Robot.RobotsPersonality.dominant)
                {
                    Console.WriteLine("=========== ROBOT LEFT DOMINANT ==============");
                    PlayLeft_Robot();
                } else
                {
                    Console.WriteLine("=========== ROBOT RIGHT DOMINANT ==============");
                    PlayRight_Robot();
                }
            }
        }

        private void recordFinalLog()
        {
            string txt = "\r\n ============================= \r\n" +
                         "Left Robot Again: " + listenRobotAgain[0] + "\r\n" +
                         "Right Robot Again: " + listenRobotAgain[1] + "\r\n" +
                         "\r\n ============================= \r\n";

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "Extra Info", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }
    }
}
