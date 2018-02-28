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
using static SOP.Modules.Prosody;
using static SOP.Modules.Robot;

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
        public Personality personality;

        public EventHandler ReenableButtonsEvent;
        private bool playedLeftButton, playedRightButton = false;
        public int UserId;

        public Stopwatch stopwatch = new Stopwatch();

        private static List<Prosody> prosodyLvls = new List<Prosody>();

        public SelectionDP selectedDP;
        private Robot leftRobot;
        private Robot rightRobot;

        // at the end of the story this must be recorded to a log file. Then, we can note if the dominant robot was more clear than the assertive or not.
        // pos 0 left robot and 1 right robot.
        public int[] listenRobotAgain = new int[2];
        // 0 = the dominant robot persuade according to participant personality, 1 = different of participant personality, 2 = no persuasion at all.
        public int conditionPersuasion;

        public enum OptionSide
        {
            none = -1,
            left = 0,
            right = 1
        }

        public static StoryForm Instance { get => instance; set => instance = value; }
        public bool PlayedLeftButton { get => playedLeftButton; set => playedLeftButton = value; }
        public bool PlayedRightButton { get => playedRightButton; set => playedRightButton = value; }
        public string UserPersonalitiy1 { get => UserPersonalitiy; set => UserPersonalitiy = value; }
        internal Robot LeftRobot { get => leftRobot; set => leftRobot = value; }
        internal Robot RightRobot { get => rightRobot; set => rightRobot = value; }
        public static List<Prosody> ProsodyLvls { get => prosodyLvls; /*set => prosodyLvls = value; */}

        public StoryForm(string UserId, Client ThalamusClientRight, Client ThalamusClientLeft, Robot rightRobot, Robot leftRobot)
        {
            InitializeComponent();

            this.ThalamusClientRight = ThalamusClientRight;
            this.ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

            this.ThalamusClientLeft = ThalamusClientLeft;
            this.ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");

            this.RightRobot = rightRobot;
            this.LeftRobot = leftRobot;

            string[] aux = UserId.Split('-');
            this.UserId = Convert.ToInt32(aux[0]);

            StoryHandler = new StoryHandler(ThalamusClientLeft, this.UserId);

            this.UserPersonalitiy = aux[2];
            conditionPersuasion = Convert.ToInt32(aux[4]);

            this.ReenableButtonsEvent += new System.EventHandler(this.EnableButtons);

            personality = new Personality(this);

            LoadLogFiles();
            LoadPersuasionLvlIntensity();

            LoadScenePersuasion(leftRobot);
            LoadScenePersuasion(rightRobot);

            selectedDP = new SelectionDP();
            instance = this;
        }

        private void LoadPersuasionLvlIntensity()
        {
            RobotsLanguage language;
            int lvl, intensity;
            string rate, volume, utterance;

            StringBuilder result = new StringBuilder();
            foreach (XElement level1Element in XElement.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"/ProsodySettings.xml").Elements("Prosody"))
            {
                language = RobotsLanguage.none;
                lvl = intensity = 0;
                rate = volume = utterance = "";
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
                        //case "pitch":
                        //    pitch = level2Element.Attribute("name").Value;
                        //    break;
                        case "volume":
                            volume = level2Element.Attribute("name").Value;
                            break;
                        case "utterance":
                            utterance = level2Element.Attribute("name").Value;
                            break;
                        case "language":
                            RobotsLanguage aux = (RobotsLanguage)Enum.Parse(typeof(RobotsLanguage), level2Element.Attribute("name").Value);
                            language = aux;
                            break;
                    }
                }

                //prosodyLvls.Add(new Prosody(lvl, intensity, rate, pitch, volume, utterance, language));
                ProsodyLvls.Add(new Prosody(lvl, intensity, rate, volume, utterance, language));
            }
        }

        private void LoadLogFiles()
        {
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance;RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalAssertive;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance;ProsodyLanguage;Condition", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance; RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalAssertive;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance; ProsodyLanguage;Condition", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance; RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalAssertive;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance; ProsodyLanguage;Condition", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "CurrentStoryNodeId;OptionSelected;SideSelected;RobotPersonality;PersLvl;PersIntensity;TotalDominant;TotalAssertive;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "DecisionPoint;PreferencePair;PreferenceSelected;RobotPersonality", "ExtraInfo", "PathInfo-" + this.UserId.ToString() + ".txt");
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

            string txt = "";

            if (SidePerform(StoryHandler.GetLeftPref().ToUpper()).Equals("L"))
            {
                // PERSONALITY
                personality.BuildPersonality(StoryHandler.GetLeftPref());
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), StoryHandler.GetLeftPref(), leftRobot.Personality.ToString());
                selectedDP.DPPrefSelected = StoryHandler.GetLeftPref();
            }
            else
            {
                // PERSONALITY
                personality.BuildPersonality(StoryHandler.GetRightPref());
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), StoryHandler.GetRightPref(), leftRobot.Personality.ToString()); ;
                selectedDP.DPPrefSelected = StoryHandler.GetRightPref();
            }

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "ExtraInfo", "PathInfo-" + this.UserId.ToString() + ".txt");

            selectedDP.RobotPersonality = leftRobot.Personality;
            leftRobot.ConsecutivePlays++;
            leftRobot.OponentPlays = 0;
            rightRobot.ConsecutivePlays = 0;
            rightRobot.OponentPlays = leftRobot.ConsecutivePlays;

            if (leftRobot.Personality.Equals(Robot.RobotsPersonality.assertive))//default left robot = assertive
            {
                selectedDP.TotalAssertive++;
            }
            else //default right robot = dominant
            {
                selectedDP.TotalDominant++;
            }

            leftRobot.TotalDominant = selectedDP.TotalDominant;
            leftRobot.TotalAssertive = selectedDP.TotalAssertive;

            rightRobot.TotalDominant = selectedDP.TotalDominant;
            rightRobot.TotalAssertive = selectedDP.TotalAssertive;

            StoryHandler.NextScene(OptionSide.left, selectedDP);

            LoadScenePersuasion(leftRobot);
            LoadScenePersuasion(rightRobot);

            CallUtterancesLeft(1);

            leftRobot.Persuasion.Animation = "-";
            leftRobot.Persuasion.Prosody.Utterance = "-";
            rightRobot.Persuasion.Animation = "-";
            rightRobot.Persuasion.Prosody.Utterance = "-";

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

        public void LoadScenePersuasion(Robot robot)
        {

            Console.WriteLine("===== ROBOT " + robot.Personality + " CONSECUTIVE PLAYS: " + robot.ConsecutivePlays);
            switch (robot.Condition)
            {
                case 0:
                    //Control Condition
                    break;
                case 1:
                    //No prosody just Gaze
                    LoadGazeTime(robot, robot.Persuasion);
                    break;
                case 2:
                    //Gaze + Animation Dominant Robot
                    LoadGazeTime(robot, robot.Persuasion);
                    LoadAnimationDominant(robot);
                    break;
                case 3:
                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot that is performing it
                    //when intensity achieves value 4 the next one starts on 1

                    LoadGazeTime(robot, robot.Persuasion);
                    LoadAnimationDominant(robot);
                    robot.Persuasion.Prosody.Lvl = 2;
                    LoadPersuasionLvl2(robot);
                    GetUtteranceLanguage(robot);
                    break;
                case 4:
                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
                    //when intensity achieves value 4 the next keeps the 4


                    LoadGazeTime(robot, robot.Persuasion);
                    LoadAnimationDominant(robot);
                    robot.Persuasion.Prosody.Lvl = 3;
                    LoadPersuasionLvl3Lvl4(robot);
                    GetUtteranceLanguage(robot);
                    break;
                case 5:
                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
                    //prosody lvl 3 is stronger than lvl2
                    //when intensity achieves value 4 the next keeps the 4

                    LoadGazeTime(robot, robot.Persuasion);
                    LoadAnimationDominant(robot);
                    robot.Persuasion.Prosody.Lvl = 4;
                    LoadPersuasionLvl3Lvl4(robot);
                    GetUtteranceLanguage(robot);
                    break;
            }
        }

        private void LoadPersuasionLvl3Lvl4(Robot robot)
        {

            int robotPlays = 0;

            if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                if (robot.OponentPlays > 1)
                {
                    robotPlays = robot.OponentPlays;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("        Robot " + robot.Personality + " ConsecutivePlays = " + robot.ConsecutivePlays + " OponentPlays= " + robot.OponentPlays);
            Console.WriteLine("");

            switch (robotPlays)
            {
                case 0:
                case 1://Intensity = 1
                    robot.Persuasion.Prosody.Intensity = 1;
                    break;
                case 2://Intensity = 2
                    robot.Persuasion.Prosody.Intensity = 2;
                    break;
                case 3://Intensity = 3
                    robot.Persuasion.Prosody.Intensity = 3;
                    break;
                case 4://Intensity = 4
                    robot.Persuasion.Prosody.Intensity = 4;
                    break;
                default://Intensity = 4
                    robot.Persuasion.Prosody.Intensity = 4;
                    break;
            }
        }

        private void LoadAnimationDominant(Robot robot)
        {
            if (robot.Condition > 1)    //Condition 1 code runs also in conditions 2,3 and 4
            {
                if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
                {
                    //  <animation> only for the dominant robot
                    //annimation intensity is done according to repetition done by the robot that is performing it
                    //when intensity achieves value 4 the next keeps 4

                    if (robot.OponentPlays >= 2) // Assertive robot was the last option selected
                    {
                        switch (robot.OponentPlays)
                        {
                            case 2:
                                robot.Persuasion.Animation = "anger1";
                                break;
                            case 3:
                                robot.Persuasion.Animation = "anger3";
                                break;
                            case 4:
                                robot.Persuasion.Animation = "anger5";
                                break;
                            default:
                                if (robot.OponentPlays > 4)
                                {
                                    robot.Persuasion.Animation = "anger5";
                                }
                                break;
                        }
                    }
                    else if (robot.ConsecutivePlays >= 2)// Dominant robot was the last option selected
                    {
                        switch (robot.ConsecutivePlays)
                        {
                            case 2:
                                robot.Persuasion.Animation = "joy1";
                                break;
                            case 3:
                                robot.Persuasion.Animation = "joy3";
                                break;
                            case 4:
                                robot.Persuasion.Animation = "joy5";
                                break;
                            default:
                                if (robot.ConsecutivePlays > 4)
                                {
                                    robot.Persuasion.Animation = "joy5";
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void GetUtteranceLanguage(Robot robot)
        {
            if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                if (Language.Equals(Thalamus.BML.SpeechLanguages.English))
                {
                    robot.Persuasion.Prosody.Language = Prosody.RobotsLanguage.EN;
                    SearchProsodyLvl(robot);
                }
                else
                {
                    robot.Persuasion.Prosody.Language = Prosody.RobotsLanguage.PT;
                    SearchProsodyLvl(robot);
                }
            }
        }

        private void LoadGazeTime(Robot robot, Persuasion _persuasion)
        {
            //gaze intensity = 1 gaze to Person and then Button 
            //gaze intensity = 2 gaze to Person then Button and again to Person
            //gaze intensity = 2 gaze to Person then Button again to Person and again to Button
            //  <utterance> <gaze>
            int robotPlays = 0;







            /////////////
            /////////////
            //                      Faz sentido esta consição??
            /////////////
            /////////////

            //if (robot.ConsecutivePlays > 0)
            //{
            //    robotPlays = robot.ConsecutivePlays;
            //}
            //else
            //{
            //    robotPlays = robot.OponentPlays;
            //}













            switch (robotPlays % 3)
            {
                case 1://Intensity = 1
                    _persuasion.Gaze = "Person-Button";
                    break;
                case 2://Intensity = 2
                    _persuasion.Gaze = "Person-Button-Person";
                    break;
                case 0://Intensity = 3
                    if (robotPlays > 0)
                    {
                        _persuasion.Gaze = "Person-Button-Person-Button";
                    }
                    else
                    {
                        _persuasion.Gaze = "Person-Button";
                    }
                    break;
            }
            _persuasion.Time = GetTimeRobotFeature();
        }

        //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, assertive will look less to person 
        //if return 0 robot will perform gaze such as: domintant will look less to person,  assertive will look more to person
        private int GetTimeRobotFeature()
        {
            Random random = new Random();

            if (random.NextDouble() <= 0.8)
                return 1;

            return 0;
        }

        internal void EnableBTS(string button)
        {
            /* ONLY TO SEE IF IS OK, AFTER ALL REMOTE IT
            if (SidePerform(StoryHandler.GetLeftPref().ToUpper()).Equals("L"))
            {
                Console.WriteLine("===================== PREFS (L-R): " + StoryHandler.GetLeftPref() + " -- " + StoryHandler.GetRightPref());
            } else
            {
                Console.WriteLine("===================== PREFS (L-R): " + StoryHandler.GetRightPref() + " -- " + StoryHandler.GetLeftPref());
            }*/

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

            string txt = "";

            if (SidePerform(StoryHandler.GetRightPref().ToUpper()).Equals("R"))
            {
                // PERSONALITY
                personality.BuildPersonality(StoryHandler.GetRightPref());
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), StoryHandler.GetRightPref(), rightRobot.Personality.ToString());
                selectedDP.DPPrefSelected = StoryHandler.GetRightPref();
            }
            else
            {
                // PERSONALITY
                personality.BuildPersonality(StoryHandler.GetLeftPref());
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), StoryHandler.GetLeftPref(), rightRobot.Personality.ToString());
                selectedDP.DPPrefSelected = StoryHandler.GetLeftPref();
            }
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "ExtraInfo", "PathInfo-" + this.UserId.ToString() + ".txt");

            selectedDP.RobotPersonality = rightRobot.Personality;
            rightRobot.ConsecutivePlays++;
            rightRobot.OponentPlays = 0;
            leftRobot.ConsecutivePlays = 0;
            leftRobot.OponentPlays = rightRobot.ConsecutivePlays;

            if (rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant))//default right robot = dominant
            {
                selectedDP.TotalDominant++;
            }
            else //default left robot = assertive
            {
                selectedDP.TotalAssertive++;
            }

            rightRobot.TotalDominant = selectedDP.TotalDominant;
            rightRobot.TotalAssertive = selectedDP.TotalAssertive;

            leftRobot.TotalDominant = selectedDP.TotalDominant;
            leftRobot.TotalAssertive = selectedDP.TotalAssertive;

            StoryHandler.NextScene(OptionSide.right, selectedDP);

            LoadScenePersuasion(leftRobot);
            LoadScenePersuasion(rightRobot);

            CallUtterancesRight(1);

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

        internal void PlayLeft_Robot()
        {
            this.PlayedLeftButton = true;
            this.DisableButtons();

            CallUtterancesLeft(0);
        }

        private void CallUtterancesLeft(int leftbutton)
        {
            string fullUtterance = "";
            // string[] tags = StoryHandler.GetLeftTag().Split(',');

            if (leftbutton == 0)
            {
                if (SidePerform(StoryHandler.GetLeftPref().ToUpper()).Equals("L"))
                {
                    string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');
                    fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], leftRobot, OptionSide.left, 0);
                }
                else
                {
                    string[] utterance = StoryHandler.GetRightUtterance(this.Language).Split(',');
                    fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], leftRobot, OptionSide.left, 0);
                }
            }
            else
            {
                if (leftRobot.Personality.Equals(Robot.RobotsPersonality.dominant))
                {
                    fullUtterance = GEtUtteranceAnimationsProsodies("", leftRobot, OptionSide.left, 1);
                }
                else
                {
                    fullUtterance = GEtUtteranceAnimationsProsodies("", rightRobot, OptionSide.right, 1);
                    if (!fullUtterance.Equals(""))
                    {
                        ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                        //ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "leftRobot-" + this.UserId.ToString() + ".txt");
                        ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                        ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                    }
                }
            }

            if (leftRobot.Personality.Equals(Robot.RobotsPersonality.dominant) || leftbutton == 0)
            {
                if (!fullUtterance.Equals(""))
                {
                    ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                    //ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                }
            }
        }

        internal void PlayRight_Robot()
        {
            this.PlayedRightButton = true;
            this.DisableButtons();

            CallUtterancesRight(0);
        }

        private void CallUtterancesRight(int rightButton)
        {
            string fullUtterance = "";
            //string[] tags = StoryHandler.GetRightTag().Split(',');

            if (rightButton == 0)
            {
                if (SidePerform(StoryHandler.GetRightPref().ToUpper()).Equals("R"))
                {
                    string[] utterance = StoryHandler.GetRightUtterance(this.Language).Split(',');
                    fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], rightRobot, OptionSide.right, 0);
                }
                else
                {
                    string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');
                    fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], rightRobot, OptionSide.right, 0);
                }
            }
            else
            {
                if (rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant))
                {
                    fullUtterance = GEtUtteranceAnimationsProsodies("", rightRobot, OptionSide.right, 1);
                }
                else
                {
                    fullUtterance = GEtUtteranceAnimationsProsodies("", leftRobot, OptionSide.left, 1);
                    if (!fullUtterance.Equals(""))
                    {
                        ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                        ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                        ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                    }
                }
            }

            if (rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant) || rightButton == 0)
            {
                ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }
        }

        private string SidePerform(string prefUtterance)
        {
            string side = "";
            UserPersonalitiy = UserPersonalitiy.ToUpper();
            /*Console.WriteLine("===== USER PERSONALITY: " + UserPersonalitiy);
            Console.WriteLine("===== Robot Left personality: " + leftRobot.Personality);
            Console.WriteLine("===== Pref utterance side? : " + prefUtterance); */

            if (StoryHandler.GetInitialDP().Contains("DP"))
            {
                if (UserPersonalitiy.Contains(prefUtterance))
                {
                    if (leftRobot.Personality.Equals(RobotsPersonality.dominant))
                    {
                        side = "L";
                    }
                    else
                    {
                        side = "R";
                    }
                }
                else
                {
                    if (rightRobot.Personality.Equals(RobotsPersonality.dominant))
                    {
                        side = "L";
                    }
                    else
                    {
                        side = "R";
                    }
                }
                Console.WriteLine("===== Condition persuasion : " + conditionPersuasion + " Side: " + side);
                if (conditionPersuasion.Equals(1))
                {
                    if (side.Equals("L")) side = "R"; else side = "L";
                }
            }
            else
            {
                side = prefUtterance.ToUpper();
            }
            Console.WriteLine("===== Side : " + side);
            return side;
        }

        private string GEtUtteranceAnimationsProsodies(string utterance, Robot robotSide, OptionSide side, int buttonOption)
        {
            string aux_prosody = "";
            string animation_prosody = "";

            if (buttonOption == 0)
            {
                //No prosody just Gaze
                aux_prosody = GetGaze(robotSide, side);
                animation_prosody = utterance + aux_prosody;
            }

            //  <animation> only for the dominant robot
            if (buttonOption == 1 && (robotSide.ConsecutivePlays > 1 || robotSide.OponentPlays > 1) && robotSide.Condition > 0)
            {
                if (robotSide.Personality.Equals(RobotsPersonality.dominant) && robotSide.Condition > 1)
                {
                    aux_prosody = "<Gaze(person3)><break time=\"1s\"/><ANIMATE(" + robotSide.Persuasion.Animation + ")>";
                    animation_prosody = utterance + aux_prosody;
                }
            }

            //if buttonOption = 0 <utterance> <gaze> 
            //if buttonOption = 1 <prosody> <utterance prosody> </prosody>
            if (robotSide.Condition >= 0)
            {
                if (buttonOption == 0)
                {
                    aux_prosody = GetGaze(robotSide, side);

                    if (robotSide.Condition > 2 && robotSide.Personality.Equals(Robot.RobotsPersonality.dominant))//add prosody lvl2 to utterance
                    {
                        if (robotSide.OponentPlays > 1)
                        {
                            LoadPersuasionLvl2(robotSide);
                            GetUtteranceLanguage(robotSide);

                            aux_prosody = animation_prosody;
                            animation_prosody = aux_prosody + "<break time=\"1s\"/>" + "<prosody rate='" + robotSide.Persuasion.Prosody.Rate + "'><prosody volume='" + robotSide.Persuasion.Prosody.Volume + "'>" + robotSide.Persuasion.Prosody.Utterance + "</prosody></prosody>";
                        }
                    }
                    else
                    {
                        animation_prosody = utterance + aux_prosody;
                    }
                }
                else
                {
                    if (robotSide.Condition > 3 && robotSide.Personality.Equals(Robot.RobotsPersonality.dominant) && robotSide.OponentPlays > 1)
                    {
                        aux_prosody = animation_prosody;
                        animation_prosody = aux_prosody + "<prosody rate='" + robotSide.Persuasion.Prosody.Rate + "'><prosody volume='" + robotSide.Persuasion.Prosody.Volume + "'>" + robotSide.Persuasion.Prosody.Utterance + "</prosody></prosody>";
                    }
                }
            }

            aux_prosody = animation_prosody;

            if (!aux_prosody.Equals(""))
            {
                animation_prosody = "<prosody pitch='" + robotSide.Pitch + "'>" + aux_prosody + "</prosody>";
            }

            return animation_prosody;
        }

        private void LoadPersuasionLvl2(Robot robot)
        {
            int robotPlays = 0;

            robot.Persuasion.Prosody.Lvl = 2;

            if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                if (robot.ConsecutivePlays > 0)
                {
                    robotPlays = robot.ConsecutivePlays;
                }
                else if (robot.OponentPlays > 1)
                {
                    robotPlays = robot.OponentPlays;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("        Robot " + robot.Personality + " ConsecutivePlays = " + robot.ConsecutivePlays + " OponentPlays= " + robot.OponentPlays);
            Console.WriteLine("");


            switch (robotPlays % 4)
            {
                case 1://Intensity = 1
                    robot.Persuasion.Prosody.Intensity = 1;
                    break;
                case 2://Intensity = 2
                    robot.Persuasion.Prosody.Intensity = 2;
                    break;
                case 3://Intensity = 3
                    robot.Persuasion.Prosody.Intensity = 3;
                    break;
                case 0://Intensity = 4
                    if (robotPlays == 0)
                    {
                        robot.Persuasion.Prosody.Intensity = 1;
                    }
                    else
                    {
                        robot.Persuasion.Prosody.Intensity = 4;
                    }
                    break;
            }
        }

        private string GetGaze(Robot robotSide, OptionSide side)
        {
            string animation_prosody = "";
            string animation_person = GetPersonOrRandomGaze(robotSide);

            switch (robotSide.Persuasion.Gaze.Count(x => x == '-'))
            {
                case 1://gaze = "Person-Button"
                    if (side.Equals(OptionSide.right))
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/>";
                    }
                    else
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/>";
                    }
                    break;
                case 2://gaze = "Person-Button-Person"
                    if (side.Equals(OptionSide.right))
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(" + animation_person + ")><break time=\"1s\"/>";
                    }
                    else
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(" + animation_person + ")><break time=\"1s\"/>";
                    }
                    break;
                case 3://gaze = "Person-Button-Person-Button"
                    if (side.Equals(OptionSide.right))
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/>";
                    }
                    else
                    {
                        animation_prosody = "<Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(" + animation_person + ")><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/>";
                    }
                    break;
            }
            return animation_prosody;
        }

        private string GetPersonOrRandomGaze(Robot robotSide)
        {
            //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, assertive will look less to person 
            //if return 0 robot will perform gaze such as: domintant will look less to person,  assertive will look more to person
            string gazeTo = "";

            if (robotSide.Persuasion.Time.Equals(1))//robot will perform gaze 80% of the time
            {
                if (robotSide.Personality.Equals(Robot.RobotsPersonality.dominant))
                {
                    gazeTo = "person3";
                }
                else
                {
                    gazeTo = "random";
                }
            }
            else//robot will perform gaze 20% of time
            {
                if (robotSide.Personality.Equals(Robot.RobotsPersonality.dominant))
                {
                    gazeTo = "random";
                }
                else
                {
                    gazeTo = "person3";
                }
            }
            return gazeTo;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ThalamusClientLeft.Shutdown();
            ThalamusClientRight.Shutdown();
            Application.Exit();
        }

        internal void SearchProsodyLvl(Robot robot)
        {

            try
            {
                foreach (Prosody p in ProsodyLvls)
                {
                    if (p.Lvl == robot.Persuasion.Prosody.Lvl && p.Intensity == robot.Persuasion.Prosody.Intensity && p.Language.Equals(robot.Persuasion.Prosody.Language))
                    {
                        robot.Persuasion.Prosody.Utterance = p.Utterance;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        internal void playStoryScene(int idScene, Thalamus.BML.SpeechLanguages language)
        {
            DisableButtons();
            string folder = "EN";
            if (language == Thalamus.BML.SpeechLanguages.Portuguese) folder = "PT";

            //Console.WriteLine("=========== NEXT AUDIO ============== " + (idScene + 1));

            axWindowsMediaPlayer1.URL = ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + ".wav";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            //Console.WriteLine("URL: " + axWindowsMediaPlayer1.URL);
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
                //Console.WriteLine("=========== AUDIO FINISHED ==============");
                if (!(StoryHandler.isEnding()))
                {
                    // saber em qual lado está o robo dominante e depois ativar o botao.
                    if (leftRobot.Personality == Robot.RobotsPersonality.dominant)
                    {
                        Console.WriteLine("=========== ROBOT LEFT DOMINANT ==============");
                        PlayLeft_Robot();
                    }
                    else
                    {
                        Console.WriteLine("=========== ROBOT RIGHT DOMINANT ==============");
                        PlayRight_Robot();
                    }
                }
                else
                {
                    Console.WriteLine("============= END GAME ===============");
                }
            }
        }

        private void recordFinalLog()
        {
            string txt = "\r\n ============================= \r\n" +
                         "Left Robot Again: " + listenRobotAgain[0] + "\r\n" +
                         "Right Robot Again: " + listenRobotAgain[1] + "\r\n" +
                         "============================= \r\n" +
                         "Total Dominant: " + selectedDP.TotalDominant + "\r\n" +
                         "Total Assertive: " + selectedDP.TotalAssertive + "\r\n" +
                         "============================= \r\n";

            txt += personality.DefineMBTIPersonality();

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }
    }
}
