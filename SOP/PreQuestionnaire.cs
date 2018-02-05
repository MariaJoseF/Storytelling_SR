using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOP
{
    public partial class PreQuestionnaire : MetroFramework.Forms.MetroForm
    {
        private const string PRE_QUESTIONNAIRE_FILE = "pre-questions.txt";
        private string OUTPUT_FILE = "pre-questions-output/pre-";
        private const string MBTI_QUESTIONNAIRE_FILE = "mbti-questions.txt";
        private string[] questions;
        private string[] mbti_questions;
        private int[][] mbti_scores;
        private int question_id = 0;
        public int UserId;
        private Func<bool> questionHandler;
        private MetroFramework.Controls.MetroRadioButton[] radioButtons;
        public PreQuestionnaire(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
            OUTPUT_FILE = OUTPUT_FILE + this.UserId.ToString() + "-"  + DateTime.Now.ToString().Replace('/','.').Replace(' ','.').Replace(':','.') + ".log";
            Directory.CreateDirectory(OUTPUT_FILE.Substring(0, OUTPUT_FILE.IndexOf('/')));
            questions = File.ReadAllLines(PRE_QUESTIONNAIRE_FILE);
            mbti_questions = File.ReadAllLines(MBTI_QUESTIONNAIRE_FILE);
            mbti_scores = new int[4][];
            for (int i = 0; i < mbti_scores.Length; i++)
                mbti_scores[i] = new int[2];

            radioButtons = new MetroFramework.Controls.MetroRadioButton[4] { metroRadioButton1, metroRadioButton2, metroRadioButton3 , metroRadioButton4};
            metroLabel1.Left = metroTrackBar1.Left;
            metroTextBox1.Visible = false;
            metroTrackBar1.Visible = false;
            metroLabel2.Visible = false;
            metroLabel3.Visible = false;
            metroLabel4.Visible = false;
            this.metroButton3.Visible = false;

            foreach (var rb in radioButtons)
            {
                rb.Visible = false;
                // all radio buttons when clicked act like the "NEXT" button to be quicker
                rb.CheckedChanged += new System.EventHandler(this.metroButton2_Click);
            }
        }

        private void writeLog(string log)
        {
            File.AppendAllText(OUTPUT_FILE, log + "\r\n");
        }

        private Func<bool> loadMBTIQuestion(string[] question)
        {
            Func<bool> answerHandler = null;
            this.Text = "MBTI Questionnaire";
            this.Name = "MBTI Questionnaire";
            this.metroLabel1.Text = question[0].Substring(question[0].IndexOf('.') + 1);
            this.radioButtons[0].Text = question[1].Substring(question[1].IndexOf('.') + 1);
            this.radioButtons[0].Visible = true;
            this.radioButtons[1].Text = question[2].Substring(question[2].IndexOf('.') + 1);
            this.radioButtons[1].Visible = true;
            answerHandler = () =>
            {
                if (!Array.Exists(radioButtons, value => value.Checked))
                    return false; // if no button is checked
                for (int i = 0; i < radioButtons.Length; i++)
                {
                    if (radioButtons[i].Checked) // i should only be 0 or 1
                    {
                        writeLog("mbti" + question[0].Substring(0, question[0].IndexOf('.')) + (i == 0 ? "A" : "B"));
                        int index = question_id - this.questions.Length;
                        index = (index % 7 + 1) / 2; // all indexes will be converted to (1-7) , then (0-6) then (0-3) effectivly putting correct indexes together
                        mbti_scores[index][i]++;
                    }
                    radioButtons[i].Visible = false;
                    radioButtons[i].Checked = false;
                }
                return true;
            };
            return answerHandler;
        }

        private Func<bool> loadQuestion(string question)
        {
            Func<bool> answerHandler;
            string key = question.Substring(1, question.IndexOf(']')-1);
            metroLabel1.Text = question.Substring(question.IndexOf(']')+1);
            switch (key)
            {
                case "text":
                    metroTextBox1.Visible = true;
                    answerHandler = () =>
                    {
                        if (metroTextBox1.Text == "") return false;
                        writeLog(metroTextBox1.Text);
                        metroTextBox1.Text = "";
                        metroTextBox1.Visible = false;
                        return true;
                    };
                    break;
                case "1-10":
                    metroTrackBar1.Visible = true;
                    metroLabel2.Visible = true;
                    metroLabel3.Visible = true;
                    metroLabel4.Visible = true;
                    answerHandler = () =>
                    {
                        writeLog(metroTrackBar1.Value.ToString());
                        metroTrackBar1.Value = 5;
                        metroTrackBar1.Visible = false;
                        metroLabel2.Visible = false;
                        metroLabel3.Visible = false;
                        metroLabel4.Visible = false;
                        return true;
                    };
                    break;
                default: // multiple choice question(supports <= 3 items only)
                    string[] keys = key.Split('/');
                    for (int i = 0; i < keys.Length; i++) { 
                        radioButtons[i].Visible = true;
                        radioButtons[i].Text = keys[i];
                    }
                    answerHandler = () =>
                    {
                        if (!Array.Exists(radioButtons, value => value.Checked))
                            return false; // if no button is checked
                        foreach (var rb in radioButtons)
                        {
                            if (rb.Checked) writeLog(rb.Text);
                            rb.Visible = false;
                            rb.Checked = false;
                        }
                        return true;
                    };
                    break;
            }
            return answerHandler;
        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PreQuestionnaire_Load(object sender, EventArgs e)
        {
            if (question_id < questions.Length)
                this.questionHandler = this.loadQuestion(questions[question_id]);
        }

        private void CheckKeys(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // pressing the enter key
			{
                this.metroButton2_Click(sender, e);
			}
		}

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (metroButton2.Text == "FINISH")
                Application.Exit();

            if (!this.questionHandler()) return;

            this.question_id++;
            if (question_id < questions.Length) 
                this.questionHandler = this.loadQuestion(questions[question_id]);
            else if (question_id - questions.Length < mbti_questions.Length)
            {
                this.metroButton3.Visible = true;
                int index = question_id - questions.Length;
                var segment = new ArraySegment<string>(mbti_questions, index, 3);
                this.questionHandler = this.loadMBTIQuestion(segment.ToArray());
                this.question_id += 2;
            }
            else
            {
                string[] values = new string[] { "EI", "SN", "TF", "JP" };
                string mbti_type = "";
                for (int i = 0; i < mbti_scores.Length; i++)
                    mbti_type += values[i][mbti_scores[i][0] > mbti_scores[i][1] ? 0 : 1];
                writeLog("MBTI TYPE: " + mbti_type);
                metroLabel1.Text = "Thanks for Participating! Time to try the story.";
                metroButton2.Text = "FINISH";
                metroButton3.Visible = false;
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (this.question_id <= questions.Length + 3)
                return;
            Console.WriteLine(question_id);
            Console.WriteLine(questions.Length);
            writeLog("% Ignore previous Line");
            question_id -= 5;
            int index = question_id - questions.Length;
            var segment = new ArraySegment<string>(mbti_questions, index, 3);
            this.questionHandler = this.loadMBTIQuestion(segment.ToArray());
            question_id += 2;
        }
    }
}
