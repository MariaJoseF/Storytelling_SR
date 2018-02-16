using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SOP.Modules.Robot;
using static StoryOfPersonality.StoryForm;

namespace SOP.Modules
{
    public class SelectionDP
    {
        private int optionSelected;
        private OptionSide sideSelected;
        private RobotsPersonality robotPersonality;
        private int persLvl;
        private int persIntensity;
        private int totalDominant;
        private int totalAssertive;
        private long elapsedMs;

        public SelectionDP()
        {
            this.optionSelected = 0;
            this.sideSelected = OptionSide.none;
            this.robotPersonality = RobotsPersonality.none;
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
        public RobotsPersonality RobotPersonality { get => robotPersonality; set => robotPersonality = value; }
        public long ElapsedMs { get => elapsedMs; set => elapsedMs = value; }
        public OptionSide SideSelected { get => sideSelected; set => sideSelected = value; }

        public override string ToString()
        {
            return optionSelected + ";" + sideSelected + ";" + robotPersonality + ";" + persLvl + ";" + persIntensity + ";" + totalDominant + ";" + totalAssertive + ";" + elapsedMs;
        }
    }
}
