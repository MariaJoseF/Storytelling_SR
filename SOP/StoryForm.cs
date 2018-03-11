using AxWMPLib;
using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using static SOP.Modules.Robot;

namespace StoryOfPersonality
{
    public partial class StoryForm : MetroFramework.Forms.MetroForm
    {
        private static StoryForm instance = null;

        public Client ThalamusClientLeft;
        public Client ThalamusClientRight;
        private string UserPersonality;
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

        // at the end of the story this must be recorded to a log file. Then, we can note if the dominant robot was more clear than the Meek or not.
        // pos 0 left robot and 1 right robot.
        public int[] listenRobotAgain = new int[2];
        // 0 = the dominant robot persuade according to participant personality, 1 = different of participant personality, 2 = no persuasion at all.
        public int playSceneAnger = 0;
        public bool btConfirmEnable = true;
        private List<Robot> PreferenceEI = new List<Robot>();
        private List<Robot> PreferenceTF = new List<Robot>();
        private List<Robot> PreferenceJP = new List<Robot>();
        private List<Robot> PreferenceSN = new List<Robot>();
        private List<Robot> preferenceDG = new List<Robot>();

        public enum OptionSide
        {
            none = -1,
            left = 0,
            right = 1
        }

        public static StoryForm Instance { get => instance; set => instance = value; }
        public bool PlayedLeftButton { get => playedLeftButton; set => playedLeftButton = value; }
        public bool PlayedRightButton { get => playedRightButton; set => playedRightButton = value; }
        public string UserPersonality1 { get => UserPersonality; set => UserPersonality = value; }
        internal Robot LeftRobot { get => leftRobot; set => leftRobot = value; }
        internal Robot RightRobot { get => rightRobot; set => rightRobot = value; }
        public static List<Prosody> ProsodyLvls { get => prosodyLvls; /*set => prosodyLvls = value; */}

        public StoryForm(string UserId, Client ThalamusClientRight, Client ThalamusClientLeft, Robot rightRobot, Robot leftRobot)
        {
            InitializeComponent();

            if (this.Language.Equals(Thalamus.BML.SpeechLanguages.English))
            {
                this.lblResearcher.Text = "Call the Researcher please.";
                this.btConfirm.Text = "Confirm";
            }
            else
            {
                this.lblResearcher.Text = "Chame o investigador por favor.";
                this.btConfirm.Text = "Confirmar";
            }

            this.ThalamusClientRight = ThalamusClientRight;
            this.ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

            this.ThalamusClientLeft = ThalamusClientLeft;
            this.ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");

            this.RightRobot = rightRobot;
            this.LeftRobot = leftRobot;

            string[] aux = UserId.Split('-');
            this.UserId = Convert.ToInt32(aux[0]);

            StoryHandler = new StoryHandler(ThalamusClientLeft, this.UserId);

            this.UserPersonality = aux[2];

            this.ReenableButtonsEvent += new System.EventHandler(this.EnableButtons);

            personality = new Personality(this);

            LoadLogFiles();


            StoryHandler.LoadPreferences(PreferenceEI, PreferenceJP, PreferenceSN, PreferenceTF, preferenceDG);
            //   LoadPersuasionLvlIntensity();

            //   LoadScenePersuasion(leftRobot);
            //     LoadScenePersuasion(rightRobot);

            selectedDP = new SelectionDP();
            instance = this;
        }


        //private void LoadPersuasionLvlIntensity()
        //{
        //    RobotsLanguage language;
        //    int lvl, intensity;
        //    string rate, volume, utterance;

        //    StringBuilder result = new StringBuilder();
        //    foreach (XElement level1Element in XElement.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"/ProsodySettings.xml").Elements("Prosody"))
        //    {
        //        language = RobotsLanguage.none;
        //        lvl = intensity = 0;
        //        rate = volume = utterance = "";
        //        foreach (XElement level2Element in level1Element.Elements())
        //        {
        //            switch (level2Element.Name.LocalName)
        //            {
        //                case "lvl":
        //                    lvl = Convert.ToInt32(level2Element.Attribute("name").Value);
        //                    break;
        //                case "intensity":
        //                    intensity = Convert.ToInt32(level2Element.Attribute("name").Value);
        //                    break;
        //                case "rate":
        //                    rate = level2Element.Attribute("name").Value;
        //                    break;
        //                //case "pitch":
        //                //    pitch = level2Element.Attribute("name").Value;
        //                //    break;
        //                case "volume":
        //                    volume = level2Element.Attribute("name").Value;
        //                    break;
        //                case "utterance":
        //                    utterance = level2Element.Attribute("name").Value;
        //                    break;
        //                case "language":
        //                    RobotsLanguage aux = (RobotsLanguage)Enum.Parse(typeof(RobotsLanguage), level2Element.Attribute("name").Value);
        //                    language = aux;
        //                    break;
        //            }
        //        }

        //        //prosodyLvls.Add(new Prosody(lvl, intensity, rate, pitch, volume, utterance, language));
        //        ProsodyLvls.Add(new Prosody(lvl, intensity, rate, volume, utterance, language));
        //    }
        //}

        private void LoadLogFiles()
        {
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance;RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;ConditionPersuasion;PersuasionCondition", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance; RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;Animation;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance; ProsodyLanguage;PersuasionCondition", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Utterance; RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;Animation;ProsodyLvl;ProsodyIntensity;ProsodyRate;ProsodyVolume;ProsodyUtterance; ProsodyLanguage;PersuasionCondition", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "CurrentStoryNodeId;OptionSelected;SideSelected;RobotPersonality;PersLvl;PersIntensity;TotalDominant;TotalMeek;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "DecisionPoint;PreferencePair;PreferenceSelected;RobotPersonality;ConditionPersuasion", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }

        private void CropAndStrechBackImage()
        {
            this.backImage.Size = this.Size;
        }

        private void DisableButtons()
        {
            this.leftButton.Enabled = this.rightButton.Enabled = false;
        }

        public void EnableButtons(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)delegate ()
            {
                this.leftButton.Enabled = this.rightButton.Enabled = true;
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
            if (btConfirmEnable)
            {
                Robot robotSide = AwakeRobot();
                ChoiceIntention(robotSide, OptionSide.left);
            }
            else
            {
                if (leftRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
                {
                    ChoiceIntention(leftRobot, OptionSide.left);
                }
                else
                {
                    ChoiceIntention(rightRobot, OptionSide.left);
                }
            }
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            if (btConfirmEnable)
            {
                Robot robotSide = AwakeRobot();
                ChoiceIntention(robotSide, OptionSide.right);
            }
            else
            {
                if (rightRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
                {
                    ChoiceIntention(rightRobot, OptionSide.left);
                }
                else
                {
                    ChoiceIntention(leftRobot, OptionSide.left);
                }
            }
        }

        private void ChoiceIntention(Robot robotSide, OptionSide optionSide)
        {
            if (btConfirmEnable)
            {
                btConfirmEnable = false;
                btConfirm.Enabled = false;
                btConfirm.Visible = true;
                string auxPreferenceSide = "";

                //gravar qual a opção que ele escolheu
                //    gravar quantas vez este robô mudou a opção a favor
                //    gravar quantas vez este robô mudou a opção a contra

                if (optionSide.Equals(OptionSide.left))
                {
                    auxPreferenceSide = StoryHandler.GetLeftPref();
                }
                else
                {
                    auxPreferenceSide = StoryHandler.GetRightPref();
                }

                selectedDP.DPPrefSelected = auxPreferenceSide;
                robotSide.PrefSelectedIntention = selectedDP.DPPrefSelected;

                Console.WriteLine("Gerar propriedades do gaze");
                LoadScenePersuasion(robotSide);

                Console.WriteLine("Activar utterance a favor ou contra");

                //GetUtteranceLanguage(robotSide);

                CallUtterances(robotSide, 2);

                Console.WriteLine("Activar gaze a favor");

            }
            else
            {
                SaveFinalPreference(robotSide, optionSide, 0);
            }
        }

        private void SaveFinalPreference(Robot robotSide, OptionSide optionSide, int intention)
        {
            string auxPreferenceSide = "";
            Console.WriteLine("gravar qual a opção que ele escolheu como final");
            Console.WriteLine("gravar quantas vez este robô mudou a opção a favor");
            Console.WriteLine("gravar quantas vez este robô mudou a opção a contra");
            stopwatch.Stop();
            DisableButtons();

            selectedDP.OptionSelected = Convert.ToInt32(optionSide);
            selectedDP.SideSelected = optionSide;
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            string txt = "";

            if (intention == 1)
            {
                auxPreferenceSide = robotSide.PrefSelectedIntention;
            }
            else
            {
                if (optionSide.Equals(OptionSide.left))
                {
                    auxPreferenceSide = StoryHandler.GetLeftPref();
                }
                else
                {
                    auxPreferenceSide = StoryHandler.GetRightPref();
                }
            }

            // PERSONALITY
            personality.BuildPersonality(auxPreferenceSide);//selecionou botão esquerda manda a preferência dessa opção
            txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), auxPreferenceSide, robotSide.Personality.ToString(), robotSide.PersuasionCondition.ToString());
            selectedDP.DPPrefSelected = auxPreferenceSide;
            robotSide.PrefSelectedFinal = selectedDP.DPPrefSelected;

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

            selectedDP.RobotPersonality = robotSide.Personality;

            robotSide.DecisionPoint = StoryHandler.GetInitialDP();
            robotSide.PreferencePair = StoryHandler.GetPrefDP();

            if (robotSide.Personality.Equals(Robot.RobotsPersonality.meek))//default left robot = Meek
            {
                selectedDP.TotalMeek++;
            }
            else //default right robot = dominant
            {
                selectedDP.TotalDominant++;
            }

            robotSide.ConsecutivePlays++;
            robotSide.OponentPlays = 0;

            if (robotSide.Side.Equals(Robot.RobotSide.left))
            {

                rightRobot.ConsecutivePlays = 0;
                rightRobot.OponentPlays = robotSide.ConsecutivePlays;

                rightRobot.PreferencePair = "-";
                rightRobot.PrefSelectedIntention = "-";
                rightRobot.PrefSelectedFinal = "-";

                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientLeft", "Robots-" + this.UserId.ToString() + ".txt");
                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");

            }
            else
            {
                leftRobot.ConsecutivePlays = 0;
                leftRobot.OponentPlays = robotSide.ConsecutivePlays;

                leftRobot.PreferencePair = "-";
                leftRobot.PrefSelectedIntention = "-";
                leftRobot.PrefSelectedFinal = "-";

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientRight", "Robots-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }

            leftRobot.TotalDominant = selectedDP.TotalDominant;
            leftRobot.TotalMeek = selectedDP.TotalMeek;

            rightRobot.TotalDominant = selectedDP.TotalDominant;
            rightRobot.TotalMeek = selectedDP.TotalMeek;

            StoryHandler.NextScene(optionSide, selectedDP);


            btConfirmEnable = true;
            btConfirm.Enabled = true;
            this.leftButton.Enabled = this.rightButton.Enabled = false;
            //CallNextScene();
        }

        private Robot AwakeRobot()
        {
            Robot per_per = new Robot();
            string preferenceSelected = StoryHandler.GetPrefDP();

            switch (preferenceSelected)
            {
                case "ei":
                case "ie":
                    per_per = PreferenceEI.First();
                    //per_per = PreferenceEI.Find(x => x.Personality.Equals(robotSide.Personality));
                    break;
                case "tf":
                case "ft":
                    per_per = PreferenceTF.First();
                    break;
                case "jp":
                case "pj":
                    per_per = PreferenceJP.First();
                    break;
                case "sn":
                case "ns":
                    per_per = PreferenceSN.First();
                    break;
                case "-":
                    per_per = preferenceDG.First();
                    break;
            }

            if (per_per.Personality.Equals(Robot.RobotsPersonality.dominant) && rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                rightRobot.Posture = RobotsPosture.pride;
                rightRobot.PersuasionCondition = per_per.PersuasionCondition;
                leftRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientRight.CPublisher.SetPosture("", rightRobot.Posture.ToString());
                return rightRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.meek) && rightRobot.Personality.Equals(Robot.RobotsPersonality.meek))
            {
                rightRobot.Posture = RobotsPosture.contempt;
                rightRobot.PersuasionCondition = per_per.PersuasionCondition;
                leftRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientRight.CPublisher.SetPosture("", rightRobot.Posture.ToString());
                return rightRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.dominant) && leftRobot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                leftRobot.Posture = RobotsPosture.pride;
                leftRobot.PersuasionCondition = per_per.PersuasionCondition;
                rightRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientLeft.CPublisher.SetPosture("", leftRobot.Posture.ToString());
                return leftRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.meek) && leftRobot.Personality.Equals(Robot.RobotsPersonality.meek))
            {
                leftRobot.Posture = RobotsPosture.contempt;
                leftRobot.PersuasionCondition = per_per.PersuasionCondition;
                rightRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientLeft.CPublisher.SetPosture("", leftRobot.Posture.ToString());
                return leftRobot;
            }
            else
            {
                return new Robot();
            }
        }

        private void CallNextScene()
        {
            stopwatch.Restart();

            /*
            Paragraph p = new Paragraph();
            p.LineHeight = 5;
            p.LineStackingStrategy = System.Windows.LineStackingStrategy.BlockLineHeight;
            p.Inlines.Add(this.StoryHandler.GetSceneUtterance(this.Language));

            p.Inlines.Add(new Run(string.Format(this.StoryHandler.GetSceneUtterance(this.Language))));
            this.sceneBox.Text = p.ToString();
            */

            this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
            this.backImage.BackgroundImage = (Image)SOP.Properties.Resources.ResourceManager.GetObject(this.StoryHandler.GetSceneLocation());
            this.CropAndStrechBackImage();
            this.leftButton.Enabled = this.rightButton.Enabled = this.PlayedLeftButton = this.PlayedRightButton = false;

            if (StoryHandler.isEnding())
            {
                this.DisableButtons();
                recordFinalLog();
            }
            playStoryScene(this.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);

            leftRobot.Persuasion.Animation = "-";
            leftRobot.Persuasion.Prosody.Utterance = "-";
            rightRobot.Persuasion.Animation = "-";
            rightRobot.Persuasion.Prosody.Utterance = "-";
        }

        public void LoadScenePersuasion(Robot robot)
        {
            //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
            //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
            //prosody lvl 3 is stronger than lvl2
            //when intensity achieves value 4 the next keeps the 4

            LoadGazeTime(robot, robot.Persuasion);

            //LoadPersuasionLvl3Lvl4(robot);
            GetUtteranceLanguage(robot);

        }

        //private void LoadPersuasionLvl3Lvl4(Robot robot)
        //{

        //    int robotPlays = 0;

        //    if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
        //    {
        //        if (robot.OponentPlays > 1)
        //        {
        //            robotPlays = robot.OponentPlays;
        //        }
        //    }

        //    Console.WriteLine("");
        //    Console.WriteLine("        Robot " + robot.Personality + " ConsecutivePlays = " + robot.ConsecutivePlays + " OponentPlays= " + robot.OponentPlays);
        //    Console.WriteLine("");

        //    switch (robotPlays)
        //    {
        //        case 0://Intensity = 1
        //        case 1://Intensity = 1
        //        case 2://Intensity = 1
        //            robot.Persuasion.Prosody.Intensity = 1;
        //            break;
        //        case 3://Intensity = 2
        //            robot.Persuasion.Prosody.Intensity = 2;
        //            break;
        //        case 4://Intensity = 3
        //            robot.Persuasion.Prosody.Intensity = 3;
        //            break;
        //        case 5://Intensity = 4
        //            robot.Persuasion.Prosody.Intensity = 4;
        //            break;
        //        default://Intensity = 4
        //            robot.Persuasion.Prosody.Intensity = 4;
        //            break;
        //    }
        //}

        private void LoadAnimation(Robot robot)
        {

            if (robot.PersuasionCondition.Equals(RobotsPersuasion.againts)) // robot persuasion condition is against the user personality
            {
                switch (robot.ConsecutivePlays)
                {
                    case 1:
                        robot.Persuasion.Animation = "anger1";
                        break;
                    case 2:
                        robot.Persuasion.Animation = "anger3";
                        break;
                    case 3:
                        robot.Persuasion.Animation = "anger5";
                        break;
                    default:
                        if (robot.OponentPlays > 3)
                        {
                            robot.Persuasion.Animation = "anger5";
                        }
                        break;
                }
            }
            else if (robot.PersuasionCondition.Equals(RobotsPersuasion.favour))// robot persuasion condition is in favour of the user personality
            {
                switch (robot.ConsecutivePlays)
                {
                    case 1:
                        robot.Persuasion.Animation = "joy1";
                        break;
                    case 2:
                        robot.Persuasion.Animation = "joy3";
                        break;
                    case 3:
                        robot.Persuasion.Animation = "joy5";
                        break;
                    default:
                        if (robot.ConsecutivePlays > 3)
                        {
                            robot.Persuasion.Animation = "joy5";
                        }
                        break;
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
                    //SearchProsodyLvl(robot);
                }
                else
                {
                    robot.Persuasion.Prosody.Language = Prosody.RobotsLanguage.PT;
                    //SearchProsodyLvl(robot);
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

            robotPlays = robot.TotalMeek + robot.TotalDominant;

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

            if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
            {
                _persuasion.Times = GetTimesRobotFeature();







                //ver melhor como é que vou fazer para ter os dois rob^^os a fazerem as percentagens correctas para cada um






                //if (rightRobot.Personality.Equals(Robot.RobotsPersonality.meek))
                //{
                //    rightRobot.Persuasion.Times = (1 - leftRobot.Persuasion.Times);
                //}
                //else
                //{
                //    leftRobot.Persuasion.Times = (1 - rightRobot.Persuasion.Times);
                //}
            }
        }

        //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, Meek will look less to person 
        //if return 0 robot will perform gaze such as: domintant will look less to person,  Meek will look more to person
        private int GetTimesRobotFeature()
        {
            Random random = new Random();

            if (random.NextDouble() <= 0.8)
                return 1;

            return 0;
        }

        //internal void EnableBTS(string button)
        //{
        //    /* ONLY TO SEE IF IS OK, AFTER ALL REMOTE IT
        //    if (SidePerform(StoryHandler.GetLeftPref().ToUpper()).Equals("L"))
        //    {
        //        Console.WriteLine("===================== PREFS (L-R): " + StoryHandler.GetLeftPref() + " -- " + StoryHandler.GetRightPref());
        //    } else
        //    {
        //        Console.WriteLine("===================== PREFS (L-R): " + StoryHandler.GetRightPref() + " -- " + StoryHandler.GetLeftPref());
        //    }*/

        //    switch (button)
        //    {
        //        case "R":
        //            this.playRight.Enabled = true;
        //            this.playRight.Style = MetroFramework.MetroColorStyle.Green;
        //            break;
        //        case "L":
        //            this.playLeft.Enabled = true;
        //            this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
        //            break;
        //        case "LR":
        //            this.rightButton.Enabled = leftButton.Enabled = true;
        //            this.rightButton.Theme = MetroFramework.MetroThemeStyle.Light;
        //            this.leftButton.Theme = MetroFramework.MetroThemeStyle.Light;
        //            this.playLeft.Enabled = playRight.Enabled = true;
        //            this.playRight.Style = MetroFramework.MetroColorStyle.Green;
        //            this.playLeft.Style = MetroFramework.MetroColorStyle.Green;
        //            break;
        //    }
        //}


        //internal void PlayLeft_Robot()
        //{
        //    this.PlayedLeftButton = true;
        //    this.DisableButtons();

        //    //   CallUtterancesLeft(0);
        //}

        private void CallUtterances(Robot robotSide, int buttonoption)
        {
            string fullUtterance = "";
            // string[] tags = StoryHandler.GetLeftTag().Split(',');

            if (robotSide.Side.Equals(RobotSide.left))
            {
                //fullUtterance = GEtUtteranceAnimationsProsodies(leftRobot, buttonoption);
                //if (!fullUtterance.Equals(""))
                //{
                //ThalamusClientLeft.StartUtteranceFromLibrary("Id", "PT:Favor", a);
                ThalamusClientLeft.PerformUtteranceFromLibrary("Against_" + Guid.NewGuid().ToString(), "PT", "Against", new string[] { }, new string[] { });
                //ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                //ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                //}
                Console.WriteLine("StoryForm LEFT UtterancePhrase ID: " + ThalamusClientLeft.idUtterancePhrase + " - " + ThalamusClientLeft.utterancePhrase);
                //Console.WriteLine("LeftRobot UtterancePhrase ID: " + LeftRobot.IdPhrasesUsed[0] + " - " + LeftRobot.PhraseUsed[0] + " - " + LeftRobot.TimesPhrases);
            }
            else
            {
                //fullUtterance = GEtUtteranceAnimationsProsodies(rightRobot, buttonoption);
                //if (!fullUtterance.Equals(""))
                //{
                //ThalamusClientRight.StartUtteranceFromLibrary("Id", "PT:Against", a);
                //ThalamusClientRight.PerformUtteranceFromLibrary("Id", "PT", "Against", a, a);

                ThalamusClientRight.PerformUtteranceFromLibrary("Against_" + Guid.NewGuid().ToString(), "PT", "Against", new string[] { }, new string[] { });
                //ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
                //ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), tags[0] + ";" + fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "leftRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                //}
                Console.WriteLine("StoryForm RIGHT UtterancePhrase ID: " + ThalamusClientRight.idUtterancePhrase + " - " + ThalamusClientRight.utterancePhrase);
                //Console.WriteLine("RightRobot UtterancePhrase ID: " + RightRobot.IdPhrasesUsed[0] + " - " + RightRobot.PhraseUsed[0] + " - " + RightRobot.TimesPhrases);
            }
        }

        //internal void PlayRight_Robot()
        //{
        //    this.PlayedRightButton = true;
        //    this.DisableButtons();

        //    //CallUtterancesRight(0);
        //}

        //private void CallUtterancesRight(int rightButton)
        //{
        //    string fullUtterance = "";
        //    //string[] tags = StoryHandler.GetRightTag().Split(',');

        //    if (rightButton == 0)
        //    {
        //        if (SidePerform(StoryHandler.GetRightPref().ToUpper()).Equals("R"))
        //        {
        //            string[] utterance = StoryHandler.GetRightUtterance(this.Language).Split(',');
        //            fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], rightRobot, OptionSide.right, 0);
        //        }
        //        else
        //        {
        //            string[] utterance = StoryHandler.GetLeftUtterance(this.Language).Split(',');
        //            fullUtterance = GEtUtteranceAnimationsProsodies(utterance[0], rightRobot, OptionSide.right, 0);
        //        }
        //    }
        //    else
        //    {
        //        if (rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant))
        //        {
        //            fullUtterance = GEtUtteranceAnimationsProsodies("", rightRobot, OptionSide.right, 1);
        //        }
        //        else
        //        {
        //            fullUtterance = GEtUtteranceAnimationsProsodies("", leftRobot, OptionSide.left, 1);
        //            if (!fullUtterance.Equals(""))
        //            {
        //                ThalamusClientLeft.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
        //                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
        //                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
        //            }
        //        }
        //    }

        //    if (rightRobot.Personality.Equals(Robot.RobotsPersonality.dominant) || rightButton == 0)
        //    {
        //        ThalamusClientRight.StartUtterance(StoryHandler.GetDecisionUtteranceId(), fullUtterance);
        //        ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
        //        ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), fullUtterance + ";" + rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
        //    }
        //}

        private string SidePerform(string prefUtterance)
        {
            string side = "";
            UserPersonality = UserPersonality.ToUpper();
            /*Console.WriteLine("===== USER PERSONALITY: " + UserPersonality);
            Console.WriteLine("===== Robot Left personality: " + leftRobot.Personality);
            Console.WriteLine("===== Pref utterance side? : " + prefUtterance); */

            if (StoryHandler.GetInitialDP().Contains("DP"))
            {
                if (UserPersonality.Contains(prefUtterance))
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
                //Console.WriteLine("===== Condition persuasion : " + conditionPersuasion + " Side: " + side);
                //if (conditionPersuasion.Equals(1))
                //{
                //    if (side.Equals("L")) side = "R"; else side = "L";
                //}
            }
            else
            {
                side = prefUtterance.ToUpper();
            }
            Console.WriteLine("===== Side : " + side);
            return side;
        }

        private string GEtUtteranceAnimationsProsodies(Robot robotSide, int buttonOption)
        {
            string aux_prosody = "";
            string animation_prosody = "";

            if (buttonOption == 1 && !robotSide.Persuasion.Animation.Equals(""))//robot makes joy or anger animation
            {
                animation_prosody = "<Gaze(person3)><break time=\"1s\"/><ANIMATE(" + robotSide.Persuasion.Animation + ")>";
            }
            else if (buttonOption == 2 && !robotSide.Persuasion.Prosody.Utterance.Equals(""))//robot says utterance in favour or againts the person choice
            {

                animation_prosody = "<prosody rate='" + robotSide.Persuasion.Prosody.Rate + "'><prosody volume='" + robotSide.Persuasion.Prosody.Volume + "'>" + robotSide.Persuasion.Prosody.Utterance + "</prosody></prosody>";
            }
            else if (buttonOption == 3 && !GetGaze(robotSide, robotSide.Side).Equals(""))//robot makes gaze at the button when user choice is the same as what he would choose
            {
                //No prosody just Gaze
                animation_prosody = GetGaze(robotSide, robotSide.Side); ;
            }

            aux_prosody = animation_prosody;

            if (!aux_prosody.Equals(""))
            {
                animation_prosody = "<prosody pitch='" + robotSide.Pitch + "'>" + aux_prosody + "</prosody>";
            }
            return animation_prosody;
        }

        //private void LoadPersuasionLvl2(Robot robot)
        //{
        //    robot.Persuasion.Prosody.Lvl = 2;

        //    if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
        //    {
        //        if (robot.OponentPlays > 1)
        //        {
        //            if (robotPlays == 4)
        //            {
        //                robotPlays = 1;
        //            }
        //            else
        //            {
        //                robotPlays++;
        //            }
        //            robot.Persuasion.Prosody.Intensity = robotPlays;
        //        }
        //    }
        //}

        private string GetGaze(Robot robotSide, RobotSide side)
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
            //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, Meek will look less to person 
            //if return 0 robot will perform gaze such as: domintant will look less to person,  Meek will look more to person
            string gazeTo = "";

            if (robotSide.Persuasion.Times.Equals(1))//robot will perform gaze 80% of the time
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
            Console.WriteLine("============= CLOSE GAME ===============");
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= CLOSE GAME ===============", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= CLOSE GAME ===============", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= CLOSE GAME ===============", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= CLOSE GAME ===============", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= CLOSE GAME ===============", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

            ThalamusClientLeft.Shutdown();
            ThalamusClientRight.Shutdown();
            Application.Exit();
        }

        //internal void SearchProsodyLvl(Robot robot)
        //{
        //    try
        //    {
        //        foreach (Prosody p in ProsodyLvls)
        //        {
        //            if (p.Lvl == robot.Persuasion.Prosody.Lvl && p.Intensity == robot.Persuasion.Prosody.Intensity && p.Language.Equals(robot.Persuasion.Prosody.Language))
        //            {
        //                robot.Persuasion.Prosody.Utterance = p.Utterance;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        internal void playStoryScene(int idScene, Thalamus.BML.SpeechLanguages language)
        {
            DisableButtons();

            string folder = "EN";
            if (language == Thalamus.BML.SpeechLanguages.Portuguese) folder = "PT";

            //Console.WriteLine("=========== NEXT AUDIO ============== " + (idScene + 1));

            axWindowsMediaPlayer1.URL = ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + ".wav";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            //Console.WriteLine("URL: " + axWindowsMediaPlayer1.URL);

            ActivateRobot();
        }

        private void ActivateRobot()
        {
            if (rightRobot.Enable)
            {
                leftRobot.Enable = true;
                rightRobot.Enable = false;
            }
            else
            {
                rightRobot.Enable = true;
                leftRobot.Enable = false;
            }
        }

        //private void playLeft_Click(object sender, EventArgs e)
        //{
        //    listenRobotAgain[0]++;
        //    PlayLeft_Robot();
        //}

        //private void playRight_Click(object sender, EventArgs e)
        //{
        //    listenRobotAgain[1]++;
        //    PlayRight_Robot();
        //}

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
                        //PlayLeft_Robot();
                    }
                    else
                    {
                        Console.WriteLine("=========== ROBOT RIGHT DOMINANT ==============");
                        //PlayRight_Robot();
                    }

                    this.leftButton.Enabled = this.rightButton.Enabled = true;

                    this.labelLeftButton.Text = StoryHandler.GetLeftUtterance(Language);
                    this.labelRightButton.Text = StoryHandler.GetRightUtterance(Language);
                    this.labelLeftButton.Visible = this.labelRightButton.Visible = true;
                    this.rightButton.Visible = this.leftButton.Visible = true;                   
                }
                else
                {
                    Console.WriteLine("============= END GAME ===============");
                    ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= END GAME ===============", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= END GAME ===============", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= END GAME ===============", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= END GAME ===============", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "============= END GAME ===============", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                    sceneBox.Visible = false;
                    this.rightButton.Visible = this.leftButton.Visible = false;
                    this.labelLeftButton.Visible = this.labelRightButton.Visible = false;
                    lblResearcher.Visible = true;
                    //  lblResearcher.Enabled = true;
                }
            }
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            btConfirmEnable = true;
            this.btConfirm.Visible = false;
            this.btConfirm.Enabled = false;
            this.labelLeftButton.Visible = this.labelRightButton.Visible = false;
            this.rightButton.Visible = this.leftButton.Visible = false;

            Console.WriteLine("Confirmar se preferência final foi selecionada senão então é igual há intenção");

            // Console.WriteLine("Activar animação favor ou contra");

            if (!rightRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (rightRobot.PrefSelectedFinal.Equals(""))
                {
                    SaveFinalPreference(rightRobot, OptionSide.none, 1);
                }
                LoadAnimation(rightRobot);
                CallUtterances(rightRobot, 1);
            }
            else if (!leftRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (leftRobot.PrefSelectedFinal.Equals(""))
                {
                    SaveFinalPreference(leftRobot, OptionSide.none, 1);
                }
                LoadAnimation(leftRobot);
                CallUtterances(leftRobot, 1);
            }

            DeletePreferenceUsed();
            CallNextScene();
        }

        private void DeletePreferenceUsed()
        {
            string preferenceSelected = StoryHandler.GetPrefDP();

            switch (preferenceSelected)
            {
                case "ei":
                case "ie":
                    PreferenceEI.RemoveAt(0);
                    break;
                case "si":
                case "is":
                    PreferenceSN.RemoveAt(0);
                    break;
                case "tf":
                case "ft":
                    PreferenceTF.RemoveAt(0);
                    break;
                case "jp":
                case "pj":
                    PreferenceJP.RemoveAt(0);
                    break;
                case "-":
                    preferenceDG.RemoveAt(0);
                    break;

            }
        }

        private void recordFinalLog()
        {
            string txt = "\r\n ============================= \r\n" +
                         "Left Robot Again: " + listenRobotAgain[0] + "\r\n" +
                         "Right Robot Again: " + listenRobotAgain[1] + "\r\n" +
                         "============================= \r\n" +
                         "Total Dominant: " + selectedDP.TotalDominant + "\r\n" +
                         "Total Meek: " + selectedDP.TotalMeek + "\r\n" +
                         "============================= \r\n";

            txt += personality.DefineMBTIPersonality();

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }
    }
}
