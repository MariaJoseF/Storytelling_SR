using SOP.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SOP.Modules.Robot;
//using static StoryOfPersonality.StoryForm;

namespace StoryOfPersonality
{
    public class Persuasion
    {
        private string gaze;
        private int time;
        private string animation;
        private Prosody prosody;

        public Persuasion()
        {
            this.gaze = "";
            this.time = -1;
            this.animation = "";
            this.prosody = new Prosody();
        }

        public Persuasion(string gaze, int time, string animation)
        {
            this.gaze = gaze;
            this.time = time;
            this.animation = animation;
            this.prosody = new Prosody();
        }

        public Persuasion(string gaze, int time, string animation, Prosody prosody)
        {
            this.gaze = gaze;
            this.time = time;
            this.animation = animation;
            this.prosody = prosody;
        }

        public void LoadScenePersuasion(Robot robot)
        {
            Persuasion _persuasion = new Persuasion();
            Prosody _prosody = new Prosody();
            _persuasion.Prosody = _prosody;

            switch (robot.Condition)
            {
                case 0:
                    //No prosody just Gaze
                    LoadGazeTime(robot, _persuasion);
                    break;
                case 1:


                    //
                    //  acontece para todas as condições???? ou é outra condição?????
                    //



                    if (robot.Personality.Equals(Robot.RobotsPersonality.dominant))
                    {
                        //  <animation> only for the dominant robot
                        //annimation intensity is done according to repetition done by the robot that is performing it
                        //when intensity achieves value 4 the next keeps 4

                        if (robot.OponentPlays > 0) // Assertive robot was the last option selected
                        {
                            switch (robot.OponentPlays)
                            {
                                case 1:
                                    _persuasion.animation = "anger1";
                                    break;
                                case 2:
                                    _persuasion.animation = "anger3";
                                    break;
                                case 3:
                                    _persuasion.animation = "anger5";
                                    break;
                                default:
                                    if (robot.OponentPlays > 3)
                                    {
                                        _persuasion.animation = "anger5";
                                    }
                                    break;
                            }
                        }
                        else if (robot.ConsecutivePlays > 0)// Dominant robot was the last option selected
                        {
                            switch (robot.ConsecutivePlays)
                            {
                                case 1:
                                    _persuasion.animation = "joy1";
                                    break;
                                case 2:
                                    _persuasion.animation = "joy3";
                                    break;
                                case 3:
                                    _persuasion.animation = "joy5";
                                    break;
                                default:
                                    if (robot.OponentPlays > 3)
                                    {
                                        _persuasion.animation = "joy5";
                                    }
                                    break;
                            }
                        }
                    }



                    //
                    // 
                    //



                    break;
                case 2:

                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot that is performing it
                    //when intensity achieves value 4 the next one starts on 1


                    switch (robot.ConsecutivePlays % 4)
                    {
                        case 1://Intensity = 1
                            _prosody.Intensity = 1;
                            break;
                        case 2://Intensity = 2
                            _prosody.Intensity = 2;
                            break;
                        case 3://Intensity = 3
                            _prosody.Intensity = 3;
                            break;
                        case 0://Intensity = 4
                            _prosody.Intensity = 4;
                            break;
                    }
                    LoadGazeTime(robot, _persuasion);
                    _prosody.Lvl = 2;
                    break;
                case 3:

                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
                    //when intensity achieves value 4 the next keeps the 4

                    switch (robot.ConsecutivePlays)
                    {
                        case 1://Intensity = 1
                            _prosody.Intensity = 1;
                            break;
                        case 2://Intensity = 2
                            _prosody.Intensity = 2;
                            break;
                        case 3://Intensity = 3
                            _prosody.Intensity = 3;
                            break;
                        case 4://Intensity = 4
                            _prosody.Intensity = 4;
                            break;
                        default://Intensity = 4
                            if (robot.ConsecutivePlays > 4)
                            {
                                _prosody.Intensity = 4;
                            }
                            break;
                    }

                    LoadGazeTime(robot, _persuasion);
                    _prosody.Lvl = 3;
                    break;
                case 4:

                    //  <utterance> <gaze> <prosody> <utterance prosody> </prosody>
                    //prosody intensity is done according to repetition done by the robot opposite to the one that is performing it
                    //prosody lvl 3 is stronger than lvl2
                    //when intensity achieves value 4 the next keeps the 4

                    switch (robot.ConsecutivePlays)
                    {
                        case 1://Intensity = 1
                            _prosody.Intensity = 1;
                            break;
                        case 2://Intensity = 2
                            _prosody.Intensity = 2;
                            break;
                        case 3://Intensity = 3
                            _prosody.Intensity = 3;
                            break;
                        case 4://Intensity = 4
                            _prosody.Intensity = 4;
                            break;
                        default://Intensity = 4
                            if (robot.ConsecutivePlays > 4)
                            {
                                _prosody.Intensity = 4;
                            }
                            break;
                    }

                    LoadGazeTime(robot, _persuasion);
                    _prosody.Lvl = 4;
                    break;
            }

            robot.Persuasion = _persuasion;

        }

        private void LoadGazeTime(Robot robot, Persuasion _persuasion)
        {
            //gaze intensity = 1 gaze to Person and then Button 
            //gaze intensity = 2 gaze to Person then Button and again to Person
            //gaze intensity = 2 gaze to Person then Button again to Person and again to Button

            //  <utterance> <gaze>
            switch (robot.ConsecutivePlays % 3)
            {
                case 1://Intensity = 1
                    _persuasion.gaze = "Person-Button";
                    break;
                case 2://Intensity = 2
                    _persuasion.gaze = "Person-Button-Person";
                    break;
                case 0://Intensity = 3
                    _persuasion.gaze = "Person-Button-Person-Button";
                    break;
            }
            _persuasion.Time = GetTimeRobotFeature();
        }


        //if return 1 robot will perform more gaze according to his personality (domintant will look more to person, assertive will look less to person)
        //if return 0 robot will perform random gaze
        private int GetTimeRobotFeature()
        {
            Random random = new Random();

            if (random.NextDouble() <= 0.8)
                return 1;

            return 0;
        }

        public string Gaze { get => gaze; set => gaze = value; }
        public int Time { get => time; set => time = value; }
        public string Animation { get => animation; set => animation = value; }
        internal Prosody Prosody { get => prosody; set => prosody = value; }

        public override string ToString()
        {
            //return "Gaze: " + gaze + " Time: " + time + " Animation: " + animation + " Prosody: " + prosody.ToString();
            return "" + gaze + ";" + time + ";" + animation + ";" + prosody.ToString();

        }

    }
}
