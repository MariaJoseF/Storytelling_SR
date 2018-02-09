using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Modules
{
    class Prosody
    {
        private int lvl;
        private int intensity;
        private string rate;
        private string pitch;
        private string volume;

        //public Prosody(string _rate, string _pitch, string _volume)
        //{
        //    this.rate = _rate;
        //    this.pitch = _pitch;
        //    this.volume = _volume;
        //}

        //public Prosody(int _lvl, string _rate, string _pitch, string _volume)
        //{
        //    this.lvl = _lvl;
        //    this.rate = _rate;
        //    this.pitch = _pitch;
        //    this.volume = _volume;
        //}

        public Prosody(int lvl, int intensity, string rate, string pitch, string volume)
        {
            this.lvl = lvl;
            this.intensity = intensity;
            this.rate = rate;
            this.pitch = pitch;
            this.volume = volume;
        }

        public string Rate { get => rate; set => rate = value; }
        public string Pitch { get => pitch; set => pitch = value; }
        public string Volume { get => volume; set => volume = value; }
        public int Lvl { get => lvl; set => lvl = value; }
        public int Intensity { get => intensity; set => intensity = value; }

        public override string ToString()
        {
            return "Level: "+ lvl + " Intensity: "+intensity+" Rate: " + rate + " Pitch: " + pitch + " Volume: " + volume;
        }
    }
}
