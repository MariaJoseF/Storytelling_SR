using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Modules
{
    public class SelectionDP
    {
        private int optionSelected;
        private int robotPersonality;
        private int persLvl;
        private int persIntensity;
        private int totalDominant;
        private int totalAssertive;
        private long elapsedMs;

        public SelectionDP()
        {
            this.optionSelected = 0;
            this.robotPersonality = -1;
            this.persLvl = 0;
            this.persIntensity = 0;
            this.totalDominant = 0;
            this.totalAssertive = 0;
            this.elapsedMs = 0;
        }

        public int OptionSelected { get => optionSelected; set => optionSelected = value; }
        public int PersLvl { get => persLvl; set => persLvl = value; }
        public int PersIntensity { get => persIntensity; set => persIntensity = value; }
        public int TotalDominant { get => totalDominant; set => totalDominant = value; }
        public int TotalAssertive { get => totalAssertive; set => totalAssertive = value; }
        public int RobotPersonality { get => robotPersonality; set => robotPersonality = value; }
        public long ElapsedMs { get => elapsedMs; set => elapsedMs = value; }

        public override string ToString()
        {
            return optionSelected + ";" + robotPersonality + ";" + persLvl + ";" + persIntensity + ";" + totalDominant + ";" + totalAssertive + ";" + elapsedMs;
        }
    }
}
