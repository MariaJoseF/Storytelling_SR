using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryOfPersonality
{
    public class Config
    {
        // 0 = Dominant robot equal participant PT, 1 = Dominant robot diff participant PT
        private int persuasionType = 1;

        // It is possible to define the intensity of the persuasion used, and the highest intensity has the features before. E.g. the intensity 4 execute all the intensities before.
        // 0 = nothing
        // 1 = Facial expression and head movements of negation or approval regarding the last decision made. (BOTH ROBOTS)
        // 2 = Sounds tick, as psiu, ups, oi, hi, aham, arran etc. (ONLY DOMINANT OR MAYBE BOTH)
        // 3 = The robot is more emphatic (strong phrases), saying that the player should follow his lead to win. But this occurs after few negations. (ONLY DOMINANT)
        // 4 = Lying, when the chose made is different, the robot lie, saying that the player will lose the game. Only when the player do not follow 3 tips in a row. (ONLY DOMINANT)
        // No implemented 5 = Asking confirmation for decision made, as Are you sure of this?, Really? ps. Just in those cases where the participant chose the decision different of the robot. (ONLY DOMINANT)
        private int persuasionIntensity = 0;

        // Only the dominand robot perform this.
        // 0 = no small talk, 1 = presentation and light small talk, 2 = presentation with more interaction
        private int rapportLevel = 0;

        public int getPersuasionType()
        {
            return persuasionType;
        }
        public int getPersuasionIntensity()
        {
            return persuasionIntensity;
        }
        public int getRapportLevel()
        {
            return rapportLevel;
        }
    }
}
