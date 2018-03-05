using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryOfPersonality
{
    public class Personality
    {
        private StoryForm storyForm;

        // e/i - s/n - j/p - t/f - l/r
        private int[] sumOfDecisions = new int[10];

        public Personality(StoryForm storyForm)
        {
            this.storyForm = storyForm;
        }

        public void BuildPersonality(string selected)
        {
            Console.WriteLine("=== Decision Pref: " + selected);
            switch (selected)
            {
                case "e":
                    sumOfDecisions[0]++;
                    break;
                case "i":
                    sumOfDecisions[1]++;
                    break;
                case "s":
                    sumOfDecisions[2]++;
                    break;
                case "n":
                    sumOfDecisions[3]++;
                    break;
                case "t":
                    sumOfDecisions[4]++;
                    break;
                case "f":
                    sumOfDecisions[5]++;
                    break;
                case "j":
                    sumOfDecisions[6]++;
                    break;
                case "p":
                    sumOfDecisions[7]++;
                    break;
                case "l":
                    sumOfDecisions[8]++;
                    break;
                case "r":
                    sumOfDecisions[9]++;
                    break;
            }
        }

        public string RecordPathPersonality(string dp, string pref, string prefSelected, string robotInfluenced, string conditionPersuasion)
        {
            string txt = dp + ";" + pref + ";" + prefSelected + ";" + robotInfluenced + ";" + conditionPersuasion;
            //Console.WriteLine("=== Decisions: " + dp + " - " + pref + " - " + prefSelected + " - " + robotInfluenced + " - " + conditionPersuasion);

            string arraySum = "";
            for (int i = 0; i < sumOfDecisions.Length; i++)
            {
                arraySum += sumOfDecisions[i] + " | ";
            }

            arraySum = "E | I | S | N | T | F | J | P | L | R | \r\n" + arraySum + "\r\n" +
                       "============================= \r\n";
            Console.WriteLine(arraySum);
            return txt;
        }

        public string DefineMBTIPersonality()
        {
            string MBTIPersonality;
            if (sumOfDecisions[0] > sumOfDecisions[1])
                MBTIPersonality = "E ";
            else if (sumOfDecisions[0] < sumOfDecisions[1])
                MBTIPersonality = "I ";
            else MBTIPersonality = "E/I ";

            if (sumOfDecisions[2] > sumOfDecisions[3])
                MBTIPersonality += "S ";
            else if (sumOfDecisions[2] < sumOfDecisions[3])
                MBTIPersonality += "N ";
            else MBTIPersonality += "S/N ";

            if (sumOfDecisions[4] > sumOfDecisions[5])
                MBTIPersonality += "T ";
            else if (sumOfDecisions[4] < sumOfDecisions[5])
                MBTIPersonality += "F ";
            else MBTIPersonality += "T/F ";

            if (sumOfDecisions[6] > sumOfDecisions[7])
                MBTIPersonality += "J ";
            else if (sumOfDecisions[6] < sumOfDecisions[7])
                MBTIPersonality += "P ";
            else MBTIPersonality += "J/P ";

            string txt = "MBTI Personality: " + MBTIPersonality + "\r\n" +
                         "============================= \r\n";
            string arraySum = "";
            for (int i = 0; i < sumOfDecisions.Length; i++)
            {
                arraySum += sumOfDecisions[i] + " | ";
            }

            arraySum = "E | I | S | N | T | F | J | P | L | R | \r\n" + arraySum + "\r\n" +
                       "============================= \r\n";

            return txt + arraySum;
        }
    }
}
