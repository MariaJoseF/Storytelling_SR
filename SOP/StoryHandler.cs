﻿
using Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace StoryOfPersonality
{
    public enum Volume
    {
        low = 0,
        medium = 1,
        high = 2
    }

    public enum Rate
    {
        low = 0,
        medium = 1,
        high = 2
    }

    public class StoryHandler
    {
        public const string UTTERANCE_DP_FILE = "utterances_dp.csv";
        public const string UTTERANCE_SCENES_FILE = "utterances_scenes.csv";
        public const string OUTPUT_FILE = "story-choices/choices-";
        public int UserId;
        Stopwatch stopwatch = new Stopwatch();

        private Dictionary<int, Scenes> storyNodes;
        private Dictionary<String, DecisionPoints> decisionPoints;

        public int currentStoryNodeId { get; set; }

        private void writeLog(string log)
        {
            File.AppendAllText(OUTPUT_FILE + this.UserId.ToString() + ".txt", log + "\r\n");
        }

        public StoryHandler(int UserId)
        {
            this.UserId = UserId;
            Directory.CreateDirectory(OUTPUT_FILE.Substring(0, OUTPUT_FILE.IndexOf('/')));
            //From the original excel file the two tabs should be imported in Unicode csv in the name mentioned
            //below. The files should be at the current user's folder.
            List<DecisionPoints> fromcsv1 = File.ReadAllLines(UTTERANCE_DP_FILE).Skip(1).Select(v => DecisionPoints.FromCSV(v)).ToList();
            List<Scenes> fromcsv2 = File.ReadAllLines(UTTERANCE_SCENES_FILE).Skip(1).Select(v => Scenes.FromCSV(v)).ToList();
            decisionPoints = fromcsv1.ToDictionary(DecisionPoints => DecisionPoints.Initial);
            storyNodes = fromcsv2.ToDictionary(Scenes => Scenes.Id);
            currentStoryNodeId = 0; // initial scene ID since we skip the first line (id -1)
        }

        internal void NextScene(EMY side, long elapsedms)
        {
            String decisionPoint = storyNodes[currentStoryNodeId].Before;
            writeLog(currentStoryNodeId + ";" + side + ";" + elapsedms.ToString());
            if (! decisionPoint.StartsWith("Final"))
                this.currentStoryNodeId =  side == EMY.left ? decisionPoints[decisionPoint].NextSt1 : decisionPoints[decisionPoint].NextSt2;
        }

        internal bool isEnding()
        {
            String decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoint.StartsWith("Final");
        }

        internal string GetLeftTag()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Tag1;
        }

        internal string GetRightTag()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Tag2;
        }

        internal String GetSceneUtterance(Thalamus.BML.SpeechLanguages language)
        {
            return (language == Thalamus.BML.SpeechLanguages.English ? storyNodes[currentStoryNodeId].TextEn : storyNodes[currentStoryNodeId].Text).Replace("\\r\\n", ""+ Environment.NewLine).Replace("\\t", Environment.NewLine);
        }

        internal string GetDecisionUtteranceId()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Id.ToString();
        }
        internal string GetDecisionUtteranceCategory() {
            return storyNodes[currentStoryNodeId].Before;
        }

        internal String GetSceneLocation()
        {
            return this.storyNodes[currentStoryNodeId].Location;
        }

        private String encloseWithVolumeTag(String utterance, Volume volume)
        {
            String value = "medium"; // default
            switch (volume)
            {
                case Volume.low:
                    value = "x-soft";
                    break;
                case Volume.medium:
                    value = "medium";
                    break;
                case Volume.high:
                    value = "x-loud";
                    break;
            }
            return "<prosody volume=" + value + utterance + "</prosody>";
        }
        private String encloseWithRateTag(String utterance, Rate rate)
        {
            String value = "1"; // default
            switch (rate)
            {
                case Rate.low:
                    value = "slow";
                    break;
                case Rate.medium:
                    value = "1";
                    break;
                case Rate.high:
                    value = "fast";
                    break;
            }
            return "<prosody rate=" + value + utterance + "</prosody>";
        }
    }
}