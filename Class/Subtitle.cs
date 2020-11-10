using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleReader
{
    public class Subtitle
    {
        readonly Dictionary<int, Srt> allSrt;
        public Dictionary<int, Srt> AllSrt { get => allSrt; }
        public Subtitle()
        {
            allSrt = new Dictionary<int, Srt>();
            srtParser();
        }

        private void srtParser()
        {
            string path = @"C:\Users\Tom\Desktop\Lecteur\eren-transform.srt";
            using (StreamReader sr = new StreamReader(path))
            {
                // Init var
                int idSrt = 0;
                float start = 0;
                float end = 0;
                string content = "";

                // Browse line
                string l = "";
                while ((l = sr.ReadLine()) != null)
                {
                    if (l == "")
                    {
                        // Add to dictionary
                        allSrt.Add(idSrt, new Srt(idSrt, start, end, content));
                        // Reset var
                        idSrt = 0;
                        start = 0;
                        end = 0;
                        content = "";
                    }
                    else
                    {
                        if (idSrt == 0)
                            // Id split
                            int.TryParse(l, out idSrt);
                        else if (l.Contains("-->"))
                        {
                            // Time split
                            string[] timeSplit = l.Split(new string[] { " --> " }, StringSplitOptions.RemoveEmptyEntries);
                            string[] startSplit = timeSplit[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] endSplit = timeSplit[1].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);


                            // Parsing Start
                            //init and assign var
                            int.TryParse(startSplit[0], out int h);
                            int.TryParse(startSplit[1], out int m);
                            float.TryParse(startSplit[2], out float s);
                            //Calcul milisecond
                            m += h * 60;
                            s += +m * 60;
                            start = s*1000;

                            // Parsing End
                            //init and assign var
                            int.TryParse(endSplit[0], out h);
                            int.TryParse(endSplit[1], out m);
                            float.TryParse(endSplit[2], out s);
                            //Calcul milisecond
                            m += +h * 60;
                            s += +m * 60;
                            end = s*1000;

                            //Console.WriteLine("start : "+start+ " | end : " + end);
                        }
                        else
                        {
                            // Assign content
                            if (content == "")
                                content = l;
                            // If more than one line assign incremente content with \n and new content
                            else
                                content += "\r\n" + l;
                        }
                    }
                }
                // Add to dictionary
                allSrt.Add(idSrt, new Srt(idSrt, start, end, content));
            }
        }
    }
}
