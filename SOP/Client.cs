using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using EmoteCommonMessages;
using System.Timers;
using System.IO;

namespace StoryOfPersonality
{
    public interface IClient : Thalamus.BML.ISpeakEvents { }

    public interface IClientPublisher : IThalamusPublisher, IFMLSpeech, Thalamus.BML.ISpeakActions, Thalamus.BML.ISpeakControlActions, Thalamus.ILibraryActions, Thalamus.BML.IPostureActions
    {
    }

    public class Client : ThalamusClient, IClient
    {
        public class ClientPublisher : IClientPublisher
        {
            dynamic publisher;
            //gets the project folder not the bin/Debug folder like System.Environment.CurrentDirectory;
            public string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName; 

            public ClientPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ClientMessage(string msg)
            {
                publisher.ClientMessage(msg);
            }

            #region Ispeak
            public void Speak(string id, string text)
            {
                publisher.Speak(id, text);
            }

            public void SpeakBookmarks(string id, string[] text, string[] bookmarks)
            {
                publisher.SpeakBookmarks(id, text, bookmarks);
            }

            public void SpeakStop()
            {
                publisher.SpeakStop();
            }
            #endregion

            #region setLanguage
            public void SetLanguage(Thalamus.BML.SpeechLanguages lang)
            {
                publisher.SetLanguage(lang);
            }
            #endregion

            #region IFML
            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }
            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }
            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }
            public void PerformUtteranceWithTags(string id, string utterance, string[] tagNames, string[] tagValues)
            {
                this.publisher.PerformUtteranceWithTags(id, utterance, tagNames, tagValues);
            }

            #endregion

            #region Library
            public void ChangeLibrary(string file)
            {
                publisher.ChangeLibrary(file);
            }
            #endregion

            #region posture
            public void SetPosture(string id, string posture, double percent = 1, double decay = 1)
            {
                publisher.SetPosture(id, posture, percent, decay);
            }

            public void ResetPose()
            {
                publisher.ResetPose();
            }
            #endregion
        }
        public ClientPublisher CPublisher;
        public StoryForm storyWindow;

        public Thalamus.BML.SpeechLanguages Language { get; private set; }
        public bool ActivateAudio { get => activateAudio; set => activateAudio = value; }

        private StartAdventure.OptionSide Side;
        private EventHandler endUtteranceEvent;
        private bool activateAudio;

        public Client(StartAdventure.OptionSide side, Thalamus.BML.SpeechLanguages language, string character)
            : base(character, character)

        {
            SetPublisher<IClientPublisher>();
            CPublisher = new ClientPublisher(Publisher);
            CPublisher.SetLanguage(language);
            Language = language;
            Side = side;

            //this.storyWindow = window;
        }
        public void StoryWindow (StoryForm window)
        {
            this.storyWindow = window;
        }

        public void StartUtteranceFromLibrary(string id, string category, string[] tags, EventHandler endUtteranceEvent = null)
        {
            string[] values = new string[tags.Length];
            //for (int i = 0; i < tags.Length; i++)
            //{
            //    string value = "<animate(" + tags[i] + ")>";
            //    values[i] = value;
            //    Console.WriteLine(value);
            //}
            CPublisher.PerformUtteranceFromLibrary(id, category, "sub", tags, values);
            this.endUtteranceEvent = endUtteranceEvent;
        }


        public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues, EventHandler endUtteranceEvent = null)
        {
            CPublisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            this.endUtteranceEvent = endUtteranceEvent;
        }

        public void StartUtterance(string id, string utteranceText, EventHandler endUtteranceEvent = null)
        {
            Console.WriteLine("UTTERANCE: " + utteranceText);
            CPublisher.PerformUtterance(id, utteranceText, "");

            this.endUtteranceEvent = endUtteranceEvent;
        }

        #region EMYSSpeak

        public void SpeakStarted(string id)
        {
            Console.WriteLine("--------------------------------------- EMYS Started The ID:" + id);
        }

        public void SpeakFinished(string id)
        {
            Console.WriteLine("--------------------------------------- EMYS Finished The ID:" + id);
            //Console.WriteLine("--------------------------------------- CurrentUtterance:" + currentUtterance);
            //Console.WriteLine("--------------------------------------- Dialog:");
            //    NextUtterance();

            if (storyWindow.playSceneAnger == 1)
            {
                //storyWindow.playStoryScene(storyWindow.StoryHandler.GetSceneUtteranceId(this.Language), this.Language);
                activateAudio = true;
                storyWindow.playSceneAnger = 0;
            }

            //if (storyWindow.PlayedLeftButton)
            //{
            //    storyWindow.Invoke((Action)(() =>
            //    {//this refer to form in WPF application 
            //        //storyWindow.EnableBTS("R");
            //        storyWindow.PlayRight_Robot();
            //    }));
            //}
            //else if (storyWindow.PlayedRightButton)
            //{
            //    storyWindow.Invoke((Action)(() =>
            //    {//this refer to form in WPF application 
            //        //storyWindow.EnableBTS("L");
            //        storyWindow.PlayLeft_Robot();
            //    }));
            //}

        }
        #endregion

        #region dialogs
        public void UtteranceStarted(string id)
        {
            Console.WriteLine("Utt started: " + id);
        }

        public void UtteranceFinished(string id)
        {
            //Console.WriteLine("------------------ HERE UTTFINISHED : " + id + currentUtterance + dialogsCat.start.ToString() + " " + dialogsSubCat.small_talk1.ToString());
        }
        #endregion


        public void WriteJSON(string timestamp, string info, string aux_path, string name_file)
        {

        string filePath = CPublisher.fileName + @"\Logs\" + aux_path + @"\";
            string filename = filePath + name_file + ".txt";

            //Console.WriteLine(filePath);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    //Console.WriteLine(filePath);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(filename, true))
            {
                file.WriteLine(timestamp + " " + info);
            }

        }


    }
}
