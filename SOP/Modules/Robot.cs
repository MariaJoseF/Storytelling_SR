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
        private string pitch;
        private int totalDominant;
        private int totalMeek;

        private RobotsPosture posture;


        public Robot(RobotsPersonality robotPersonality, string _ptich)
        {
            personality = robotPersonality;
            consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.condition = -1;
            this.pitch = _ptich;
        }

        public Robot(RobotsPersonality personality, Persuasion persuasion, int condition)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = persuasion;
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
        public string Pitch { get => pitch; set => pitch = value; }
        public int TotalDominant { get => totalDominant; set => totalDominant = value; }
        public int TotalMeek { get => totalMeek; set => totalMeek = value; }
        public RobotsPosture Posture { get => posture; set => posture = value; }

        public enum RobotsPersonality
        {
            none = -1,
            meek = 0,
            dominant = 1
        }

        public enum RobotsPosture
        {
            admiration = 0,
            anger = 1,
            contempt = 2,
            disappointment = 3,
            hope = 4,
            joy = 5,
            pride = 6,
            satisfaction = 7
        }


        public override string ToString()
        {
           // return "RobotsPersonality: " + personality + " Persuasion: " + persuasion + " ConsecutivePlays: " + consecutivePlays + " OponentPlays: " + oponentPlays + " Persuasion: " + persuasion.ToString() + " Condition: " + condition;
            //return "" + personality + ";" + consecutivePlays + ";" + oponentPlays + ";" + persuasion.ToString() + ";" + condition;
            return "" + personality + ";" + consecutivePlays + ";" + oponentPlays + ";" + totalDominant + ";" + totalMeek + ";" + pitch + ";" + persuasion.ToString() + ";" + condition;

        }
    }
}
