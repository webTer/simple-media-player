using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MediaPlayerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sldTimeline.Value = player.Position.TotalMilliseconds;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            sldTimeline.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
            sldVolume.Maximum = player.Volume;
            timer?.Stop();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            player.Stop();

            int nextVideoIndex = listBox.SelectedIndex + 1;
            listBox.SelectedIndex = nextVideoIndex;
        }

        private void OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void OnPlay(object sender, RoutedEventArgs e)
        {
            player.Play();
        }

        private void OnPause(object sender, RoutedEventArgs e)
        {
            player.Pause();
     
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP4 files (*.mp4)|*.mp4";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            bool? answer = fileDialog.ShowDialog();
            if (answer == true)
            {
                string fileName = fileDialog.FileName;
                string videoName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                VideoInfo video = new VideoInfo{ Name = videoName, FileName = fileName };
                listBox.Items.Add(video);
                player.Source = new Uri(fileName);
                player.Play();
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timer?.Stop();
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double milliseconds = sldTimeline.Value;
            player.Position = TimeSpan.FromMilliseconds(milliseconds);
            timer?.Start();
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem is VideoInfo current)
            {
                player.Source = new Uri(current.FileName);
                player.Play();
            }
        }
    }
}
