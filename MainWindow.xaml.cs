using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SubtitleReader
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        Subtitle subtitle;
        Task tt;
        public MainWindow()
        {
            InitializeComponent();
            /*
            MediaControl.InitStaticVar();
            InitDispatcherTimer();
            InitSubtitle();
            InitTask();
            Video.ScrubbingEnabled = true;
            Video.Pause();
            */
        }

        private void InitTask()
        {
            tt = new Task(() => MediaControl.EditSubtitleAsync(Video, lblSubtitle, subtitle));
        }

        private void InitSubtitle()
        {
            subtitle = new Subtitle(textBoxOpenSrt.Text);
        }

        private void InitDispatcherTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,50);
            dispatcherTimer.Tick += new EventHandler(dispatcherTick);
            dispatcherTimer.Start();
        }
        private void dispatcherTick(object sender, EventArgs e)
        {
            MediaControl.PrintTimer(Video, lblTime);
            if (MediaControl.subtitleState)
            {
                MediaControl.PrintSubtitle(lblSubtitle);

                if (tt.Status != TaskStatus.Running)
                {
                    tt = new Task(() => MediaControl.EditSubtitleAsync(Video, lblSubtitle, subtitle));
                    tt.Start();
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void pctPlay_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void gridControl_MouseEnter(object sender, MouseEventArgs e)
        {
            gridControl.Opacity = 100;
        }

        private void gridControl_MouseLeave(object sender, MouseEventArgs e)
        {
            gridControl.Opacity = 0;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            MediaControl.Play(Video, pctPlay);
        }

        private void btnBackward_Click(object sender, RoutedEventArgs e)
        {
            MediaControl.MooveBackward(Video);
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            MediaControl.MooveForward(Video);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    MediaControl.Play(Video, pctPlay);
                    break;
                case Key.Left:
                    MediaControl.MooveBackward(Video);
                    break;
                case Key.Right:
                    MediaControl.MooveForward(Video);
                    break;
                case Key.C:
                    MediaControl.ActivateCc(lblSubtitle);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //dispatcherTimer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaControl.ActivateCc(lblSubtitle);
        }

        private void btnOpenVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textBoxOpenVideo.Text = openFileDialog.FileName;
        }

        private void btnOpenSrt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textBoxOpenSrt.Text = openFileDialog.FileName;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(textBoxOpenVideo.Text == "VideoFile" && textBoxOpenSrt.Text == "SubtitleFile"))
            {
                Video.Source = new Uri(textBoxOpenVideo.Text);
                MediaControl.InitStaticVar();
                InitDispatcherTimer();
                InitSubtitle();
                InitTask();
                Video.ScrubbingEnabled = true;
                Video.Pause();

                gridMenu.Opacity = 0;
                gridPlayer.Opacity = 1;
            }
            else
            {
                textBoxOpenVideo.BorderBrush = Brushes.Red;
                textBoxOpenSrt.BorderBrush = Brushes.Red;
            }
        }
    }
}


// TODO:
/*
 * Interface pour choisir la vidéo
 * Interface pour choisir les sous titres
 * Synchroniser le srt du eren-trasnform
 * 
 * 
 * 
 * 
 */