using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus.BML;

namespace SOP.Modules
{
    public class Prosody
    {
        //private int lvl;
        //private int intensity;
        private string rate;
        //private string pitch;
        private string volume;
        private string utterance;
        private RobotsLanguage language;

        public enum RobotsLanguage
        {
            none = -1,
            EN = 0,
            PT = 1
        }

        public Prosody()
        {
            //this.lvl = 0;
            //this.intensity = 0;
            this.rate = "medium";
            //this.pitch = "";
            this.volume = "soft";
            this.utterance = "";
            this.language = RobotsLanguage.none;
        }

        public Prosody(int lvl, int intensity, string rate, /*string pitch,*/ string volume, string utterance, RobotsLanguage language)
        {
            //this.lvl = lvl;
            //this.intensity = intensity;
            this.rate = rate;
           // this.pitch = pitch;
            this.volume = volume;
            this.utterance = utterance;
            this.language = language;
        }

        public Prosody(string rate, string volume)
        {
            this.rate = rate;
            this.volume = volume;
            this.utterance = "";
            this.language = RobotsLanguage.none;
        }

        public Prosody(RobotsLanguage language)
        {
            this.language = language;
        }

        public string Rate { get => rate; set => rate = value; }
        //public string Pitch { get => pitch; set => pitch = value; }
        public string Volume { get => volume; set => volume = value; }
        //public int Lvl { get => lvl; set => lvl = value; }
        //public int Intensity { get => intensity; set => intensity = value; }
        public RobotsLanguage Language { get => language; set => language = value; }
        public string Utterance { get => utterance; set => utterance = value; }

        public override string ToString()
        {
            //return "Level: " + lvl + " Intensity: " + intensity + " Rate: " + rate + " Pitch: " + pitch + " Volume: " + volume + " Utterance: " + utterance + " Language: " + language;
            //return "" + lvl + ";" + intensity + ";" + rate + ";" + pitch + ";" + volume + ";" + utterance + ";" + language;
            //return "" + lvl + ";" + intensity + ";" + rate + ";" + volume + ";" + utterance + ";" + language;
            return "" + rate + ";" + volume + ";" + utterance + ";" + language;
        }
    }
}