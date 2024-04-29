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
                , "healing:["
            };

            //strings found in lines based on offensive abilities
            string[] offenseStrings = new string[]
            {
                "hits"
                , "crits"
                , "glances"
                , "but they are immune"

            };

            //strings found in lines based on defensive abilities
            string[] defenseStrings = new string[]
            {
                "has caused"
                , "dodge"
                , "resisted"
                , "strikes through"
                , "misses"
            };

            StreamReader reader = File.OpenText(@"C:\Development\swglogparser\sampleLogs\5378010400_chatlog.txt");
            string line;
            List<string> lines = new List<string>();
            List<LogItem> offenseItems = new List<LogItem>();
            List<LogItem> defenseItems = new List<LogItem>();
            List<LogItem> skippedItems = new List<LogItem>();

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("[Combat]"))
                {
                    //Console.WriteLine(line.Substring(18));
                    if (!avoidStrings.Any(line.Contains)) 
                    {
                        lines.Add(line);

                        LogItem logItem = new LogItem();

                        //start parsing the line 
                        logItem.timestamp = line.Substring(10, 8);

                        if (offenseStrings.Any(line.Contains))
                        {
                            logItem.restOfIt = line.Substring(18);
                            offenseItems.Add(logItem);
                          
                        };

                        if (defenseStrings.Any(line.Contains))
                        {
                            logItem.restOfIt = line.Substring(18);
                            defenseItems.Add(logItem);
                        };


                        //trap missing lines to find bugs based on the kludgy string manipulation
                        if (!defenseStrings.Any(line.Contains) && !offenseStrings.Any(line.Contains))
                        {
                            logItem.restOfIt = line.Substring(18);
                            skippedItems.Add(logItem);

                        };

                    }



                }
            }
            WriteFile(offenseItems, "offenseItems.txt");
            WriteFile(defenseItems, "defenseItems.txt");
            WriteFile(skippedItems, "skippedItems.txt");
        }

        static void WriteFile(List<LogItem> items,string filename)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"c:\temp\", filename)))
            {
                foreach(LogItem item in items)
                {
                    outputFile.WriteLine(item.restOfIt);
                }
            }
        }
    }

    public class LogItem
    {
        public string timestamp { get; set; }
        public string restOfIt { get; set; }


    }
}
