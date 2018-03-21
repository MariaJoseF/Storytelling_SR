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
        private string txtButton = "LET'S BEGIN";

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
            ThalamusClientRight = new Client(OptionSide.right, Language, "Dominant");
            //ThalamusClientRight.CPublisher.ChangeLibrary("rightUtterances");

            ThalamusClientLeft = new Client(OptionSide.left, Language, "Meek");
            //ThalamusClientLeft.CPublisher.ChangeLibrary("leftUtterances");

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
            if (languageSelector.Text == "English")
            {
                this.metroButton4Story.Text = txtButton;
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

            ThalamusClientLeft.CPublisher.ChangeLibrary(ThalamusClientLeft.CPublisher.fileName + @"\Utterances\phrasesAgainstPositive.xlsx");
            ThalamusClientRight.CPublisher.ChangeLibrary(ThalamusClientRight.CPublisher.fileName + @"\Utterances\phrasesAgainstPositive.xlsx");
            //this.sceneBox.Text = this.StoryHandler.GetSceneUtterance(this.Language);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            string[] aux = UserId.Split('-');

            if ((Convert.ToInt32(aux[1]) == 1))
            {
                rightRobot = new Robot(Robot.RobotsPersonality.dominant,Robot.RobotSide.right, this.Language);
                leftRobot = new Robot(Robot.RobotsPersonality.meek, Robot.RobotSide.left, this.Language);
            }
            else
            {
                rightRobot = new Robot(Robot.RobotsPersonality.meek, Robot.RobotSide.right, this.Language);
                leftRobot = new Robot(Robot.RobotsPersonality.dominant, Robot.RobotSide.left, this.Language);
            }

            this.storyForm = new StoryForm(UserId, ThalamusClientRight, ThalamusClientLeft, rightRobot, leftRobot);
            this.storyForm.Language = this.Language;

            this.ThalamusClientLeft.StoryWindow(storyForm);
            this.ThalamusClientRight.StoryWindow(storyForm);
            this.storyForm.Show();
            this.Hide();
        }
    }
}
