using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryOfPersonality
{
    class Persuasion
    {
        private string gaze;
        private int time;
        private string animation;
        private Prosody prosody;


        public Persuasion(string _gaze, int _time, string _animation)
        {
            this.gaze = _gaze;
            this.time = _time;
            this.animation = _animation;
        }

        public string Gaze { get => gaze; set => gaze = value; }
        public int Time { get => time; set => time = value; }
        public string Animation { get => animation; set => animation = value; }
        internal Prosody Prosody { get => prosody; set => prosody = value; }

        public override string ToString()
        {
            return "Gaze: " + gaze + " Time: " + time + " Animation: " + animation;
        }
    }
}
