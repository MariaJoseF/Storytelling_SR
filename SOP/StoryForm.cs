using AxWMPLib;
using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Timers;
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
        public bool WaitAnimation = false;
        int[,] PreferencesCongruency = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };//first element is number of times dominant robot was against and the second is for the meek robot
        int[,] PreviousPrefSelected = new int[,] { { -1, -1 } };// first element is robot Personality, second element is the id on the preference list


        public bool WaitIntro1 = false;
        public bool WaitIntro2 = false;
        public bool WaitIntro3 = false;
        public bool WaitIntro4 = false;

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

        public StoryForm(string UserId, Client ThalamusClientRight, Client ThalamusClientLeft, Robot rightRobot, Robot leftRobot, Thalamus.BML.SpeechLanguages language)
        {
            this.Language = language;
            InitializeComponent();
            //Console.WriteLine("RESOLUTION: " + this.ClientSize.Width + " ::: " + this.ClientSize.Height);
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
            this.ThalamusClientRight.StoryWindow(this);
            this.ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

            this.ThalamusClientLeft = ThalamusClientLeft;
            this.ThalamusClientLeft.StoryWindow(this);
            this.ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");

            this.RightRobot = rightRobot;
            this.LeftRobot = leftRobot;

            string[] aux = UserId.Split('-');
            this.UserId = Convert.ToInt32(aux[0]);

            StoryHandler = new StoryHandler(ThalamusClientLeft, this.UserId);

            this.UserPersonality = aux[1].ToLower();

            this.ReenableButtonsEvent += new System.EventHandler(this.EnableButtons);

            personality = new Personality(this);

            LoadLogFiles();

            StoryHandler.LoadPreferences(PreferenceEI, PreferenceJP, PreferenceSN, PreferenceTF, preferenceDG);


            StoryHandler.storyWindow = this;

            StoryHandler.TestIntroduction();

            selectedDP = new SelectionDP();
            instance = this;

        }

        private void LoadLogFiles()
        {
            ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";UserPersonality;RobotPersonality;RobotSide;Posture;RobotPitch;Animation;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;IntentionCongruent;IdPhrase;PhraseUtterance;AnimationUtterance;PersuasionCondition", "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";UserPersonality;RobotPersonality;RobotSide;Posture;RobotPitch;Animation;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;IntentionCongruent;IdPhrase;PhraseUtterance;AnimationUtterance;PersuasionCondition", "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";UserPersonality;RobotPersonality;RobotSide;Posture;RobotPitch;Animation;ProsodyRate;ProsodyVolume;ProsodyLanguage;DecisionPoint;PreferencePair;PreferenceSelectedIntention;PreferenceSelectedFinal;IntentionCongruent;IdPhrase;PhraseUtterance;AnimationUtterance;PersuasionCondition", "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";CurrentStoryNodeId;OptionSelected;SideSelected;RobotPersonality;DPPreferencePair;DPPreferenceIntention;DpPreferenceFinal;ElapsedMS", "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");
            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";DecisionPoint;PreferencePair;PreferenceSelected;UserPersonality;RobotPersonality;ConditionPersuasion", "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }

        private void CropAndStrechBackImage()
        {
            Size s = new Size(Screen.PrimaryScreen.Bounds.Width, 640);
            //this.backImage.Size = this.Size;
            this.backImage.Size = s;
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

            //playStoryScene(this.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
            this.CropAndStrechBackImage();

        }

        private void StoryForm_Shown(object sender, EventArgs e)
        {
           // this.button1.Visible = true;
           // this.button1.Text = "Olá ...";
           // this.sceneBox.Text = "teste";


           // RobotsIntroduction();
           // this.sceneBox.Text = "Teste 123";
           //// waiting 2 seconds to start the next method
           //var waitTime = new TimeSpan(0, 0, 2);
           // var waitUntil = DateTime.Now + waitTime;
           // int secs = 0;
           // while (DateTime.Now <= waitUntil)
           // {
           //     secs++;
           //     Console.WriteLine("Waiting: " + secs);
           //     System.Threading.Thread.Sleep(1000);
           // }

        //    this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);



            playStoryScene(StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
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
                leftButton.Theme = MetroFramework.MetroThemeStyle.Light;
            }
            leftButton.Highlight = true;
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
                rightButton.Theme = MetroFramework.MetroThemeStyle.Light;
            }
            rightButton.Highlight = true;
        }

        private void ChoiceIntention(Robot robotSide, OptionSide optionSide)
        {
            if (btConfirmEnable)
            {
                btConfirmEnable = false;
                //btConfirm.Enabled = false;
                //btConfirm.Visible = true;

                string auxPreferenceSide = "";

                if (optionSide.Equals(OptionSide.left))
                {
                    auxPreferenceSide = StoryHandler.GetLeftPref();
                }
                else
                {
                    auxPreferenceSide = StoryHandler.GetRightPref();
                }

                if (StoryHandler.GetPrefDP().Equals("-"))
                {
                    robotSide.CongruentIntention = RobotCongruent.none;
                }
                else if (UserPersonality.Contains(auxPreferenceSide))
                {
                    robotSide.CongruentIntention = RobotCongruent.congruent;
                }
                else
                {
                    robotSide.CongruentIntention = RobotCongruent.non_congruent;
                }

                selectedDP.DpPrefPair = StoryHandler.GetPrefDP();
                selectedDP.DPPrefIntention = auxPreferenceSide;
                robotSide.PrefSelectedIntention = selectedDP.DPPrefIntention;

                if (!robotSide.CongruentIntention.Equals(RobotCongruent.non_congruent))
                {
                    switch (selectedDP.DpPrefPair)
                    {
                        case "ei":
                        case "ie":
                            LoadCongruency(robotSide, 0);
                            break;
                        case "tf":
                        case "ft":
                            LoadCongruency(robotSide, 1);
                            break;
                        case "jp":
                        case "pj":
                            LoadCongruency(robotSide, 2);
                            break;
                        case "sn":
                        case "ns":
                            LoadCongruency(robotSide, 3);
                            break;
                        case "-":
                            LoadCongruency(robotSide, 4);
                            break;
                    }
                }
                //selectedDP.DPPrefSelected = auxPreferenceSide;
                //robotSide.PrefSelectedIntention = selectedDP.DPPrefSelected;

                //Console.WriteLine("Gerar propriedades do gaze");
                // LoadScenePersuasion(robotSide);
                GetUtteranceLanguage(robotSide);

                //Console.WriteLine("Activar utterance a favor ou contra");

                CallUtterances(robotSide, 2, optionSide);

                //Console.WriteLine("Activar gaze a favor");

                CallUtterances(robotSide, 3, OptionSide.none);

                //btConfirm.Enabled = true;
            }
            else
            {
                SaveFinalPreference(robotSide, optionSide, 0);
            }

            //Console.WriteLine("ChoiceIntention F rightRobot " + rightRobot.ToString());
            //Console.WriteLine("ChoiceIntention F leftRobot " + leftRobot.ToString());
        }

        private void LoadCongruency(Robot robotSide, int index)
        {
            if (robotSide.Personality.Equals(RobotsPersonality.dominantNeutral) && PreferencesCongruency[index, 0] < 1)
            {
                PreferencesCongruency[index, 0]++;
                robotSide.PersuasionCondition = Robot.RobotsPersuasion.Against;
            }
            else if (robotSide.Personality.Equals(RobotsPersonality.meekNeutral) && PreferencesCongruency[index, 1] < 1)
            {
                PreferencesCongruency[index, 1]++;
                robotSide.PersuasionCondition = Robot.RobotsPersuasion.Against;
            }
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

            //robotSide.ConsecutivePlays++;
            //robotSide.OponentPlays = 0;

            //if (robotSide.Side.Equals(Robot.RobotSide.left))
            //{
            //    rightRobot.ConsecutivePlays = 0;
            //    rightRobot.OponentPlays = robotSide.ConsecutivePlays;

            //    // ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientLeft", "Robots-" + this.UserId.ToString() + ".txt");
            //    // ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            //}
            //else
            //{
            //    leftRobot.ConsecutivePlays = 0;
            //    leftRobot.OponentPlays = robotSide.ConsecutivePlays;

            //    //ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientRight", "Robots-" + this.UserId.ToString() + ".txt");
            //    // ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), "Button Pressed;" + robotSide.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");
            //}
            /*
            leftRobot.TotalPositive = selectedDP.TotalPositive;
            leftRobot.TotalNegative = selectedDP.TotalNegative;

            rightRobot.TotalPositive = selectedDP.TotalPositive;
            rightRobot.TotalNegative = selectedDP.TotalNegative;
            */
            btConfirmEnable = true;

            //Console.WriteLine("LPositive: " + leftRobot.TotalPositive + " - LNegative: " + leftRobot.TotalNegative + " - SDP - " + selectedDP.TotalNegative);
            //Console.WriteLine("RPositive: " + rightRobot.TotalPositive + " - RNegative: " + rightRobot.TotalNegative + " - SDP - " + selectedDP.TotalPositive);
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
                    if (PreviousPrefSelected[0, 0] == 0) //meek robot was the last selected
                    {

                        per_per = LoadNextPrefSelected(PreferenceEI, RobotsPersonality.dominantNeutral);
                    }
                    else if (PreviousPrefSelected[0, 0] == 1) //dominant robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceEI, RobotsPersonality.meekNeutral);
                    }
                    else
                    {
                        per_per = LoadFirstPrefSelected(PreferenceEI);
                    }

                    break;
                case "tf":
                case "ft":
                    if (PreviousPrefSelected[0, 0] == 0) //meek robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceTF, RobotsPersonality.dominantNeutral);
                    }
                    else if (PreviousPrefSelected[0, 0] == 1) //dominant robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceTF, RobotsPersonality.meekNeutral);
                    }
                    else
                    {
                        per_per = LoadFirstPrefSelected(PreferenceTF);
                    }
                    break;
                case "jp":
                case "pj":
                    if (PreviousPrefSelected[0, 0] == 0) //meek robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceJP, RobotsPersonality.dominantNeutral);
                    }
                    else if (PreviousPrefSelected[0, 0] == 1) //dominant robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceJP, RobotsPersonality.meekNeutral);
                    }
                    else
                    {
                        per_per = LoadFirstPrefSelected(PreferenceJP);
                    }
                    break;
                case "sn":
                case "ns":
                    if (PreviousPrefSelected[0, 0] == 0) //meek robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceSN, RobotsPersonality.dominantNeutral);
                    }
                    else if (PreviousPrefSelected[0, 0] == 1) //dominant robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(PreferenceSN, RobotsPersonality.meekNeutral);
                    }
                    else
                    {
                        per_per = LoadFirstPrefSelected(PreferenceSN);
                    }
                    break;
                case "-":
                    if (PreviousPrefSelected[0, 0] == 0) //meek robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(preferenceDG, RobotsPersonality.dominantNeutral);
                    }
                    else if (PreviousPrefSelected[0, 0] == 1) //dominant robot was the last selected
                    {
                        per_per = LoadNextPrefSelected(preferenceDG, RobotsPersonality.meekNeutral);
                    }
                    else
                    {
                        per_per = LoadFirstPrefSelected(preferenceDG);
                    }
                    break;
            }





            //////////////  REVER   //////////////


            if (per_per.Personality.Equals(Robot.RobotsPersonality.dominantNeutral) && rightRobot.Personality.Equals(Robot.RobotsPersonality.dominantNeutral))
            {
                rightRobot.Posture = RobotsPosture.neutral;
                rightRobot.PersuasionCondition = per_per.PersuasionCondition;
                leftRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientRight.CPublisher.ResetPose();
                ThalamusClientRight.CPublisher.SetPosture("", rightRobot.Posture.ToString());
                return rightRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.meekNeutral) && rightRobot.Personality.Equals(Robot.RobotsPersonality.meekNeutral))
            {
                rightRobot.Posture = RobotsPosture.neutral;
                rightRobot.PersuasionCondition = per_per.PersuasionCondition;
                leftRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientRight.CPublisher.ResetPose();
                ThalamusClientRight.CPublisher.SetPosture("", rightRobot.Posture.ToString());
                return rightRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.dominantNeutral) && leftRobot.Personality.Equals(Robot.RobotsPersonality.dominantNeutral))
            {
                leftRobot.Posture = RobotsPosture.neutral;
                leftRobot.PersuasionCondition = per_per.PersuasionCondition;
                rightRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientLeft.CPublisher.ResetPose();
                ThalamusClientLeft.CPublisher.SetPosture("", leftRobot.Posture.ToString());
                return leftRobot;
            }
            else if (per_per.Personality.Equals(Robot.RobotsPersonality.meekNeutral) && leftRobot.Personality.Equals(Robot.RobotsPersonality.meekNeutral))
            {
                leftRobot.Posture = RobotsPosture.neutral;
                leftRobot.PersuasionCondition = per_per.PersuasionCondition;
                rightRobot.PersuasionCondition = RobotsPersuasion.none;
                ThalamusClientLeft.CPublisher.ResetPose();
                ThalamusClientLeft.CPublisher.SetPosture("", leftRobot.Posture.ToString());
                return leftRobot;
            }
            else
            {
                return new Robot();
            }
        }

        private Robot LoadFirstPrefSelected(List<Robot> preferenceList)
        {
            Robot per_per = preferenceList.First();
            if (per_per.Personality.Equals(RobotsPersonality.dominantNeutral))
            {
                PreviousPrefSelected[0, 0] = 1;
            }
            else
            {
                PreviousPrefSelected[0, 0] = 0;
            }
            PreviousPrefSelected[0, 1] = 0;
            return per_per;
        }

        private Robot LoadNextPrefSelected(List<Robot> preferencePair, RobotsPersonality personality)
        {
            Robot per_per;
            try
            {
                per_per = preferencePair.First(personalityR => personalityR.Personality == personality);

                if (personality.Equals(RobotsPersonality.dominantNeutral))
                {
                    PreviousPrefSelected[0, 0] = 1;
                }
                else
                {
                    PreviousPrefSelected[0, 0] = 0;
                }

                PreviousPrefSelected[0, 1] = preferencePair.FindIndex(personalityR => personalityR.Personality == personality);
            }
            catch (Exception e)
            {

                Console.WriteLine("************** Excepção " + e.Message + " **************");
                per_per = LoadFirstPrefSelected(preferencePair);
            }

            return per_per;
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

        //public void LoadScenePersuasion(Robot robot)
        //{
        //    //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
        //    //when intensity achieves value 4 the next keeps the 4
        //    //LoadGazeTime(robot, robot.Persuasion);
        //    GetUtteranceLanguage(robot);
        //}

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
            else if (robot.PersuasionCondition.Equals(RobotsPersuasion.Positive))// robot persuasion condition is in favour of the user personality
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

        //private void LoadGazeTime(Robot robot, Persuasion _persuasion)
        //{
        //    //gaze intensity = 1 gaze to Person and then Button 
        //    //gaze intensity = 2 gaze to Person then Button and again to Person
        //    //gaze intensity = 2 gaze to Person then Button again to Person and again to Button

        //    int robotPlays = 0;

        //    robotPlays = robot.TotalNegative + robot.TotalPositive;

        //    switch (robotPlays % 3)
        //    {
        //        case 1://Intensity = 1
        //            _persuasion.Gaze = "Person-Button";
        //            break;
        //        case 2://Intensity = 2
        //            _persuasion.Gaze = "Person-Button-Person";
        //            break;
        //        case 0://Intensity = 3
        //            if (robotPlays > 0)
        //            {
        //                _persuasion.Gaze = "Person-Button-Person-Button";
        //            }
        //            else
        //            {
        //                _persuasion.Gaze = "Person-Button";
        //            }
        //            break;
        //    }

        //    if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
        //    {
        //        _persuasion.Times = GetTimesRobotFeature();

        //        if (rightRobot.Personality.Equals(Robot.RobotsPersonality.meek))
        //        {
        //            rightRobot.Persuasion.Times = (1 - leftRobot.Persuasion.Times);
        //        }
        //        else
        //        {
        //            leftRobot.Persuasion.Times = (1 - rightRobot.Persuasion.Times);
        //        }
        //    }
        //}

        //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, Meek will look less to person 
        //if return 0 robot will perform gaze such as: domintant will look less to person,  Meek will look more to person
        //private int GetTimesRobotFeature()
        //{
        //    Random random = new Random();

        //    if (random.NextDouble() <= 0.8)
        //        return 1;

        //    return 0;
        //}

        private void CallUtterances(Robot robotSide, int buttonoption, OptionSide optionSide)
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
                    //ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "ANIMATION", new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                    ThalamusClientLeft.PerformUtteranceFromLibrary("Animation", "ANIMATION", language, new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                }
                else
                {
                    //ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, "ANIMATION", new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                    ThalamusClientRight.PerformUtteranceFromLibrary("Animation", "ANIMATION", language, new string[] { "|pitch|", "|rate|", "|volume|", "|animation|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, robotSide.Persuasion.Animation });
                }
                //SAVE LOGS
            }
            else if (buttonoption == 2)// make utterance in favour or against
            {
                string persuasion_condition = "";
                string cat = "none";
                if (robotSide.PersuasionCondition.Equals(RobotsPersuasion.Positive))
                {
                    persuasion_condition = "phraseFavourPref_" + language;
                    cat = "POSITIVE";
                }
                else
                {
                    if (optionSide.Equals(OptionSide.left))
                    {
                        persuasion_condition = "phraseAgainstPref1_" + language;
                    }
                    else
                    {
                        persuasion_condition = "phraseAgainstPref2_" + language;
                    }
                    cat = "AGAINST";
                }

                if (robotSide.Side.Equals(RobotSide.left))
                {
                    if (cat.Equals("AGAINST"))
                    {
                        leftRobot.TotalAgainst++;
                    }
                    else
                    {
                        leftRobot.TotalFavour++;
                    }
                    //ThalamusClientLeft.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                    ThalamusClientLeft.PerformUtteranceFromLibrary(cat, StoryHandler.GetInitialDP(), persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                }
                else
                {
                    if (cat.Equals("AGAINST"))
                    {
                        rightRobot.TotalAgainst++;
                    }
                    else
                    {
                        rightRobot.TotalFavour++;
                    }
                    //ThalamusClientRight.PerformUtteranceFromLibrary("Utterance_" + Guid.NewGuid().ToString(), language, persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                    ThalamusClientRight.PerformUtteranceFromLibrary(cat, StoryHandler.GetInitialDP(), persuasion_condition, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume });
                }
                Console.WriteLine("Cat: " + cat + " - Side: " + robotSide.Side.ToString() + " - Left Against: " + leftRobot.TotalAgainst + " - Left Favour: " + leftRobot.TotalFavour);
                Console.WriteLine("Cat: " + cat + " - Side: " + robotSide.Side.ToString() + " - Right Against: " + rightRobot.TotalAgainst + " - Right Favour: " + rightRobot.TotalFavour);
                //SAVE LOGS
            }
            else if (buttonoption == 3)//gaze at button confirms
            {
                //string animation_person = GetGaze(robotSide);
                //string[] aux = animation_person.Split('-');

                if (robotSide.Side.Equals(RobotSide.left))
                {
                    if (robotSide.PersuasionCondition.Equals(RobotsPersuasion.Against))
                    {
                        robotSide.Posture = RobotsPosture.neutral;
                        ThalamusClientLeft.CPublisher.ResetPose();
                        ThalamusClientLeft.CPublisher.SetPosture("", robotSide.Posture.ToString());
                    }
                    //else
                    //{
                    //    robotSide.Posture = RobotsPosture.gratitude;
                    //    ThalamusClientLeft.CPublisher.ResetPose();
                    //    ThalamusClientLeft.CPublisher.SetPosture("", robotSide.Posture.ToString());
                    //}

                    //switch (aux[0])
                    //{
                    //    case "Gaze_PB":
                    //        ThalamusClientLeft.PerformUtteranceFromLibrary("Gaze", language, "Gaze_PB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|", "|animation_side|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1], "btConfirm" });
                    //        break;
                    //    case "Gaze_PBP":
                    //        ThalamusClientLeft.PerformUtteranceFromLibrary("Gaze", language, "Gaze_PBP", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|", "|animation_side|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1], "btConfirm" });
                    //        break;
                    //    case "Gaze_PBPB":
                    //        ThalamusClientLeft.PerformUtteranceFromLibrary("Gaze", language, "Gaze_PBPB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|", "|animation_side|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1], "btConfirm" });
                    //        break;
                    //}
                    //SAVE LOGS
                }
                else
                {

                    if (robotSide.PersuasionCondition.Equals(RobotsPersuasion.Against))
                    {
                        robotSide.Posture = RobotsPosture.neutral;
                        ThalamusClientRight.CPublisher.ResetPose();
                        ThalamusClientRight.CPublisher.SetPosture("", robotSide.Posture.ToString());
                    }
                    //else
                    //{
                    //    robotSide.Posture = RobotsPosture.pride;
                    //    ThalamusClientRight.CPublisher.ResetPose();
                    //    ThalamusClientRight.CPublisher.SetPosture("", robotSide.Posture.ToString());
                    //}


                    //switch (aux[0])
                    //{
                    //    case "Gaze_PB":
                    //        ThalamusClientRight.PerformUtteranceFromLibrary("Gaze", language, "Gaze_PB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                    //        break;
                    //    case "Gaze_PBP":
                    //        ThalamusClientRight.PerformUtteranceFromLibrary("Gaze", language, "Gaze_PBP", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                    //        break;
                    //    case "Gaze_PBPB":
                    //        ThalamusClientRight.PerformUtteranceFromLibrary("Gaze", "PT", "Gaze_PBPB", new string[] { "|pitch|", "|rate|", "|volume|", "|animation_person|" }, new string[] { robotSide.Pitch, robotSide.Persuasion.Prosody.Rate, robotSide.Persuasion.Prosody.Volume, aux[1] });
                    //        break;
                    //}
                    //SAVE LOGS
                }
            }
        }

        //private string GetGaze(Robot robotSide)
        //{
        //    string animation_prosody = "";
        //    string animation_person = GetPersonOrRandomGaze(robotSide);
        //    switch (robotSide.Persuasion.Gaze.Count(x => x == '-'))
        //    {
        //        case 1://gaze = "Person-Button"
        //            animation_prosody = "Gaze_PB-" + animation_person;
        //            break;
        //        case 2://gaze = "Person-Button-Person"
        //            animation_prosody = "Gaze_PBP-" + animation_person;
        //            break;
        //        case 3://gaze = "Person-Button-Person-Button"
        //            animation_prosody = "Gaze_PBPB-" + animation_person;
        //            break;
        //    }
        //    return animation_prosody;
        //}

        //private string GetPersonOrRandomGaze(Robot robotSide)
        //{
        //    //if return 1 robot will perform more gaze according to his personality such as: domintant will look more to person, Meek will look less to person 
        //    //if return 0 robot will perform gaze such as: domintant will look less to person,  Meek will look more to person
        //    string gazeTo = "";

        //    if (robotSide.Persuasion.Times.Equals(1))//robot will perform gaze 80% of the time
        //    {
        //        if (robotSide.Personality.Equals(Robot.RobotsPersonality.dominant))
        //        {
        //            gazeTo = "person3";
        //        }
        //        else
        //        {
        //            gazeTo = "random";
        //        }
        //    }
        //    else//robot will perform gaze 20% of time
        //    {
        //        if (robotSide.Personality.Equals(Robot.RobotsPersonality.dominant))
        //        {
        //            gazeTo = "random";
        //        }
        //        else
        //        {
        //            gazeTo = "person3";
        //        }
        //    }
        //    return gazeTo;
        //}

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
            if (language == Thalamus.BML.SpeechLanguages.Portuguese) { folder = "PT"; }

            //Console.WriteLine("=========== NEXT AUDIO ============== " + (idScene + 1));

            if (System.IO.File.Exists(ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + audioExt))
            {
                axWindowsMediaPlayer1.URL = ThalamusClientLeft.CPublisher.fileName + @"\\speech\\" + folder + "\\" + (idScene + 1) + audioExt;
                axWindowsMediaPlayer1.settings.volume = 80;
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
                    if (leftRobot.Personality == Robot.RobotsPersonality.dominantNeutral)
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
            WaitAnimation = false;

            //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
            //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());
            string txt = "";
            string txtWeight = "";

            if (!rightRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (rightRobot.PrefSelectedFinal.Equals("-"))
                {
                    SaveFinalPreference(rightRobot, OptionSide.none, 1);
                }

                // PERSONALITY
                personality.BuildPersonality(rightRobot.PrefSelectedFinal);//send the preference of the button chosen
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), rightRobot.PrefSelectedFinal, UserPersonality, rightRobot.Personality.ToString(), rightRobot.PersuasionCondition.ToString());
                //selectedDP.DPPrefSelectedFinal = rightRobot.PrefSelectedFinal;
                //rightRobot.PrefSelectedFinal = selectedDP.DPPrefSelectedFinal;

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                LoadAnimation(rightRobot);

                CallUtterances(rightRobot, 1, OptionSide.none);

                //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
                //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());
                while (WaitAnimation == false)
                {

                }

                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + UserPersonality + rightRobot.ToString(), "ThalamusClientRight", "rightRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + UserPersonality + rightRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");

                ThalamusClientRight.CPublisher.ResetPose();
                if (rightRobot.Personality.Equals(RobotsPersonality.dominantNeutral))
                {
                    ThalamusClientRight.CPublisher.SetPosture("", RobotsPosture.neutral.ToString());
                }
                else if(rightRobot.Personality.Equals(RobotsPersonality.meekNeutral))
                {
                    ThalamusClientRight.CPublisher.SetPosture("", RobotsPosture.neutral.ToString());
                }

            }
            else if (!leftRobot.PersuasionCondition.Equals(RobotsPersuasion.none))
            {
                if (leftRobot.PrefSelectedFinal.Equals("-"))
                {
                    SaveFinalPreference(leftRobot, OptionSide.none, 1);
                }

                // PERSONALITY
                personality.BuildPersonality(leftRobot.PrefSelectedFinal);//send the preference of the button chosen
                txt = personality.RecordPathPersonality(StoryHandler.GetInitialDP(), StoryHandler.GetPrefDP(), leftRobot.PrefSelectedFinal, UserPersonality, leftRobot.Personality.ToString(), leftRobot.PersuasionCondition.ToString());
                //selectedDP.DPPrefSelectedFinal = leftRobot.PrefSelectedFinal;
                //leftRobot.PrefSelectedFinal = selectedDP.DPPrefSelectedFinal;

                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");

                LoadAnimation(leftRobot);

                CallUtterances(leftRobot, 1, OptionSide.none);

                //Console.WriteLine("btConfirm_Click rightRobot " + rightRobot.ToString());
                //Console.WriteLine("btConfirm_Click leftRobot " + leftRobot.ToString());
                while (WaitAnimation == false)
                {

                }

                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + UserPersonality + leftRobot.ToString(), "ThalamusClientLeft", "leftRobot-" + this.UserId.ToString() + ".txt");
                ThalamusClientLeft.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + UserPersonality + leftRobot.ToString(), "ThalamusClientsFull", "Robots-" + this.UserId.ToString() + ".txt");

                ThalamusClientLeft.CPublisher.ResetPose();
                if (leftRobot.Personality.Equals(RobotsPersonality.dominantNeutral))
                {
                    ThalamusClientLeft.CPublisher.SetPosture("", RobotsPosture.neutral.ToString());
                }
                else
                {
                    ThalamusClientLeft.CPublisher.SetPosture("", RobotsPosture.neutral.ToString());
                }
            }

            if (selectedDP.SideSelected.Equals(OptionSide.left))
            {
                //Console.WriteLine(" == Final side: LEFT: " + selectedDP.DPPrefSelectedFinal + " - " + StoryHandler.GetLeftWeight1() + " - " + StoryHandler.GetLeftWeight2());
                personality.BuildPersonalityWeight(selectedDP.DPPrefSelectedFinal, StoryHandler.GetLeftWeight1(), StoryHandler.GetLeftWeight2());//send the preference of the button chosen          
            }
            else if (selectedDP.SideSelected.Equals(OptionSide.right))
            {
                //Console.WriteLine(" == Final side: RIGHT: " + selectedDP.DPPrefSelectedFinal + " - " + StoryHandler.GetRightWeight1() + " - " + StoryHandler.GetRightWeight2());
                personality.BuildPersonalityWeight(selectedDP.DPPrefSelectedFinal, StoryHandler.GetRightWeight1(), StoryHandler.GetRightWeight2());//send the preference of the button chosen
            }

            txtWeight = personality.RecordPathPersonalityWeight();

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txtWeight, "ExtraInfo", "MBTIWeight-" + this.UserId.ToString() + ".txt");

            DeletePreferenceUsed();
            rightRobot.PersuasionCondition = RobotsPersuasion.none;
            leftRobot.PersuasionCondition = RobotsPersuasion.none;

            this.btConfirm.Visible = false;
            this.btConfirm.Enabled = false;
            this.labelLeftButton.Visible = this.labelRightButton.Visible = false;
            this.rightButton.Visible = this.leftButton.Visible = false;

            StoryHandler.NextScene(selectedDP);

            //selectedDP.DPPrefIntention = "-";
            //selectedDP.DpPrefPair = "-";
            //selectedDP.DPPrefSelectedFinal = "-";
            //selectedDP.OptionSelected = -1;
            //selectedDP.SideSelected = OptionSide.none;
            selectedDP = new SelectionDP();

            rightRobot.PreferencePair = "-";
            rightRobot.PrefSelectedIntention = "-";
            rightRobot.PrefSelectedFinal = "-";
            leftRobot.PreferencePair = "-";
            leftRobot.PrefSelectedIntention = "-";
            leftRobot.PrefSelectedFinal = "-";

            rightButton.Highlight = false;
            leftButton.Highlight = false;
            rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            leftButton.Theme = MetroFramework.MetroThemeStyle.Dark;

            Console.WriteLine("start timer {0:HH:mm:ss.fff}", DateTime.Now);

            // waiting 2 seconds to start the next method
            var waitTime = new TimeSpan(0, 0, 2);
            var waitUntil = DateTime.Now + waitTime;
            int secs = 0;
            while (DateTime.Now <= waitUntil)
            {
                secs++;
                Console.WriteLine("Waiting: " + secs);
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("end timer {0:HH:mm:ss.fff}", DateTime.Now);
            CallNextScene();
        }

       

        private void DeletePreferenceUsed()
        {
            string preferenceSelected = StoryHandler.GetPrefDP();
            //Console.WriteLine("Deleted preference " + preferenceSelected);

            switch (preferenceSelected)
            {
                case "ei":
                case "ie":
                    PreferenceEI.RemoveAt(PreviousPrefSelected[0, 1]);
                    break;
                case "si":
                case "is":
                    PreferenceSN.RemoveAt(PreviousPrefSelected[0, 1]);
                    break;
                case "tf":
                case "ft":
                    PreferenceTF.RemoveAt(PreviousPrefSelected[0, 1]);
                    break;
                case "jp":
                case "pj":
                    PreferenceJP.RemoveAt(PreviousPrefSelected[0, 1]);
                    break;
                case "-":
                    preferenceDG.RemoveAt(PreviousPrefSelected[0, 1]);
                    break;
            }
        }

        private void recordFinalLog()
        {
            string txt = "\r\n ============================= \r\n" +
                         "Total Favour Left: " + leftRobot.TotalFavour + "\r\n" +
                         "Total Favour Right: " + rightRobot.TotalFavour + "\r\n" +
                         "Total Favour: " + (leftRobot.TotalFavour + rightRobot.TotalFavour) + "\r\n" +
                         "Total Against Left: " + leftRobot.TotalAgainst + "\r\n" +
                         "Total Against Right: " + rightRobot.TotalAgainst + "\r\n" +
                         "Total Against: " + (leftRobot.TotalAgainst + rightRobot.TotalAgainst) + "\r\n" +
                         "============================= \r\n";

            txt += personality.DefineMBTIPersonality();

            txt += "\r\n ============================= \r\n" +
                   personality.DefineMBTIPersonalityWeight();

            ThalamusClientRight.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), ";" + txt, "ExtraInfo", "ExtraInfo-" + this.UserId.ToString() + ".txt");
        }

        internal void RobotsIntroduction()
        {
            // Categories = intro_dom, intro_dom2, intro_meek | sub = en, pt
            string lang = "PT";
            if (this.Language == Thalamus.BML.SpeechLanguages.English)
            {
                lang = "EN";
            }
            if (leftRobot.Personality.Equals(RobotsPersonality.dominantNeutral))
            {
                ThalamusClientLeft.PerformUtteranceFromLibrary("Intro1", "intro_dom", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { leftRobot.Pitch, leftRobot.Persuasion.Prosody.Rate, leftRobot.Persuasion.Prosody.Volume });
                Console.WriteLine(" LEFT ");
            }
            else
            {
                ThalamusClientRight.PerformUtteranceFromLibrary("Intro1", "intro_dom", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { rightRobot.Pitch, rightRobot.Persuasion.Prosody.Rate, rightRobot.Persuasion.Prosody.Volume });
            }
            while (WaitIntro1 == false)
            {

            }
            if (leftRobot.Personality.Equals(RobotsPersonality.meekNeutral))
            {
                ThalamusClientLeft.PerformUtteranceFromLibrary("Intro2", "intro_meek", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { leftRobot.Pitch, leftRobot.Persuasion.Prosody.Rate, leftRobot.Persuasion.Prosody.Volume });
            }
            else
            {
                ThalamusClientRight.PerformUtteranceFromLibrary("Intro2", "intro_meek", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { rightRobot.Pitch, rightRobot.Persuasion.Prosody.Rate, rightRobot.Persuasion.Prosody.Volume });
            }
            while (WaitIntro2 == false)
            {

            }
            if (leftRobot.Personality.Equals(RobotsPersonality.dominantNeutral))
            {
                ThalamusClientLeft.PerformUtteranceFromLibrary("Intro3", "intro_dom2", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { leftRobot.Pitch, leftRobot.Persuasion.Prosody.Rate, leftRobot.Persuasion.Prosody.Volume });
            }
            else
            {
                ThalamusClientRight.PerformUtteranceFromLibrary("Intro3", "intro_dom2", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { rightRobot.Pitch, rightRobot.Persuasion.Prosody.Rate, rightRobot.Persuasion.Prosody.Volume });
            }
            while (WaitIntro3 == false)
            {

            }
            if (leftRobot.Personality.Equals(RobotsPersonality.meekNeutral))
            {
                ThalamusClientLeft.PerformUtteranceFromLibrary("Intro4", "intro_meek2", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { leftRobot.Pitch, leftRobot.Persuasion.Prosody.Rate, leftRobot.Persuasion.Prosody.Volume });
                ThalamusClientRight.PerformUtteranceFromLibrary("Intro5", "intro_dom3", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { rightRobot.Pitch, rightRobot.Persuasion.Prosody.Rate, rightRobot.Persuasion.Prosody.Volume });
            }
            else
            {
                ThalamusClientRight.PerformUtteranceFromLibrary("Intro4", "intro_meek2", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { rightRobot.Pitch, rightRobot.Persuasion.Prosody.Rate, rightRobot.Persuasion.Prosody.Volume });
                ThalamusClientLeft.PerformUtteranceFromLibrary("Intro5", "intro_dom3", lang, new string[] { "|pitch|", "|rate|", "|volume|" }, new string[] { leftRobot.Pitch, leftRobot.Persuasion.Prosody.Rate, leftRobot.Persuasion.Prosody.Volume });
            }
            while (WaitIntro4 == false)
            {

            }
        }
    }
}
