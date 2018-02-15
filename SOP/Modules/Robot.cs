using StoryOfPersonality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Modules
{
    public class Robot
    {
        private RobotsPersonality personality;
        private Persuasion persuasion;
        private int consecutivePlays;
        private int oponentPlays;
        private int condition;

        public Robot(RobotsPersonality robotPersonality)
        {
            personality = robotPersonality;
            consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.condition = -1;
        }

        //public Robot(RobotsPersonality personality, Persuasion persuasion)
        //{
        //    this.personality = personality;
        //    this.consecutivePlays = 0;
        //    this.persuasion = persuasion;
        //    this.condition = -1;
        //}

        public Robot(RobotsPersonality personality, Persuasion persuasion, int condition)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.condition = condition;
        }

        public Robot(RobotsPersonality personality, int condition)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.condition = condition;
        }

        public RobotsPersonality Personality { get => personality; set => personality = value; }
        public Persuasion Persuasion { get => persuasion; set => persuasion = value; }
        public int ConsecutivePlays { get => consecutivePlays; set => consecutivePlays = value; }
        public int Condition { get => condition; set => condition = value; }
        public int OponentPlays { get => oponentPlays; set => oponentPlays = value; }

        public enum RobotsPersonality
        {
            none = -1,
            assertive = 0,
            dominant = 1
        }

        public override string ToString()
        {
           // return "RobotsPersonality: " + personality + " Persuasion: " + persuasion + " ConsecutivePlays: " + consecutivePlays + " OponentPlays: " + oponentPlays + " Persuasion: " + persuasion.ToString() + " Condition: " + condition;
            return "" + personality + ";" + persuasion + ";" + consecutivePlays + ";" + oponentPlays + ";" + persuasion.ToString() + ";" + condition;

        }
    }
}
