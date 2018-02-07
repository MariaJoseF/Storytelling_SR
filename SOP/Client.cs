﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using EmoteCommonMessages;
using System.Timers;

namespace StoryOfPersonality
{
    public enum EMY
    {
        left = 0,
        right = 1
    }
   
    public interface IClient : Thalamus.BML.ISpeakEvents { }

    public interface IClientPublisher : IThalamusPublisher, IFMLSpeech, Thalamus.BML.ISpeakActions, Thalamus.BML.ISpeakControlActions, Thalamus.ILibraryActions
    {
      //  new void SetLanguage(Thalamus.BML.SpeechLanguages languages);
    }

    public class Client : ThalamusClient, IClient
    {
        public class ClientPublisher : IClientPublisher
        {
            dynamic publisher;
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
        }
        public ClientPublisher CPublisher;
        public StoryForm storyWindow;

        public Thalamus.BML.SpeechLanguages Language { get; private set; } 
        private EMY Side;
        private EventHandler endUtteranceEvent;

        public Client(StoryForm window, EMY side, Thalamus.BML.SpeechLanguages language, string character )
            : base(character, character)

        {
            SetPublisher<IClientPublisher>();
            CPublisher = new ClientPublisher(Publisher);
            CPublisher.SetLanguage(language);
            Language = language;
            Side = side;

            this.storyWindow = window;
        }
        public void StartUtteranceFromLibrary(string id, string category, string[] tags, EventHandler endUtteranceEvent = null)
        {
            string[] values = new string[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                string value = "<animate(" + tags[i] + ")>";
                values[i] = value;
                Console.WriteLine(value);
            }
            CPublisher.PerformUtteranceFromLibrary(id, category, "sub", tags, values);
            this.endUtteranceEvent = endUtteranceEvent;
        }
        public void StartUtterance(string id, string utteranceText, EventHandler endUtteranceEvent = null)
        {
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
            Console.WriteLine("--------------------------------------- Dialog:" );
            //    NextUtterance();

            //não percebi o que faz

            //   endUtteranceEvent(this, EventArgs.Empty); // fire the custom event
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

    }
}
