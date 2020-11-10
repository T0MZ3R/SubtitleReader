using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SubtitleReader
{
    public static class MediaControl
    {
        public static Boolean subtitleState;
        static string content;
        static double mediaTime;
        static Boolean canPrintSubtitle;

        public static void InitStaticVar()
        {
            content = "";
            mediaTime = 0;
            canPrintSubtitle = false;
            subtitleState = true;
        }
        private static MediaState GetMediaState(MediaElement myMedia)
        {
            FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helperObject = hlp.GetValue(myMedia);
            FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            MediaState state = (MediaState)stateField.GetValue(helperObject);
            return state;
        }
        public static void Play(MediaElement video, Image pctPlay)
        {
            if (GetMediaState(video) == MediaState.Play)
            {
                video.Pause();
                pctPlay.Source = new BitmapImage(new Uri(@"Ressources/play.png", UriKind.Relative));

            }
            else
            {
                video.Play();
                pctPlay.Source = new BitmapImage(new Uri(@"Ressources/pause.png", UriKind.Relative));
            }
        }
        public static void MooveBackward(MediaElement video)
        {
            video.Position = new TimeSpan(0,0,0,0, (int)video.Position.TotalMilliseconds-5000);
        }
        public static void MooveForward(MediaElement video)
        {
            video.Position = new TimeSpan(0,0,0,0, (int)video.Position.TotalMilliseconds+5000);
        }
        public static void PrintTimer(MediaElement Video, Label lblTime)
        {
            lblTime.Content = new TimeSpan(0, Video.Position.Hours, Video.Position.Minutes, Video.Position.Seconds, Video.Position.Milliseconds);
            mediaTime = Video.Position.TotalMilliseconds;
        }
        public static void PrintSubtitle(Label lblSubtitle)
        {
            if (canPrintSubtitle)
            {
                if (content == null)
                    content = "";
                if (content.Contains("<i>"))
                {
                    lblSubtitle.FontStyle = FontStyles.Italic;
                    content = content.Replace("<i>","");
                    content = content.Replace("</i>", "");
                }
                else
                {
                    lblSubtitle.FontStyle = FontStyles.Normal;
                }
                lblSubtitle.Content = content;
                canPrintSubtitle = false;
            }
        }
        public static async Task EditSubtitleAsync(MediaElement Video, Label lblSubtitle, Subtitle subtitle)
        {
            for (int i = 1; i < subtitle.AllSrt.Count; i++)
            {
                if (TimeSpan.Compare(new TimeSpan(0,0,0,0,(int)mediaTime),new TimeSpan(0,0,0,0,(int)subtitle.AllSrt[i].Start)) == 1 && TimeSpan.Compare(new TimeSpan(0, 0, 0, 0, (int)mediaTime), new TimeSpan(0, 0, 0, 0, (int)subtitle.AllSrt[i].End)) != 1 && subtitle.AllSrt[i].CanPrint)
                {
                    subtitle.AllSrt[i].CanPrint = false;
                    content = subtitle.AllSrt[i].Content;
                    canPrintSubtitle = true;
                    ClearLbl(Video, subtitle.AllSrt[i]);
                }
                
            }
        }
        private static void ClearLbl(MediaElement Video, Srt srt)
        {
            while (true)
            {
                if (TimeSpan.Compare(new TimeSpan(0, 0, 0, 0, (int)srt.End), new TimeSpan(0, 0, 0, 0, (int)mediaTime)) == -1 || TimeSpan.Compare(new TimeSpan(0, 0, 0, 0, (int)srt.Start), new TimeSpan(0, 0, 0, 0, (int)mediaTime)) == 1)
                {
                    content = "";
                    canPrintSubtitle = true;
                    srt.CanPrint = true;
                    break;
                }
            }
        }

        public static void ActivateCc(Label lblSubtitle)
        {
            if (subtitleState)
            {
                subtitleState = false;
                lblSubtitle.Content = "";
            }
            else
                subtitleState = true;
        }
    }
}
