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
        private int totalDominant;
        private int totalMeek;
        private long elapsedMs;
        private string dpPrefIntention;
        private string dpPrefSelectedFinal;
        private string dpPrefPair;

        public SelectionDP()
        {
            this.optionSelected = 0;
            this.sideSelected = OptionSide.none;
            this.robotPersonality = RobotsPersonality.none;
            this.totalDominant = 0;
            this.totalMeek = 0;
            this.elapsedMs = 0;
            this.dpPrefIntention = "";
            this.dpPrefSelectedFinal = "";
            this.dpPrefPair = "";
        }

        public int OptionSelected { get => optionSelected; set => optionSelected = value; }
        public int TotalDominant { get => totalDominant; set => totalDominant = value; }
        public int TotalMeek { get => totalMeek; set => totalMeek = value; }
        public RobotsPersonality RobotPersonality { get => robotPersonality; set => robotPersonality = value; }
        public long ElapsedMs { get => elapsedMs; set => elapsedMs = value; }
        public OptionSide SideSelected { get => sideSelected; set => sideSelected = value; }
        public string DPPrefIntention { get => dpPrefIntention; set => dpPrefIntention = value; }
        public string DPPrefSelectedFinal { get => dpPrefSelectedFinal; set => dpPrefSelectedFinal = value; }
        public string DpPrefPair { get => dpPrefPair; set => dpPrefPair = value; }

        public override string ToString()
        {
            return optionSelected + ";" + sideSelected + ";" + robotPersonality + ";" + totalDominant + ";" + totalMeek  + ";" + dpPrefPair + ";" + dpPrefIntention + ";" + dpPrefSelectedFinal + ";" + elapsedMs;
        }
    }
}
