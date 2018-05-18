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
    public partial class StartAdventure : MetroFramework.Forms.MetroForm
    {

        private static StartAdventure instance = null;
        private string UserId;

        public Client ThalamusClientLeft;
        public Client ThalamusClientRight;
        public Thalamus.BML.SpeechLanguages Language;

        private Robot leftRobot;
        private Robot rightRobot;

        internal Robot LeftRobot { get => leftRobot; set => leftRobot = value; }
        internal Robot RightRobot { get => rightRobot; set => rightRobot = value; }

        public StoryForm storyForm;

        public enum OptionSide
        {
            none = -1,
            left = 0,
            right = 1
        }

        public static StartAdventure Instance { get => instance; set => instance = value; }

        public StartAdventure(string UserId)
        {
            InitializeComponent();

            //string[] aux = UserId.Split('-');
            this.UserId = UserId;

            this.Language = Thalamus.BML.SpeechLanguages.English;
            
            string[] aux = UserId.Split('-');

            if (aux[1] == "D")
            {
                ThalamusClientRight = new Client(OptionSide.right, Language, "Dominant");
                //ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

                ThalamusClientLeft = new Client(OptionSide.left, Language, "Meek");
                //ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");
            }
            else
            {
                ThalamusClientRight = new Client(OptionSide.right, Language, "Meek");
                //ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

                ThalamusClientLeft = new Client(OptionSide.left, Language, "Dominant");
                //ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");
            }


            instance = this;
        }

        /**********************
         *    GUI EVENTS
         **********************/

        private void StoryForm_Load(object sender, EventArgs e)
        {
            this.languageSelector.Text = "English"; // put this as default
            this.CropAndStrechBackImage();
        }

        private void StoryForm_Resize(object sender, EventArgs e)
        {
            this.CropAndStrechBackImage();
        }

        private void CropAndStrechBackImage()
        {
            this.backImage.Size = this.Size;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ThalamusClientLeft.Shutdown();
            ThalamusClientRight.Shutdown();
            Application.Exit();
        }

        private void LanguageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newSize = 80;
            this.metroButton4Story.Font = new Font(this.metroButton4Story.Font.FontFamily, newSize);
            this.metroButton4Story.Visible = true;
            SetLanguage();
           
            ThalamusClientLeft.CPublisher.ChangeLibrary(ThalamusClientLeft.CPublisher.fileName + @"\Utterances\phrasesAgainstPositive.xlsx");
            ThalamusClientRight.CPublisher.ChangeLibrary(ThalamusClientRight.CPublisher.fileName + @"\Utterances\phrasesAgainstPositive.xlsx");
            //this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
        }

        private void SetLanguage()
        {
            if (languageSelector.Text == "English")
            {
                this.metroButton4Story.Text = "LET'S BEGIN";
                this.Language = Thalamus.BML.SpeechLanguages.English;
                ThalamusClientLeft.CPublisher.SetLanguage(this.Language);
                ThalamusClientRight.CPublisher.SetLanguage(this.Language);
            }
            else
            {
                this.metroButton4Story.Text = "VAMOS COMEÇAR!";
                this.Language = Thalamus.BML.SpeechLanguages.Portuguese;
                ThalamusClientLeft.CPublisher.SetLanguage(this.Language);
                ThalamusClientRight.CPublisher.SetLanguage(this.Language);
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.languageSelector.Visible = false;
            this.metroButton4Story.Visible = false;
            this.languageSelector.Enabled = false;

            if (languageSelector.Text == "English")
            {
                this.metroButton4Story.Text = "Counselours Information";
            }
            else
            {
                this.metroButton4Story.Text = "Informações dos Conselheiros";
            }

            string[] aux = UserId.Split('-');

            if (aux[1] == "D")
            {
               
                if (aux[2] == "P")
                {
                    rightRobot = new Robot(Robot.RobotsPersonality.dominant, Robot.RobotSide.right, this.Language);
                    leftRobot = new Robot(Robot.RobotsPersonality.meek, Robot.RobotSide.left, this.Language);
                }
                else
                {
                    rightRobot = new Robot(Robot.RobotsPersonality.neutral, Robot.RobotSide.right, this.Language);
                    leftRobot = new Robot(Robot.RobotsPersonality.neutral, Robot.RobotSide.left, this.Language);
                }
            }
            else
            {
                if (aux[2] == "P")
                {
                    rightRobot = new Robot(Robot.RobotsPersonality.meek, Robot.RobotSide.right, this.Language);
                    leftRobot = new Robot(Robot.RobotsPersonality.dominant, Robot.RobotSide.left, this.Language);
                }
                else
                {
                    rightRobot = new Robot(Robot.RobotsPersonality.neutral, Robot.RobotSide.right, this.Language);
                    leftRobot = new Robot(Robot.RobotsPersonality.neutral, Robot.RobotSide.left, this.Language);
                }
            }

            this.storyForm = new StoryForm(UserId, ThalamusClientRight, ThalamusClientLeft, rightRobot, leftRobot);
            this.storyForm.Language = this.Language;
            SetLanguage();

            this.ThalamusClientLeft.StoryWindow(storyForm);
            this.ThalamusClientRight.StoryWindow(storyForm);
            this.storyForm.Show();
            this.Hide();

        }
    }
}
