using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using GreeniumPrototype.Models;
using GreeniumPrototype.Models.CircularProgress;
using Newtonsoft.Json.Linq;

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
        private static LogModel MySession;

        //private Storyboard AccountStoryBoard { get; set; }

        private static async Task RunPeriodicAsync(Action onTick,
            TimeSpan dueTime,
            TimeSpan interval,
            CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                onTick?.Invoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        static MainWindow()
        {
            Pages.Add("Contribute", new Contribute());
            Pages.Add("Emails", new Emails());
  
        }



        public MainWindow() {
            
            InitializeComponent();
            AccountGrid.DataContext = MySession;
            RunPeriodicAsync(()=> { AutoRegister(); }, new TimeSpan(0,0,1), new TimeSpan(0,0,30),CancellationToken.None);
           
            
            //UCID_Param.Text = UniqueSerial.GetVolumeSerial(); To be revised          

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
            var browser = (WebBrowser) sender;
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

        private void Login()
        {
            throw new NotImplementedException();
        }


        private void AnimateProgress()
        {
            throw new NotImplementedException();
        }

        private async Task AutoRegister()
        {
            int from = 0, to = 0;
            if (MySession == null)
            {
                MySession = new LogModel();
            }
            else
            {
                from = MySession.XP;
            }

            if (!xcp.FileExists())
            {
                try
                {
                    var User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var TruncUser = User.Substring(User.LastIndexOf(@"\"));
                    var result = JObject.Parse(await RequestHandler.Call("http://localhost:10840/Account/Create", "POST", null, null,
                        new
                        {
                            Name = User,
                            FullName = TruncUser,
                            Emails = new string[] {"nom.prenom@gmail.com"},
                            UID = (UUID = Guid.NewGuid().ToString()),
                        }
                    ));


                    MySession.ID = result["ID"].ToString();
                    MySession.Name = result["Name"].ToString();
                    MySession.FullName = result["FullName"].ToString();
                    MySession.XP = int.Parse(result["XP"].ToString());
                    MySession.Level = int.Parse(result["Level"].ToString());
                    MySession.Creation = result["Creation"].ToString();

                    xcp.OpenFile();
                    xcp.AddNode(UUID, MySession.ID);
                    xcp.SaveFile();
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
                    xcp.OpenFile();
                    var Config = xcp.ReadIdentifiers().First();

                    var ConfigArray = Config.Split(';');
                    UUID = ConfigArray[0];

                    var result = JObject.Parse(await RequestHandler.Call($"http://localhost:10840/Account/{ConfigArray[1]}", "GET", null, null, null));

                    MySession.ID = result["ID"].ToString();
                    MySession.Name = result["Name"].ToString();
                    MySession.FullName = result["FullName"].ToString();
                    MySession.XP = int.Parse(result["XP"].ToString());
                    MySession.Level = int.Parse(result["Level"].ToString());
                    MySession.Creation = result["Creation"].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            to = MySession.XP;
            if (from == 0)
                from = to;

            Application.Current.Dispatcher.Invoke(()=>{
                
                UCID_Param.Text = UUID;
                UCID_Param2.Text = MySession.ID;
                var total = (((MySession.Level)) * (0.25 * (MySession.Level) + 1.0)) * 500.0;
                TotalXP.Text = $"{total}";
                CurrentXPTB.Text = $"{MySession.XP}";
                LevelLabel.Text = $"{MySession.Level}";
                AccountProgress.Value = MySession.XP;
                AccountProgress.Range = total;
 
            });

        }

    }
}
