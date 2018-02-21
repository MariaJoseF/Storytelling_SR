
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using SOP.Modules;

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

        public StoryForm storyWindow;


        public string UTTERANCE_DP_FILE;
        public string UTTERANCE_SCENES_FILE;
        // public const string OUTPUT_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"/Logs/StoryChoices/choices-";
        public int UserId;
        private Stopwatch stopwatch = new Stopwatch();


        private Dictionary<int, Scenes> storyNodes;
        private Dictionary<String, DecisionPoints> decisionPoints;
        public Client clientThalamus;

        public int currentStoryNodeId { get; set; }

        //private void writeLog(string log)
        //{
        //    File.AppendAllText(OUTPUT_FILE + this.UserId.ToString() + ".txt", log + "\r\n");
        //}

        public StoryHandler(Client thalamusClient, int UserId)
        {
            this.UserId = UserId;

            UTTERANCE_DP_FILE = thalamusClient.CPublisher.fileName + @"\Utterances\utterances_dp.csv";
            UTTERANCE_SCENES_FILE = thalamusClient.CPublisher.fileName + @"\Utterances\utterances_scenes.csv";

            //Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"/Logs/StoryChoices/");
            //From the original excel file the two tabs should be imported in Unicode csv in the name mentioned
            //below. The files should be at the current user's folder.
            List<DecisionPoints> fromcsv1 = File.ReadAllLines(UTTERANCE_DP_FILE).Skip(1).Select(v => DecisionPoints.FromCSV(v)).ToList();
            List<Scenes> fromcsv2 = File.ReadAllLines(UTTERANCE_SCENES_FILE).Skip(1).Select(v => Scenes.FromCSV(v)).ToList();
            decisionPoints = fromcsv1.ToDictionary(DecisionPoints => DecisionPoints.Initial);
            storyNodes = fromcsv2.ToDictionary(Scenes => Scenes.Id);
            currentStoryNodeId = 0; // initial scene ID since we skip the first line (id -1)

            this.clientThalamus = thalamusClient;



        }

        internal void NextScene(EMYS side, SelectionDP selectOption)
        {
            String decisionPoint = storyNodes[currentStoryNodeId].Before;

            clientThalamus.WriteJSON(String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now), currentStoryNodeId + ";" + selectOption, "StoryChoices", "choices-" + this.UserId.ToString() + ".txt");

            if (!decisionPoint.StartsWith("Final"))
                this.currentStoryNodeId = side == EMYS.left ? decisionPoints[decisionPoint].NextSt1 : decisionPoints[decisionPoint].NextSt2;

        }

        internal bool isEnding()
        {
            String decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoint.StartsWith("Final");
        }

        internal string GetInitialDP()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Initial;
        }

        internal string GetPrefDP()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Pref;
        }

        internal string GetLeftTag()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Tag1;
        }

        internal string GetLeftPref()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Pref1;
        }

        internal string GetLeftUtterance(Thalamus.BML.SpeechLanguages language)
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;

            return (language == Thalamus.BML.SpeechLanguages.English ? decisionPoints[decisionPoint].Textp1En : decisionPoints[decisionPoint].Textp1);
        }

        internal string GetRightTag()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Tag2;
        }

        internal string GetRightPref()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Pref2;
        }

        internal string GetRightUtterance(Thalamus.BML.SpeechLanguages language)
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;

            return (language == Thalamus.BML.SpeechLanguages.English ? decisionPoints[decisionPoint].Textp2En : decisionPoints[decisionPoint].Textp2);
        }

        internal String GetSceneUtterance(Thalamus.BML.SpeechLanguages language)
        {
            return (language == Thalamus.BML.SpeechLanguages.English ? storyNodes[currentStoryNodeId].TextEn : storyNodes[currentStoryNodeId].Text).Replace("\\r\\n", "" + Environment.NewLine).Replace("\\t", Environment.NewLine);
        }

        internal int GetSceneUtteranceId(Thalamus.BML.SpeechLanguages language)
        {
            return (language == Thalamus.BML.SpeechLanguages.English ? storyNodes[currentStoryNodeId].Id : storyNodes[currentStoryNodeId].Id);
        }

        internal string GetDecisionUtteranceId()
        {
            string decisionPoint = storyNodes[currentStoryNodeId].Before;
            return decisionPoints[decisionPoint].Id.ToString();
        }
        internal string GetDecisionUtteranceCategory()
        {
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