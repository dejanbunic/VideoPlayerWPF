using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Timers;
using System.Windows.Controls.Primitives;

namespace VideoPlayerWPF
{

    public partial class MainWindow : Window
    {
        private bool userIsDraggingSlider = false;
        DispatcherTimer timer;
        private bool fullscreen = false;
      
        private TimeSpan TotalTime;
        private DispatcherTimer DoubleClickTimer = new DispatcherTimer();
        string file = "";
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
           timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(timer_tick);
        }

        private void timer_tick(object sender, EventArgs e)
        {
           if((!userIsDraggingSlider))
                videoSlider.Value = videoElement.Position.TotalSeconds;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)

        {
            
            if (videoElement.Source != null)
            {
                string buttonString = playButtonPicture.Source.ToString();
                
                if (buttonString.Contains("pictures/play-button.png"))
                {
                    playButtonPicture.Source =  new BitmapImage(new Uri(@"/pictures/pause-button.png", UriKind.Relative));
                    videoElement.Play();
                }
                else
                {
                    playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/play-button.png", UriKind.Relative));
                    videoElement.Pause();
                }
            }
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (!ofd.FileName.Equals(""))
            {
                videoElement.Source = new Uri(ofd.FileName);
                videoElement.LoadedBehavior = MediaState.Manual;
                videoElement.UnloadedBehavior = MediaState.Manual;
                videoElement.Volume = (double)volumeSlider.Value;
                playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/pause-button.png", UriKind.Relative));
                videoElement.Play();
            }

        }
       

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            videoElement.Volume = (double)volumeSlider.Value/100;
           
            labela.Content = (int)volumeSlider.Value+"%";
        }

        private void videoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                labelaVrijeme.Content = TimeSpan.FromSeconds((int)videoSlider.Value) + " / " + TimeSpan.FromSeconds((int)videoSlider.Maximum);
        }

      

        private void videoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan timeSpan = videoElement.NaturalDuration.TimeSpan;
            videoSlider.Maximum = timeSpan.TotalSeconds;
            timer.Start(); 
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            file = (string)((DataObject)e.Data).GetFileDropList()[0];
            videoElement.Source = new Uri(file);
            videoElement.LoadedBehavior = MediaState.Manual;
            videoElement.UnloadedBehavior = MediaState.Manual;
            videoElement.Volume = (double)volumeSlider.Value;
            playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/pause-button.png", UriKind.Relative));
            videoElement.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentControl window = this;

            this.WindowStyle = WindowStyle.None;

            this.WindowState = WindowState.Maximized;
            gridOptions.Visibility = Visibility.Collapsed;
            fullscreen = true;

        }

        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) { 
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
            gridOptions.Visibility = Visibility.Visible;
                fullscreen = false;
            }
            if(e.Key == Key.P)
            {
                if (videoElement.Source != null)
                {
                    string buttonString = playButtonPicture.Source.ToString();

                    if (buttonString.Contains("pictures/play-button.png"))
                    {
                        playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/pause-button.png", UriKind.Relative));
                        videoElement.Play();
                    }
                    else
                    {
                        playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/play-button.png", UriKind.Relative));
                        videoElement.Pause();
                    }
                }
            }
            if(e.Key == Key.F)
            {
                ContentControl window = this;

                this.WindowStyle = WindowStyle.None;

                this.WindowState = WindowState.Maximized;
                //
                gridOptions.Visibility = Visibility.Collapsed;
                fullscreen = true;
            }
            if(e.Key == Key.O)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                if (!ofd.FileName.Equals(""))
                {
                    videoElement.Source = new Uri(ofd.FileName);
                    videoElement.LoadedBehavior = MediaState.Manual;
                    videoElement.UnloadedBehavior = MediaState.Manual;
                    videoElement.Volume = (double)volumeSlider.Value;
                    playButtonPicture.Source = new BitmapImage(new Uri(@"/pictures/pause-button.png", UriKind.Relative));
                    videoElement.Play();
                }
            }
           
        }

        private void gridOptions_MouseMove(object sender, MouseEventArgs e)
        {
           // if(fullscreen)
            gridOptions.Visibility = Visibility.Visible;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
          //  if (fullscreen)
                gridOptions.Visibility = Visibility.Visible;
        }


        private  void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fullscreen)
            {
                if (gridOptions.Visibility.Equals(Visibility.Collapsed))
                    gridOptions.Visibility = Visibility.Visible;
               else
                {
                    gridOptions.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            videoElement.Position = TimeSpan.FromSeconds(videoSlider.Value);
        }

        
        private void volumeSlider_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            if (videoElement.Volume < 0)
            {
                videoElement.Volume = 0;
                volumeSlider.Value = 0;
            }
            if (videoElement.Volume > 1)
            {
                videoElement.Volume = 1;
                volumeSlider.Value = 100;
            }

            videoElement.Volume += (e.Delta > 0) ? 0.05 : -0.05;

            volumeSlider.Value += (e.Delta > 0) ? 5 : -5;
        }

       
    }
}
