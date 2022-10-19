using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Http;

namespace ProjectElection_Main
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Ellipse UnlockedEllipse;
        public Ellipse DashEllipse;
        public List<SolidColorBrush> Colours = new List<SolidColorBrush>();
        public RadioButton[] UnlockedMembers = new RadioButton[5];
        public RadioButton[] DashMembers = new RadioButton[5];
        public RadioButton[] Everyone = new RadioButton[10];
        public Dictionary<string, int> IdList = new Dictionary<string, int> {
            //Unlocked
            {"Nisan",1101},
            {"Nesibe",1102},
            {"Josie",1103},
            {"Tan",1104},
            {"Gaye",1105},
            //Dash
            {"Ediz",1201},
            {"Belgin",1202},
            {"Laren",1203},
            {"Berk",1204},
            {"Serkan",1205},
        };
        public string UID = "";
        private int TeamId = 0;
        private int[] CaptainIds = new int[2] { 0, 0 };
        private int i = 0;
        private HttpClient http = new HttpClient();
        public Window1()
        {
            http.BaseAddress = new Uri("https://localhost:7241/");
            InitializeComponent();
            //Colours wooo
            Colours.Add(new SolidColorBrush(Colors.Yellow));
            Colours.Add(new SolidColorBrush(Colors.White));
            //Members
            UnlockedMembers = new RadioButton[5] { Nisan, Nesibe, Josie, Tan, Gaye };
            DashMembers = new RadioButton[5] { Ediz, Belgin, Laren, Berk, Serkan};
            Everyone = UnlockedMembers.Concat(DashMembers).ToArray();

            foreach(RadioButton rButton in Everyone)
            {
                rButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    RadioButton btn = (RadioButton)sender;
                    CaptainIds[i] = IdList[btn.Name];
                    i++;

                    if(i == 2){
                        foreach (RadioButton rButton in Everyone){
                            rButton.IsEnabled = false;
                        }
                        Submit.Visibility = Visibility.Visible;
                    }else{
                        btn.IsEnabled = false;
                    }
                };
            }
        }

        //Submit button
        private void Button_Click(object sender, RoutedEventArgs e){
            SubmitVote();
            this.Close(); //ironic... This was last line of code :kekw: -Dora
        }

        private async void SubmitVote(){
            await http.PostAsync($"/CastVote/?UID={UID}&VotedTeamId={TeamId}&VotedCaptainIds={{{CaptainIds[0]},{CaptainIds[1]}}}", new StringContent(""));
        }

        //Couldn't be bothered making a seperate function -Dora
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Glow effect for chosen team
            UnlockedEllipse = (Ellipse)UnlockedBtn.Template.FindName("Stroke", UnlockedBtn);
            DashEllipse = (Ellipse)DashBtn.Template.FindName("Stroke", DashBtn);
            UnlockedEllipse.Stroke = Colours[0];
            DashEllipse.Stroke = Colours[1];
            
            //Set up few extra stuff
            TeamId = 11;
            ClearSelections();

            //Show Unlocked captains
            UnlockedCaptain.Visibility = Visibility.Visible;
            DashCaptain.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Glow effect for chosen team
            UnlockedEllipse = (Ellipse)UnlockedBtn.Template.FindName("Stroke", UnlockedBtn);
            DashEllipse = (Ellipse)DashBtn.Template.FindName("Stroke", DashBtn);
            UnlockedEllipse.Stroke = Colours[1];
            DashEllipse.Stroke = Colours[0];

            //Set up few extra stuff
            TeamId = 12;
            ClearSelections();

            //Show Dash captains
            DashCaptain.Visibility = Visibility.Visible;
            UnlockedCaptain.Visibility = Visibility.Hidden;
        }

        private void ClearSelections(){
            foreach(RadioButton rButton in Everyone){
                rButton.IsChecked = false;
                rButton.IsEnabled = true;
            }
            Submit.Visibility = Visibility.Hidden;
            CaptainIds = new int[2] { 0, 0 };
            i = 0;
        }
    }
}
