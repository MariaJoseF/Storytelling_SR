using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace StoryOfPersonality
{
    public enum NameOfFiles
    {
        StoryChoices = 0,
        ThalamusClientLeft = 1,
        ThalamusClientRight = 2,
        PlayerConfig = 3
    }

    public partial class Logs
    {
        public StoryHandler storyHandler;

        public Logs(StoryHandler storyHandler)
        {
            this.storyHandler = storyHandler;
        }

        // File
        // 0 - storyChoices
        // 1 - ThalamusLeft
        // 2 - ThalamusRight
        // 3 - Player Configuration
        public void RecordLog(NameOfFiles idFile, string textRecord)
        {
            string aux_path = "";
            if (idFile == NameOfFiles.StoryChoices) aux_path = "StoryChoices";
            else if (idFile == NameOfFiles.ThalamusClientLeft) aux_path = "ThalamusClientLeft";
            else if (idFile == NameOfFiles.ThalamusClientRight) aux_path = "ThalamusClientRight";
            else if (idFile == NameOfFiles.PlayerConfig) aux_path = "PlayerConfig";

            string filePath = storyHandler.clientThalamus.CPublisher.fileName + @"\Logs\" + aux_path + @"\";
            string filename = filePath + storyHandler.UserId.ToString() + ".txt";

            Console.WriteLine(filePath);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine(filePath);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(filename, true))
            {

                file.WriteLine(textRecord);
            }
        }
    }
}
