using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SOP.Modules.Robot;
//using static StoryOfPersonality.StoryForm;

namespace StoryOfPersonality
{
    public class Persuasion
    {
        //private string gaze;
        //private int times;
        private string animation;
        private Prosody prosody;

        public Persuasion()
        {
            //this.gaze = "";
            //this.times = -1;
            this.animation = "";
            this.prosody = new Prosody();
        }

        //public Persuasion(/*string gaze, int time, */string animation)
        //{
        //    //this.gaze = gaze;
        //    //this.times = time;
        //    this.animation = animation;
        //    this.prosody = new Prosody();
        //}

        //public Persuasion(/*string gaze, int time,*/ string animation, Prosody prosody)
        //{
        //    //this.gaze = gaze;
        //    //this.times = time;
        //    this.animation = animation;
        //    this.prosody = prosody;
        //}

        //public string Gaze { get => gaze; set => gaze = value; }
        //public int Times { get => times; set => times = value; }
        public string Animation { get => animation; set => animation = value; }
        internal Prosody Prosody { get => prosody; set => prosody = value; }

        public override string ToString()
        {
            //return "Gaze: " + gaze + " Times: " + times + " Animation: " + animation + " Prosody: " + prosody.ToString();
            //return "" + gaze + ";" + times + ";" + animation + ";" + prosody.ToString();
            //return "" + gaze + ";" + animation + ";" + prosody.ToString();
            return ""  + animation + ";" + prosody.ToString();


        }

    }
}
