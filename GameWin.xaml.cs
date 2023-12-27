using System.Windows;
using ClassLibrary;
using System.Windows.Input;
using System;
//using System.Windows.Threading;
using System.Windows.Media;

namespace HomeForCat
{
    public partial class GameWindow : Window
    {
        Language language;
        Gamer gamers;
        MazeField maze; 
        AvatarNavigation nav;
        DispatcherTimer gameTimer = new DispatcherTimer();
        int timerSeconds, timerMinutes;

        public GameWindow(Language lang, Gamer gamersList)
        {
            InitializeComponent();
            language = lang;
            ChangeLanguage();
            gameTimer.Tick += new EventHandler(TimerTick);
            gameTimer.Interval = new TimeSpan(0, 0, 1);
            gamers = gamersList;

            getProgress();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWind = new MainWindow(language, gamers);
            mainWind.Show();
            this.Close();
        }

        private void generateBtn_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            int mazeWidth = (int)width.Value;
            int mazeHeight = (int)height.Value;
            timerSeconds = 0;
            timerMinutes = 0;

            waitImage.Visibility = Visibility.Hidden;
            waitLab.Visibility = Visibility.Hidden;
            badImage.Visibility = Visibility.Hidden;
            badLab.Visibility = Visibility.Hidden;
            goodImage.Visibility = Visibility.Hidden;
            goodLab.Visibility = Visibility.Hidden;

            MazeGeneration GeneratedMaze = new MazeGeneration(mazeWidth, mazeHeight);
            maze = new MazeField(mazeField, GeneratedMaze);
            nav = new AvatarNavigation(mazeField, GeneratedMaze);
            drawField();

            gameTimer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timerSeconds++;
            if(timerSeconds % 60 == 0)
            {
                timerSeconds = 0;
                timerMinutes++;
            }
            timer.Text = new TimeSpan(0, timerMinutes, timerSeconds).ToString();
        }

        private void Game_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            drawField();
        }

        private void drawField()
        {
            if (maze != null)
            {
                ChangeFieldSize();
                maze.DrawMaze(mazeField);
                maze.DrawExits(mazeField);
                nav.DrawAvatar(mazeField);
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (maze != null)
            {
                nav.PressKey(e);
                if (nav.InPositiveExit())
                {
                    gameTimer.Stop();
                    gamers.ChangeProgress(timerMinutes*60 + timerSeconds, maze.width * maze.height, true);
                    showGoodEnd();
                    getProgress();
                    if (gamers.GamerProgress[gamers.CurrentGamer] >= 1000)
                    {
                        EndWindow endWin = new EndWindow(language, gamers);
                        endWin.Show();
                        this.Close();
                    }
                }
                if (nav.InNegativeExit())
                {
                    gameTimer.Stop();
                    gamers.ChangeProgress(timerMinutes * 60 + timerSeconds, maze.width * maze.height, false);
                    showBadEnd();
                    getProgress();
                    if (gamers.GamerProgress[gamers.CurrentGamer] <= -1000)
                    {
                        EndWindow endWin = new EndWindow(language, gamers);
                        endWin.Show();
                        this.Close();
                    }
                }
            }
        }

        private void ChangeLanguage()
        {
            this.Title = language.changeLang("WinTitle");
            backLab.Content = language.changeLang("backLab");
            generateBtn.Content = language.changeLang("generateBtn");
            widthLab.Content = language.changeLang("widthLab");
            heightLab.Content = language.changeLang("heightLab");
            timerLab.Content = language.changeLang("timerLab");
            waitLab.Content = language.changeLang("waitLab");
            goodLab.Content = language.changeLang("goodLab");
            badLab.Content = language.changeLang("badLab");
        }

        private void ChangeFieldSize()
        {
            int bgWidth = (int)(this.Width - 370);
            int bgHeight = (int)(this.Height - 135);
            int itemSize;

            itemSize = (int)(bgWidth / width.Value);

            if (itemSize * height.Value > bgHeight)
            {
                itemSize = (int)(bgHeight / height.Value);
            }
            mazeField.Width = itemSize * width.Value;
            mazeField.Height = itemSize * height.Value;
            mazeField.Background = Brushes.LightGray;

            nav.AvaSize = itemSize;
            maze.ItemSize = itemSize;
        }

        private void getProgress()
        {
            SolidColorBrush pink = new SolidColorBrush(Color.FromRgb(224, 60, 98));
            SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(92, 182, 212));
            progressBar.Width = 200 * Math.Abs(gamers.GamerProgress[gamers.CurrentGamer]) / 1000;
            gamerLab.Content = gamers.GamerNames[gamers.CurrentGamer];
            progressLab.Content = gamers.GamerProgress[gamers.CurrentGamer];
            if (gamers.GamerProgress[gamers.CurrentGamer] < 0)
            {
                progressBar.Fill = pink;
            }
            else
            {
                progressBar.Fill = blue;
            }
        }

        private void showBadEnd()
        {
            mazeField.Background = Brushes.Transparent;
            mazeField.Children.Clear();
            maze = null;
            badImage.Visibility = Visibility.Visible;
            badLab.Visibility = Visibility.Visible;
        }

        private void showGoodEnd()
        {
            mazeField.Background = Brushes.Transparent;
            mazeField.Children.Clear();
            maze = null;
            goodImage.Visibility = Visibility.Visible;
            goodLab.Visibility = Visibility.Visible;
        }
    }
}
