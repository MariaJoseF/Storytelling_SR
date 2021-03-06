﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Parser
{
    class Scenes
    {
        public int Id;
        public String Initial;
        public String Text;
        public String Before;
        public String TextEn;
        public String Location;

        public static Scenes FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Scenes scenes = new Scenes();
            scenes.Id = Convert.ToInt32(values[0]);
            scenes.Initial = Convert.ToString(values[1]);
            scenes.Text = Convert.ToString(values[2]);
            scenes.Before = Convert.ToString(values[3]);
            scenes.TextEn = Convert.ToString(values[4]);
            scenes.Location = Convert.ToString(values[5]);
            return scenes;
        }
    }

    class DecisionPoints
    {
        public int Id;
        public String Initial;
        public String Pref;
        public String Pref1;
        public String Pref2;
        public String Textp1;
        public String Textp2;
        public String Textp1En;
        public String Textp2En;
        public int NextSt1;
        public int NextSt2;
        public double Weight11;
        public double Weight12;
        public double Weight21;
        public double Weight22;

        public static DecisionPoints FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(';');
            DecisionPoints decisionPoints = new DecisionPoints();
            decisionPoints.Id = Convert.ToInt32(values[0]);
            decisionPoints.Initial = Convert.ToString(values[1]);
            decisionPoints.Pref = Convert.ToString(values[2]);
            decisionPoints.Pref1 = Convert.ToString(values[3]);
            decisionPoints.Pref2 = Convert.ToString(values[4]);
            decisionPoints.Textp1 = Convert.ToString(values[5]);
            decisionPoints.Textp2 = Convert.ToString(values[6]);
            decisionPoints.Textp1En = Convert.ToString(values[7]);
            decisionPoints.Textp2En = Convert.ToString(values[8]);
            decisionPoints.NextSt1 = Convert.ToInt32(values[11]);
            decisionPoints.NextSt2 = Convert.ToInt32(values[12]);

            decisionPoints.Weight11 = Convert.ToDouble(values[9]);
            decisionPoints.Weight12 = Convert.ToDouble(values[10]);
            decisionPoints.Weight21 = Convert.ToDouble(values[13]);
            decisionPoints.Weight22 = Convert.ToDouble(values[14]);
            //Console.WriteLine(values.ToString());
            return decisionPoints;
        }
    }

    class Phrases
    {
        public int Id;
        public String Favor;
        public String Against;
        public String FavorEN;
        public String AgainstEN;

        public static Phrases FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Phrases phrases = new Phrases();
            phrases.Id = Convert.ToInt32(values[0]);
            phrases.Favor = Convert.ToString(values[1]);
            phrases.Against = Convert.ToString(values[2]);
            phrases.FavorEN = Convert.ToString(values[3]);
            phrases.AgainstEN = Convert.ToString(values[3]);
            return phrases;
        }
    }
}
