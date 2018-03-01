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
        private int totalMeek;
        private long elapsedMs;
        private string dpPref;
        private string dpPrefSelected;

        public SelectionDP()
        {
            this.optionSelected = 0;
            this.sideSelected = OptionSide.none;
            this.robotPersonality = RobotsPersonality.none;
            this.persLvl = 0;
            this.persIntensity = 0;
            this.totalDominant = 0;
            this.totalMeek = 0;
            this.elapsedMs = 0;
            this.dpPref = "";
            this.dpPrefSelected = "";
        }

        public int OptionSelected { get => optionSelected; set => optionSelected = value; }
        public int PersLvl { get => persLvl; set => persLvl = value; }
        public int PersIntensity { get => persIntensity; set => persIntensity = value; }
        public int TotalDominant { get => totalDominant; set => totalDominant = value; }
        public int TotalMeek { get => totalMeek; set => totalMeek = value; }
        public RobotsPersonality RobotPersonality { get => robotPersonality; set => robotPersonality = value; }
        public long ElapsedMs { get => elapsedMs; set => elapsedMs = value; }
        public OptionSide SideSelected { get => sideSelected; set => sideSelected = value; }
        public string DPPref { get => dpPref; set => dpPref = value; }
        public string DPPrefSelected { get => dpPrefSelected; set => dpPrefSelected = value; }

        public override string ToString()
        {
            return optionSelected + ";" + sideSelected + ";" + robotPersonality + ";" + persLvl + ";" + persIntensity + ";" + totalDominant + ";" + totalMeek + ";" + elapsedMs + ";" + DPPref + ";" + DPPrefSelected;
        }
    }
}
