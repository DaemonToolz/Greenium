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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GreeniumCore.API;
using GreeniumCore.FileTracker.FileTracker;
using GreeniumCore.Security;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Dictionary<string, Page> Pages = new Dictionary<string, Page>();
        private static readonly XmlConfigProvider xcp = new XmlConfigProvider(System.AppDomain.CurrentDomain.BaseDirectory, "conf");
        private static String UUID;

        static MainWindow()
        {
            Pages.Add("Contribute", new Contribute());
            Pages.Add("Emails", new Emails());
  
        }

        public MainWindow() {
            InitializeComponent();
            UCID_Param.Text = UniqueSerial.GetVolumeSerial();

            AutoRegister();
    
           // xcp.OpenFile();
            //xcp.AddNode("54sq1sqf65f1sd6f51", "");
          
        }

        private void btnRightMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
            Browser.BeginStoryboard(Resources["sbWbEnlarge"] as Storyboard);
        }

        private void btnRightMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
            Browser.BeginStoryboard(Resources["sbWbReduce"] as Storyboard);
        }

        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        {
            var sb = Resources[Storyboard] as Storyboard;
            if (sb == null) return;

            sb.Begin(pnl);

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;

            }
            else if (Storyboard.Contains("Hide"))
            {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                    Browser.Navigate(txtUrl.Text);
            }

            private void wbSample_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
            {
                txtUrl.Text = e.Uri.OriginalString;
            }

            private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = ((Browser != null) && (Browser.CanGoBack));
            }

            private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                Browser.GoBack();
            }

            private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = ((Browser != null) && (Browser.CanGoForward));
            }

            private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                Browser.GoForward();
            }

            private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
            }

            private void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                Browser.Navigate(txtUrl.Text);
            }

        private void Browser_Navigated(object sender, NavigatingCancelEventArgs e)
        {
            if (sender is WebBrowser browser)
                txtUrl.Text = browser.Source.ToString();

        }

        private void ContributeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(btnRightMenuHide.Visibility != Visibility.Visible )
                btnRightMenuShow_Click(null, null);
            SideMenuFrame.Navigate(Pages["Contribute"]);
        }

        private void btnMails_Click(object sender, RoutedEventArgs e)
        {
            if (btnRightMenuHide.Visibility != Visibility.Visible)
                btnRightMenuShow_Click(null, null);
            SideMenuFrame.Navigate(Pages["Emails"]);
        }

        private void Login(){
            
        }

        private async void AutoRegister(){
            if (!xcp.FileExists())
            {
                try
                {
                    var User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var TruncUser = User.Substring(User.LastIndexOf(@"\"));
                    await RequestHandler.Call("http://localhost:10840/Account/Create", "POST", null, null,
                        new
                        {
                            Name = User,
                            FullName = TruncUser,
                            Emails = new string[] {"nom.prenom@gmail.com"},
                            UID = (UUID = Guid.NewGuid().ToString()),
                        }
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    var Config = xcp.ReadIdentifiers().First();

                    var ConfigArray = Config.Split(';');
                    UUID = ConfigArray[0];

                    await RequestHandler.Call($"http://localhost:10840/Account/{ConfigArray[1]}", "GET", null, null, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
