using System.Windows;
using ClassLibrary;

namespace HomeForCat
{
    public partial class EndWindow : Window
    {
        Language language;
        Gamer gamers;
        public EndWindow(Language lang, Gamer gamersList)
        {
            InitializeComponent();
            language = lang;
            gamers = gamersList;
            changeLanguage();
            goodEndLab.Visibility = Visibility.Visible;
            badEndLab.Visibility = Visibility.Hidden; ;
            goodEndImage.Visibility = Visibility.Visible;
            badEndImage.Visibility = Visibility.Hidden;
            if (gamers.GamerProgress[gamers.CurrentGamer] < 0)
            {
                goodEndLab.Visibility = Visibility.Hidden;
                badEndLab.Visibility = Visibility.Visible;
                goodEndImage.Visibility = Visibility.Hidden;
                badEndImage.Visibility = Visibility.Visible;
            }
        }
        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            gamers.DeleteCurrentGamer();
            MainWindow mainWind = new MainWindow(language, gamers);
            mainWind.Show();
            this.Close();
        }

        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
            gamers.ResetCurrentGamer();
            GameWindow gameWind = new GameWindow(language, gamers);
            gameWind.Show();
            this.Close();
        }

        private void changeLanguage()
        {
            this.Title = language.changeLang("WinTitle");
            endingLab.Content = $"{language.changeLang("greetingsPart1")} {gamers.GamerNames[gamers.CurrentGamer]}," +
                $" {language.changeLang("greetingsPart2")}";
            goodEndLab.Text = language.changeLang("goodEndLab");
            badEndLab.Text = language.changeLang("badEndLab");
            question.Content = language.changeLang("question");
            yesBtn.Content = language.changeLang("yesBtn");
            noBtn.Content = language.changeLang("noBtn");
        }
    }
}
