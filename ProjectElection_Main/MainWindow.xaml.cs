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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;

namespace ProjectElection_Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected string password = "";
        public Button[] ButtonArr;
        public HttpClient http = new HttpClient();
        public Window1 VoteWindow = new Window1();
        public MainWindow()
        {
            http.BaseAddress = new Uri("https://localhost:7241/");
            InitializeComponent();
            
            //Number Buttons
            ButtonArr = new Button[] { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9 };
            foreach (Button button in ButtonArr){
                button.Click += WriteNumber;
            }

            //Delete button
            bDel.Click += (object sender, RoutedEventArgs e) =>{
                password = string.Concat(password.SkipLast(1));
                PasswordBox.Text = password;
            };

            //Submit button Test
            bTry.Click += async(object sender, RoutedEventArgs e) =>{
                string result = await http.GetAsync($"/ValidateVoter/?UID={password}").Result.Content.ReadAsStringAsync();
                result = result.Trim('"');
                if (result == "success"){
                    VoteWindow.Show();
                    Error.Visibility = Visibility.Hidden;
                }else{
                    Error.Visibility = Visibility.Visible;
                }
            };
        }

        private void WriteNumber(object sender, RoutedEventArgs e){
            Button btn = (Button)sender;
            password += btn.Name[1]; // Buttons are named as "bN"(b0,b1,b2,...) getting 1st member basically gets N
            PasswordBox.Text = password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
