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
    public partial class PostQuestionnaire : MetroFramework.Forms.MetroForm
    {
        private const string POST_QUESTIONNAIRE_FILE = "post-questions.txt";
        private string OUTPUT_FILE = "post-questions-output/post-";
        private string[] questions;
        private int question_id = 0;
        public int UserId;
        public PostQuestionnaire(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
            OUTPUT_FILE = OUTPUT_FILE + this.UserId.ToString() + "-" + DateTime.Now.ToString().Replace('/','.').Replace(' ','.').Replace(':','.') + ".log";
            Directory.CreateDirectory(OUTPUT_FILE.Substring(0, OUTPUT_FILE.IndexOf('/')));
            questions = File.ReadAllLines(POST_QUESTIONNAIRE_FILE);
            metroLabel1.Left = metroTrackBar1.Left;
        }

        private void PostQuestionnaire_Load(object sender, EventArgs e)
        {
            if (question_id < questions.Length)
                metroLabel1.Text = questions[question_id];
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (metroButton2.Text == "FINISH")
                Application.Exit();

            File.AppendAllText(OUTPUT_FILE, metroTrackBar1.Value.ToString() + "\r\n");
            metroTrackBar1.Value = 5;
            question_id++;
            if (question_id < questions.Length) 
                metroLabel1.Text = questions[question_id];
            else
            {
                metroLabel1.Text = "Thanks for Participating!";
                metroTrackBar1.Visible = false;
                metroLabel2.Visible = false;
                metroLabel3.Visible = false;
                metroLabel4.Visible = false;
                metroButton2.Text = "FINISH";
            }
        }
    }
}
