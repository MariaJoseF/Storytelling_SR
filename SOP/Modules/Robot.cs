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
        //private int consecutivePlays;
        //private int oponentPlays;
        private string pitch;
        private int totalFavour;
        private int totalAgainst;
        private string decisionPoint;
        private string preferencePair;
        private string prefSelectedIntention;
        private string prefSelectedFinal;

        private int timesPhrases = 0;
        private int idPhrasesUsed;
        private string phraseUsed = "Error|none";
        private string animationUsed;
        //private string gazeUsed;

        private RobotsPersuasion persuasionCondition;//persuasionCondition mesmo
        private RobotsPosture posture;
        private RobotSide side;

        private RobotCongruent congruentIntention;


        public Robot(RobotsPersonality personality, RobotsPersuasion against)
        {
            this.personality = personality;
            //this.consecutivePlays = 0;
            //this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.persuasionCondition = against;
            this.totalFavour = 0;
            this.totalAgainst = 0;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";

            //if (personality.Equals(RobotsPersonality.dominant))
            //{
            //    this.pitch = "x-low";
            //    this.persuasion.Prosody = new Prosody("+20%", "x-loud");
            //}
            //else if (personality.Equals(RobotsPersonality.meek))
            //{
            //    this.pitch = "x-high";
            //    this.persuasion.Prosody = new Prosody("medium", "loud");
            //}
            //else
            //{
                this.pitch = "default";
                this.persuasion.Prosody = new Prosody("medium", "default");
            //}
            this.congruentIntention = RobotCongruent.none;
        }

        public Robot(RobotsPersonality personality, RobotSide side, Thalamus.BML.SpeechLanguages language)
        {
            this.personality = personality;
            //this.consecutivePlays = 0;
            //this.oponentPlays = 0;
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
            this.totalFavour = 0;
            this.totalAgainst = 0;
            this.side = side;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";

            //if (personality.Equals(RobotsPersonality.dominant))
            //{
            //    this.pitch = "x-low";
            //    this.persuasion.Prosody = new Prosody("+20%", "x-loud");
            //}
            //else if(personality.Equals(RobotsPersonality.meek))
            //{
            //    this.pitch = "x-high";
            //    this.persuasion.Prosody = new Prosody("medium", "loud");
            //}
            //else
            //{
                this.pitch = "default";
                this.persuasion.Prosody = new Prosody("medium", "default");
            //}
            this.congruentIntention = RobotCongruent.none;
        }

        public Robot()
        {
            this.personality = RobotsPersonality.none;
            //this.consecutivePlays = 0;
            //this.oponentPlays = 0;
            this.persuasion = new Persuasion();
            this.persuasionCondition = RobotsPersuasion.none;
            this.totalFavour = 0;
            this.totalAgainst = 0;
            this.side = RobotSide.none;
            this.decisionPoint = "-";
            this.preferencePair = "-";
            this.prefSelectedIntention = "-";
            this.prefSelectedFinal = "-";
            this.pitch = "";
            this.congruentIntention = RobotCongruent.none;
        }

        public RobotsPersonality Personality { get => personality; set => personality = value; }
        public Persuasion Persuasion { get => persuasion; set => persuasion = value; }
        //public int ConsecutivePlays { get => consecutivePlays; set => consecutivePlays = value; }
        //public int OponentPlays { get => oponentPlays; set => oponentPlays = value; }
        public string Pitch { get => pitch; set => pitch = value; }
        public int TotalFavour { get => totalFavour; set => totalFavour = value; }
        public int TotalAgainst { get => totalAgainst; set => totalAgainst = value; }
        public RobotsPosture Posture { get => posture; set => posture = value; }
        public string DecisionPoint { get => decisionPoint; set => decisionPoint = value; }
        public string PreferencePair { get => preferencePair; set => preferencePair = value; }
        public string PrefSelectedIntention { get => prefSelectedIntention; set => prefSelectedIntention = value; }
        public RobotsPersuasion PersuasionCondition { get => persuasionCondition; set => persuasionCondition = value; }
        public RobotSide Side { get => side; set => side = value; }
        public string PrefSelectedFinal { get => prefSelectedFinal; set => prefSelectedFinal = value; }

        public int TimesPhrases { get => timesPhrases; set => timesPhrases = value; }
        public string PhraseUsed { get => phraseUsed; set => phraseUsed = value; }
        public int IdPhrasesUsed { get => idPhrasesUsed; set => idPhrasesUsed = value; }
        public RobotCongruent CongruentIntention { get => congruentIntention; set => congruentIntention = value; }
        public string AnimationUsed { get => animationUsed; set => animationUsed = value; }
       // public string GazeUsed { get => gazeUsed; set => gazeUsed = value; }

        public enum RobotsPersonality
        {
            none = -1,
            meekNeutral = 0,
            dominantNeutral = 1,
        }

        public enum RobotsPersuasion
        {
            none = -1,
            Positive = 0,
            Against = 1
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
            //shame = 3,
            hope = 4,
            joy = 5,
            //pride = 6,
            satisfaction = 7,
            gratitude = 8,
            neutral = 9
        }

        public enum RobotCongruent
        {
            none = -1,
            congruent = 0,
            non_congruent = 1
        }
        public override string ToString()
        {
            // return "RobotsPersonality: " + personality + " Persuasion: " + persuasion + " ConsecutivePlays: " + consecutivePlays + " OponentPlays: " + oponentPlays + " Persuasion: " + persuasion.ToString() + " Condition: " + condition;
            //return "" + personality + ";" + consecutivePlays + ";" + oponentPlays + ";" + persuasion.ToString() + ";" + condition;
            //return ";" + personality + ";" + posture + ";" + consecutivePlays + ";" + oponentPlays + ";" + totalFavour + ";" + totalNegative + ";" + pitch + ";" + persuasion.ToString() + ";" + decisionPoint + ";" + preferencePair + ";" + prefSelectedIntention + ";" + prefSelectedFinal + ";" + congruentIntention + ";" + timesPhrases + ";" + IdPhrasesUsed + ";" + phraseUsed + ";" + animationUsed + ";" + persuasionCondition;
            return ";" + personality + ";" + posture + ";" + pitch + ";" + persuasion.ToString() + ";" + decisionPoint + ";" + preferencePair + ";" + prefSelectedIntention + ";" + prefSelectedFinal + ";" + congruentIntention + ";" + IdPhrasesUsed + ";" + phraseUsed + ";" + animationUsed + ";" + persuasionCondition;
        }
    }
}
