using StoryOfPersonality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SOP.Modules.Prosody;

namespace SOP.Modules
{
    public class Robot
    {
        private RobotsPersonality personality;
        private Persuasion persuasion;
        private int consecutivePlays;
        private int oponentPlays;
        private string pitch;
        private int totalDominant;
        private int totalMeek;
        private string decisionPoint;
        private string preferencePair;
        private string prefSelectedIntention;
        private string prefSelectedFinal;

        private int timesPhrases = 0;
        private List<string> idPhrasesUsed = new List<string>();
        private List<string> phraseUsed = new List<string>();

        private RobotsPersuasion persuasionCondition;//persuasionCondition mesmo
        private RobotsPosture posture;
        private bool enable;
        private RobotSide side;

        //public Robot(RobotsPersonality personality, Persuasion persuasion)
        //{
        //    this.personality = personality;
        //    this.consecutivePlays = 0;
        //    this.oponentPlays = 0;
        //    this.persuasion = persuasion;
        //    this.persuasionCondition = RobotsPersuasion.none;
        //    this.totalDominant = 0;
        //    this.totalMeek = 0;
        //    this.enable = false;
        //    this.side = RobotSide.none;
        //    this.decisionPoint = "-";
        //    this.preferencePair = "-";
        //    this.prefSelectedIntention = "-";
        //     this.prefSelectedFinal= "-";

        //    if (personality.Equals(RobotsPersonality.dominant))
        //    {
        //        this.pitch = "x-high";
        //        this.posture = RobotsPosture.pride;
        //    }
        //    else
        //    {
        //        this.pitch = "x-low";
        //        this.posture = RobotsPosture.contempt;
        //    }
        //}

        public Robot(RobotsPersonality personality)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.persuasionCondition = RobotsPersuasion.none;
            this.totalDominant = 0;
            this.totalMeek = 0;
            this.enable = false;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";

            if (personality.Equals(RobotsPersonality.dominant))
            {
                this.pitch = "x-high";
                this.persuasion.Prosody = new Prosody("medium", "x-loud");
            }
            else
            {
                this.pitch = "x-low";
                this.persuasion.Prosody = new Prosody("medium", "soft");
            }
        }

        public Robot(RobotsPersonality personality, RobotSide side, Thalamus.BML.SpeechLanguages language)
        {
            this.personality = personality;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();

            if (language.Equals(RobotsLanguage.EN))
            {
                this.persuasion.Prosody = new Prosody(RobotsLanguage.EN);
            }
            else
            {
                this.persuasion.Prosody = new Prosody(RobotsLanguage.PT);
            }

            this.persuasionCondition = RobotsPersuasion.none;
            this.totalDominant = 0;
            this.totalMeek = 0;
            this.enable = false;
            this.side = side;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";
            if (personality.Equals(RobotsPersonality.dominant))
            {
                this.pitch = "x-high";
                this.persuasion.Prosody = new Prosody("medium", "x-loud");
            }
            else
            {
                this.pitch = "x-low";
                this.persuasion.Prosody = new Prosody("medium", "soft");
            }
        }

        public Robot()
        {
            this.personality = RobotsPersonality.none;
            this.consecutivePlays = 0;
            this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.persuasionCondition = RobotsPersuasion.none;
            this.totalDominant = 0;
            this.totalMeek = 0;
            this.enable = false;
            this.side = RobotSide.none;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";
            this.pitch = "";
        }

        public RobotsPersonality Personality { get => personality; set => personality = value; }
        public Persuasion Persuasion { get => persuasion; set => persuasion = value; }
        public int ConsecutivePlays { get => consecutivePlays; set => consecutivePlays = value; }
        public int OponentPlays { get => oponentPlays; set => oponentPlays = value; }
        public string Pitch { get => pitch; set => pitch = value; }
        public int TotalDominant { get => totalDominant; set => totalDominant = value; }
        public int TotalMeek { get => totalMeek; set => totalMeek = value; }
        public RobotsPosture Posture { get => posture; set => posture = value; }
        public string DecisionPoint { get => decisionPoint; set => decisionPoint = value; }
        public string PreferencePair { get => preferencePair; set => preferencePair = value; }
        public string PrefSelectedIntention { get => prefSelectedIntention; set => prefSelectedIntention = value; }
        public RobotsPersuasion PersuasionCondition { get => persuasionCondition; set => persuasionCondition = value; }
        public bool Enable { get => enable; set => enable = value; }
        public RobotSide Side { get => side; set => side = value; }
        public string PrefSelectedFinal { get => prefSelectedFinal; set => prefSelectedFinal = value; }

        public int TimesPhrases { get => timesPhrases; set => timesPhrases = value; }
        public List<string> PhraseUsed { get => phraseUsed; set => phraseUsed = value; }
        public List<string> IdPhrasesUsed { get => idPhrasesUsed; set => idPhrasesUsed = value; }

        public enum RobotsPersonality
        {
            none = -1,
            meek = 0,
            dominant = 1
        }

        public enum RobotsPersuasion
        {
            none = -1,
            favour = 0,
            againts = 1
        }

        public enum RobotSide
        {
            none = -1,
            left = 0,
            right = 1
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
            return "" + personality + ";" + consecutivePlays + ";" + oponentPlays + ";" + totalDominant + ";" + totalMeek + ";" + pitch + ";" + persuasion.ToString() + ";" + decisionPoint + ";" + preferencePair + ";" + prefSelectedIntention + ";" + prefSelectedFinal + ";" + persuasionCondition;

        }
    }
}
