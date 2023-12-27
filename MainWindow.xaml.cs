using System.Text.RegularExpressions;
using System.Windows;
using ClassLibrary;

namespace HomeForCat
{
    public partial class MainWindow : Window
    {
        Gamer gamersList = new Gamer();
        Language language = new Language();

        public MainWindow()
        {
            InitializeComponent();
            getGamersList();
            gamerExistedName.SelectedIndex = 0;
            lang.SelectedIndex = 0;
        }

        public MainWindow(Language selectedLang, Gamer gamers)
        {
            InitializeComponent();
            getGamersList();
            gamerExistedName.SelectedIndex = 0;
            language = selectedLang;
            if (language.LangUa) lang.SelectedIndex = 0;
            if (language.LangEng) lang.SelectedIndex = 1;
            gamersList = gamers;
            gamerExistedName.SelectedIndex = gamersList.CurrentGamer;
        }

        private void checkBoxNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxNewPlayer.IsChecked == true)
            {
                gamerNewName.Visibility = Visibility.Visible;
                gamerExistedName.Visibility = Visibility.Hidden;
                startBtn.IsEnabled = true;
            }
            else
            {
                gamerExistedName.Visibility = Visibility.Visible;
                gamerNewName.Visibility = Visibility.Hidden;
                if (gamersList.GamerNames.Count == 0)
                {
                    startBtn.IsEnabled = false;
                    gamerExistedName.IsEnabled = false;
                }
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxNewPlayer.IsChecked == false)
            {
                gamersList.CurrentGamer = gamerExistedName.SelectedIndex;
                GameWindow gameWind = new GameWindow(language, gamersList);
                gameWind.Show();
                this.Close();
            }
            else
            {
                Regex temp = new Regex(@"[A-Za-zА-ЯЇЄІҐа-яїєґьі']{1,}");
                Match tempMatch = temp.Match(gamerNewName.Text);
                if (!tempMatch.Success)
                {
                    MessageBox.Show(language.changeLang("errorMessage"), language.changeLang("errorTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    gamerNewName.Text = "";
                }
                else
                {
                    gamersList.AddNewGamer(gamerNewName.Text);
                    GameWindow gameWind = new GameWindow(language, gamersList);
                    gameWind.Show();
                    this.Close();
                }
            }
        }

        private void lang_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lang.SelectedIndex == 0)
            {
                language.LangUa = true;
                language.LangEng = false;
            }
            else
            {
                language.LangUa = false;
                language.LangEng = true;
            }
            this.Title = language.changeLang("WinTitle");
            LabelLang.Content = language.changeLang("LabelLang");
            authorLab.Content = language.changeLang("authorLab");
            ua.Text = language.changeLang("ua");
            eng.Text = language.changeLang("eng");
            ExpanderRules.Header = language.changeLang("ExpanderRules");
            LabelRules.Text = language.changeLang("LabelRules");
            LabelTitle.Content = language.changeLang("LabelTitle");
            checkBoxNewPlayer.Content = language.changeLang("checkBoxNewPlayer");
            startBtn.Content = language.changeLang("startBtn");
            emptyList.Text = language.changeLang("emptyList");
            gamerNewName.Text = language.changeLang("gamerNewName");
        }
        private void getGamersList()
        {
            if (gamersList.GamerNames.Count != 0) {
                gamerExistedName.Items.Clear();
                for (int i = 0; i < gamersList.GamerNames.Count; i++)
                {
                    gamerExistedName.Items.Add(gamersList.GamerNames[i]);
                }
            }
            else
            {
                startBtn.IsEnabled = false;
                gamerExistedName.IsEnabled = false;
            }
        }
    }
}
