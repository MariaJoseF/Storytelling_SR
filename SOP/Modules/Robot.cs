using StoryOfPersonality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Modules
{
    class Robot
    {
        private RobotsPersonality personality;
        private Persuasion persuasion;
        private int consecutivePlays;

        public Robot(RobotsPersonality robotPersonality)
        {
            this.personality = robotPersonality;
            this.consecutivePlays = 0;
            this.persuasion = null;
        }

        public Robot(RobotsPersonality personality, Persuasion persuasion)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.persuasion = persuasion;
        }

        internal RobotsPersonality Personality { get => personality; set => personality = value; }
        public Persuasion Persuasion { get => persuasion; set => persuasion = value; }
        public int ConsecutivePlays { get => consecutivePlays; set => consecutivePlays = value; }

        internal enum RobotsPersonality
        {
            none = -1,
            assertive = 0,
            dominant = 1
        }
    }
}
