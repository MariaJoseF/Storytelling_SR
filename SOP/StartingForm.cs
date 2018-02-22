using StoryOfPersonality;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOP
{
    public partial class StartingForm : MetroFramework.Forms.MetroForm
    {
        public int UserID;
        public StartingForm()
        {
            InitializeComponent();
            this.UserID = new Random().Next(9999);
            this.metroLabel1.Text = this.metroLabel1.Text + this.UserID.ToString();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            SOP.PreQuestionnaire s = new SOP.PreQuestionnaire(this.UserID);
            s.Show();
            this.Hide();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Count(x => x == '-') == 4)
            {
                StoryForm s = new StoryForm(metroTextBox1.Text);
                s.Show();
                this.Hide();
            }
            else
            {
                this.label1.Visible = true;
            }

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            SOP.PostQuestionnaire s = new SOP.PostQuestionnaire(this.UserID);
            s.Show();
            this.Hide();
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.UserID = Int32.Parse(this.metroTextBox1.Text);
            }
            catch (FormatException)
            {

            }
        }
    }
}
