using AxWMPLib;
using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

            selectedDP = new SelectionDP();
            instance = this;
        }

        private void LoadLogFiles()
        {
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;TimesPhrases;IdPhrase;PhraseUtterance;PersuasionCondition", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;TimesPhrases;IdPhrase;PhraseUtterance;PersuasionCondition", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";RobotPersonality;ConsecutivePlays;OpopnentPlays;TotalDominant;TotalMeek;RobotPitch;Gaze;TimeRobotFeatures;AnimationDominant;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;TimesPhrases;IdPhrase;PhraseUtterance;PersuasionCondition", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";CurrentStoryNodeId;OptionSelected;SideSelected;RobotPersonality;TotalDominant;TotalMeek;DPPreferencePair;DPPreferenceIntention;DpPreferenceFinal;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";DecisionPoint;PreferencePair;PreferenceSelected;RobotPersonality;ConditionPersuasion", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
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
                labelLeftButton.Visible = labelRightButton.Visible = true;
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
                if (!leftRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
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
                if (!rightRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
                {
                    ChoiceIntention(rightRobot, OptionSide.right);
                }
                else
                {
                    ChoiceIntention(leftRobot, OptionSide.right);
                }
            }
        }

        private void ChoiceIntention(Robot robotSide, OptionSide optionSide)
        {
            //Console.WriteLine("ChoiceIntention I rightRobot " + rightRobot.ToString());
            //Console.WriteLine("ChoiceIntention I leftRobot " + leftRobot.ToString());

            //Console.WriteLine("robotSide " + robotSide.ToString());

            if (btConfirmEnable)
            {
                btConfirmEnable = false;
                btConfirm.Enabled = false;
                btConfirm.Visible = true;

                string auxPreferenceSide = "";

                if (optionSide.Equals(OptionSide.left))
                {
                    auxPreferenceSide = StoryHandler.GetLeftPref();
                }
                else
                {
                    auxPreferenceSide = StoryHandler.GetRightPref();
                }

                selectedDP.DpPrefPair = StoryHandler.GetPrefDP();
                selectedDP.DPPrefIntention = auxPreferenceSide;
                robotSide.PrefSelectedIntention = selectedDP.DPPrefIntention;
                //selectedDP.DPPrefSelected = auxPreferenceSide;
                //robotSide.PrefSelectedIntention = selectedDP.DPPrefSelected;

                //Console.WriteLine("Gerar propriedades do gaze");
                LoadScenePersuasion(robotSide);

                //Console.WriteLine("Activar utterance a favor ou contra");

                CallUtterances(robotSide, 2);

                //Console.WriteLine("Activar gaze a favor");

                CallUtterances(robotSide, 3);

                btConfirm.Enabled = true;
            }
            else
            {
                SaveFinalPreference(robotSide, optionSide, 0);
            }

            //Console.WriteLine("ChoiceIntention F rightRobot " + rightRobot.ToString());
            //Console.WriteLine("ChoiceIntention F leftRobot " + leftRobot.ToString());
        }

        private void SaveFinalPreference(Robot robotSide, OptionSide optionSide, int intention)
        {
            //Console.WriteLine("SaveFinalPreference I rightRobot " + rightRobot.ToString());
            //Console.WriteLine("SaveFinalPreference I leftRobot " + leftRobot.ToString());

            string auxPreferenceSide = "";

            stopwatch.Stop();
            DisableButtons();

            if (intention == 1)
            {
                if (robotSide.PrefSelectedIntention.Equals(StoryHandler.GetLeftPref()))
                {
                    auxPreferenceSide = StoryHandler.GetLeftPref();
                    optionSide = OptionSide.left;
                }
                else
                {
                    auxPreferenceSide = StoryHandler.GetRightPref();
                    optionSide = OptionSide.right;
                }

                selectedDP.DPPrefSelectedFinal = auxPreferenceSide;
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

                selectedDP.DPPrefSelectedFinal = auxPreferenceSide;

                this.btConfirm.Visible = true;
                this.btConfirm.Enabled = true;
                this.leftButton.Enabled = this.rightButton.Enabled = false;
            }

            selectedDP.DpPrefPair = StoryHandler.GetPrefDP();
            selectedDP.OptionSelected = Convert.ToInt32(optionSide);
            selectedDP.SideSelected = optionSide;
            selectedDP.ElapsedMs = stopwatch.ElapsedMilliseconds;

            selectedDP.RobotPersonality = robotSide.Personality;

            robotSide.DecisionPoint = StoryHandler.GetInitialDP();
            robotSide.PreferencePair = StoryHandler.GetPrefDP();
            robotSide.PrefSelectedFinal = selectedDP.DPPrefSelectedFinal;

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

                // ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientLeft", "Robots-" + this.UserId.ToString() + ".txt");
                // ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }
            else
            {
                leftRobot.ConsecutivePlays = 0;
                leftRobot.OponentPlays = robotSide.ConsecutivePlays;

                //ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientRight", "Robots-" + this.UserId.ToString() + ".txt");
                // ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }

            leftRobot.TotalDominant = selectedDP.TotalDominant;
            leftRobot.TotalMeek = selectedDP.TotalMeek;

            rightRobot.TotalDominant = selectedDP.TotalDominant;
            rightRobot.TotalMeek = selectedDP.TotalMeek;

            btConfirmEnable = true;

            //Console.WriteLine("SaveFinalPreference F rightRobot " + rightRobot.ToString());
            //Console.WriteLine("SaveFinalPreference F leftRobot " + leftRobot.ToString());
        }

        private Robot AwakeRobot()
        {
            Robot per_per = new Robot();
            string preferenceSelected = StoryHandler.GetPrefDP();
            //Console.WriteLine("AwakeRobot preferenceSelected " + preferenceSelected);

            switch (preferenceSelected)
            {
                case "ei":
                case "ie":
                    per_per = PreferenceEI.First();
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
            rightRobot.Persuasion.Animation = "-";
        }

        public void LoadScenePersuasion(Robot robot)
        {
            //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
            //when intensity achieves value 4 the next keeps the 4
            LoadGazeTime(robot, robot.Persuasion);
            GetUtteranceLanguage(robot);
        }

        private void LoadAnimation(Robot robot)
        {
            if (robot.PersuasionCondition.Equals(RobotsPersuasion.Against)) // robot persuasion condition is against the user personality
            {
                if (robot.PrefSelectedIntention.Equals(robot.PrefSelectedFinal))
                {
                    robot.Persuasion.Animation = "anger5";
                }
                else
                {
                    robot.Persuasion.Animation = "joy1";
                }
            }
            else if (robot.PersuasionCondition.Equals(RobotsPersuasion.Favour))// robot persuasion condition is in favour of the user personality
            {

                if (robot.PrefSelectedIntention.Equals(robot.PrefSelectedFinal))
                {
                    robot.Persuasion.Animation = "joy1";
                }
                else
                {
                    robot.Persuasion.Animation = "anger5";
                }
            }
        }

        private void GetUtteranceLanguage(Robot robot)
        {
            if (Language.Equals(Thalamus.BML.SpeechLanguages.English))
            {
                robot.Persuasion.Prosody.Language = Prosody.RobotsLanguage.EN;
            }
            else
            {
                robot.Persuasion.Prosody.Language = Prosody.RobotsLanguage.PT;
            }
        }

        private void LoadGazeTime(Robot robot, Persuasion _persuasion)
        {
            //gaze intensity = 1 gaze to Person and then Button 
            //gaze intensity = 2 gaze to Person then Button and again to Person
            //gaze intensity = 2 gaze to Person then Button again to Person and again to Button

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

                if (rightRobot.Personality.Equals(Robot.RobotsPersonality.meek))
                {
                    rightRobot.Persuasion.Times = (1 - leftRobot.Persuasion.Times);
                }
                else
                {
                    leftRobot.Persuasion.Times = (1 - rightRobot.Persuasion.Times);
                }
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

        private void CallUtterances(Robot robotSide, int buttonoption)
        {
            string language = "";
            if (robotSide.Persuasion.Prosody.Language.Equals(Prosody.RobotsLanguage.EN))
            {
                language = "EN";
            }
            else
            {
                language = "PT";
            }

            if (buttonoption == 1)//make animation anger or joys
            {
                if (robotSide.Side.Equals(RobotSide.left))
                {
                    ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "ANIMATION", new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                }
                else
                {
                    ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "ANIMATION", new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                }
                //SAVE LOGS
            }
            else if (buttonoption == 2)// make utterance in favour or against
            {
                string persuasion_condition = "";
                if (robotSide.PersuasionCondition.Equals(RobotsPersuasion.Favour))
                {
                    persuasion_condition = "FAVOUR";
                }
                else
                {
                    persuasion_condition = "AGAINST";
                }

                if (robotSide.Side.Equals(RobotSide.left))
                {
                    ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                }
                else
                {
                    ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                }
                //SAVE LOGS
            }
            else if (buttonoption == 3)//gaze at button confirms
            {
                string animation_person = GetGaze(robotSide);
                string[] aux = animation_person.Split('-');

                if (robotSide.Side.Equals(RobotSide.left))
                {
                    switch (aux[0])
                    {
                        case "Gaze_PB":
                            ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "Gaze_PB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                        case "Gaze_PBP":
                            ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "Gaze_PBP", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                        case "Gaze_PBPB":
                            ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "Gaze_PBPB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                    }
                    //SAVE LOGS
                }
                else
                {
                    switch (aux[0])
                    {
                        case "Gaze_PB":
                            ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "Gaze_PB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                        case "Gaze_PBP":
                            ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "Gaze_PBP", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                        case "Gaze_PBPB":
                            ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), "PT", "Gaze_PBPB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                            break;
                    }
                    //SAVE LOGS
                }
            }
        }

        private string GetGaze(Robot robotSide)
        {
            string animation_prosody = "";
            string animation_person = GetPersonOrRandomGaze(robotSide);
            switch (robotSide.Persuasion.Gaze.Count(x => x == '-'))
            {
                case 1://gaze = "Person-Button"
                    animation_prosody = "Gaze_PB-" + animation_person;
                    break;
                case 2://gaze = "Person-Button-Person"
                    animation_prosody = "Gaze_PBP-" + animation_person;
                    break;
                case 3://gaze = "Person-Button-Person-Button"
                    animation_prosody = "Gaze_PBPB-" + animation_person;
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
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= CLOSE GAME ===============", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= CLOSE GAME ===============", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= CLOSE GAME ===============", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= CLOSE GAME ===============", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= CLOSE GAME ===============", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

            ThalamusClientLeft.Shutdown();
            ThalamusClientRight.Shutdown();
            Application.Exit();
        }

        internal void playStoryScene(int idScene, Thalamus.BML.SpeechLanguages language)
        {
            DisableButtons();

            string folder = "EN";
            string audioExt = ".mp3";
            if (language == Thalamus.BML.SpeechLanguages.Portuguese) { folder = "PT"; audioExt = ".wav"; }

            //Console.WriteLine("=========== NEXT AUDIO ============== " + (idScene + 1));

            if (System.IO.File.Exists(ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + audioExt))
            {
                axWindowsMediaPlayer1.URL = ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + audioExt;
                axWindowsMediaPlayer1.settings.volume = 50;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                Console.WriteLine(" ========== AUDIO " + (idScene + 1) + " NOT FOUND! ========== ");
            }
            //Console.WriteLine("URL: " + axWindowsMediaPlayer1.URL);
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
                    }
                    else
                    {
                        Console.WriteLine("=========== ROBOT RIGHT DOMINANT ==============");
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
                    ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= END GAME ===============", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= END GAME ===============", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= END GAME ===============", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= END GAME ===============", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
                    ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";============= END GAME ===============", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                    sceneBox.Visible = false;
                    this.rightButton.Visible = this.leftButton.Visible = false;
                    this.labelLeftButton.Visible = this.labelRightButton.Visible = false;
                    lblResearcher.Visible = true;
                }
            }
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            btConfirmEnable = true;

            //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
            //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());
            string txt = "";

            if (!rightRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (rightRobot.PrefSelectedFinal.Equals("-"))
                {
                    SaveFinalPreference(rightRobot, OptionSide.none, 1);
                }
                // PERSONALITY
                personality.BuildPersonality(rightRobot.PrefSelectedFinal);//selecionou botão esquerda manda a preferência dessa opção
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), rightRobot.PrefSelectedFinal, rightRobot.Personality.ToString(), rightRobot.PersuasionCondition.ToString());
                //selectedDP.DPPrefSelectedFinal = rightRobot.PrefSelectedFinal;
                //rightRobot.PrefSelectedFinal = selectedDP.DPPrefSelectedFinal;

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                LoadAnimation(rightRobot);

                CallUtterances(rightRobot, 1);

                //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
                //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }
            else if (!leftRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (leftRobot.PrefSelectedFinal.Equals("-"))
                {
                    SaveFinalPreference(leftRobot, OptionSide.none, 1);
                }
                // PERSONALITY
                personality.BuildPersonality(leftRobot.PrefSelectedFinal);//selecionou botão esquerda manda a preferência dessa opção
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), leftRobot.PrefSelectedFinal, leftRobot.Personality.ToString(), leftRobot.PersuasionCondition.ToString());
                //selectedDP.DPPrefSelectedFinal = leftRobot.PrefSelectedFinal;
                //leftRobot.PrefSelectedFinal = selectedDP.DPPrefSelectedFinal;

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                LoadAnimation(leftRobot);

                CallUtterances(leftRobot, 1);

                //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
                //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());

                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            }

            DeletePreferenceUsed();
            rightRobot.PersuasionCondition = RobotsPersuasion.none;
            leftRobot.PersuasionCondition = RobotsPersuasion.none;

            this.btConfirm.Visible = false;
            this.btConfirm.Enabled = false;
            this.labelLeftButton.Visible = this.labelRightButton.Visible = false;
            this.rightButton.Visible = this.leftButton.Visible = false;

            StoryHandler.NextScene(selectedDP);

            selectedDP.DPPrefIntention = "-";
            selectedDP.DpPrefPair = "-";
            selectedDP.DPPrefSelectedFinal = "-";
            selectedDP.OptionSelected = -1;
            selectedDP.SideSelected = OptionSide.none;

            rightRobot.PreferencePair = "-";
            rightRobot.PrefSelectedIntention = "-";
            rightRobot.PrefSelectedFinal = "-";
            leftRobot.PreferencePair = "-";
            leftRobot.PrefSelectedIntention = "-";
            leftRobot.PrefSelectedFinal = "-";

            CallNextScene();
        }

        private void DeletePreferenceUsed()
        {
            string preferenceSelected = StoryHandler.GetPrefDP();
            Console.WriteLine("Deleted preference " + preferenceSelected);

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
                         "Total Dominant: " + selectedDP.TotalDominant + "\r\n" +
                         "Total Meek: " + selectedDP.TotalMeek + "\r\n" +
                         "============================= \r\n";

            txt += personality.DefineMBTIPersonality();

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }
    }
}
