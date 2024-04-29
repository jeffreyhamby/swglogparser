using System.Diagnostics;
using System.Net;

namespace SwgLogParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //strings found in lines to be ignored by the log parser
            string[] avoidStrings = new string[]
            {
                "heals"
                , "subsided"
                , "out of range"
                , "free shot"
                , "have been poisoned"
                , "sustained more poison"
                , "not a valid target"
                , "logging ON"
                , "log file size"
                , "group pickup point"
                , "performs" //buffs
                , "set a Rallypoint"
            };

            //strings found in lines based on offensive abilities
            string[] offenseStrings = new string[]
            {
                "hits"
                , "crits"
                , "glances"

            };

            //strings found in lines based on defensive abilities
            string[] defenseStrings = new string[]
            {
                "has caused"
            };

            StreamReader reader = File.OpenText(@"C:\Development\swglogparser\sampleLogs\5378010400_chatlog.txt");
            string line;
            List<string> lines = new List<string>();
            List<string> offenseLines = new List<string>();
            List<string> defenseLines = new List<string>();
            List<LogItem> logItems = new List<LogItem>();

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("[Combat]"))
                {
                    string timeStamp = line.Substring(10, 8);
                    Console.WriteLine(line.Substring(18));
                    if (!avoidStrings.Any(line.Contains)) 
                    {
                        lines.Add(line);

                        LogItem logItem = new LogItem();

                        //start parsing the line 
                        logItem.timestamp = line.Substring(10, 8);

                        if (offenseStrings.Any(line.Contains))
                        {
                            logItem.offensive = true;
                            logItem.restOfIt = line.Substring(18);
                            offenseLines.Add(line.Substring(18));
                        };

                        if (defenseStrings.Any(line.Contains))
                        {
                            logItem.defensive = true;
                            logItem.restOfIt = line.Substring(18);
                            defenseLines.Add(line.Substring(18));
                        };


                        //trap missing lines to find bugs based on the kludgy string manipulation
                        if (!defenseStrings.Any(line.Contains) && !offenseStrings.Any(line.Contains))
                        {
                            logItem.notSure = true;
                            logItem.restOfIt = line.Substring(18);
                            
                        };



                        logItems.Add(logItem);

                    }



                }
            }
        }
    }

    public class LogItem
    {
        public string timestamp { get; set; }
        public bool offensive { get; set; }
        public bool defensive { get; set; }   
        public bool notSure { get; set; }
        public string restOfIt { get; set; }


    }
}
