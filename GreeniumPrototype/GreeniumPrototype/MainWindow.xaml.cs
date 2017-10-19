using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using CefSharp.Wpf;
using GreeniumCore.API;
using GreeniumCore.FileTracker.FileTracker;
using GreeniumPrototype.Models;
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
        private static readonly XmlBookProvider bookmarks = new XmlBookProvider(System.AppDomain.CurrentDomain.BaseDirectory, "main");
        private static String UUID;
        private static LogModel MySession;

        //private Storyboard AccountStoryBoard { get; set; }

        internal static async Task RunPeriodicAsync(Action onTick,
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
            Pages.Add("Bookmarks", new BookmarksPage());

        }



        public MainWindow() {
            
            InitializeComponent();
            AccountGrid.DataContext = MySession;
            RunPeriodicAsync(()=> { AutoRegister(); }, new TimeSpan(0,0,1), new TimeSpan(0,0,30),CancellationToken.None);
            bookmarks.OpenFile();
            ((BookmarksPage)Pages["Bookmarks"]).UpdateBookmarks();

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


        private void txtUrl_KeyUp(object sender, KeyEventArgs e){
                if (e.Key == Key.Enter)
                    Browser.Load(txtUrl.Text);
            //Browser.Navigate(txtUrl.Text);
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
                if (Browser.CanGoBack)
                {
                    
                }
                //Browser.GoBack();
            }

            private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = ((Browser != null) && (Browser.CanGoForward));
            }

            private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                if (Browser.CanGoForward)
                {
                    
                }
                //Browser.GoForward();
            }

            private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
            }

            private void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                Browser.Load(txtUrl.Text);
                //Browser.Navigate(txtUrl.Text);
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

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            var browser = (ChromiumWebBrowser)sender;
            txtUrl.Text = browser.Address.ToString();

        }

   
        private void PinURL()
        {
            var CommonUID = Guid.NewGuid().ToString().Replace("-","");
            var generatedGrid = new Grid() {Name = $"TH{CommonUID}", Width = 75};
            generatedGrid.Children.Add(new Label() {Content = Browser.Address, HorizontalAlignment = HorizontalAlignment.Left});
            var interaction = new Button() {Name=$"BTN{CommonUID}", Content = "X", Width = 20, Height = 20, VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right};

            interaction.Click += (object sender, RoutedEventArgs e) =>{
                var btn = sender as Button;

                foreach (TabItem item in MainControl.Items)
                {
                    if (item.Name.Trim().Equals("") || !item.Name.StartsWith("TBC"))
                        continue;

                    if (item.Name.Substring(3).Equals(btn.Name.Substring(3)))
                    {
                        ((ChromiumWebBrowser) ((Grid) item.Content).Children[0]).Dispose();
                        MainControl.Items.Remove(item);
                        break;
                    }
                }
            };

            generatedGrid.Children.Add(interaction);

            Grid innerGrid = new Grid();
            innerGrid.Children.Add(new ChromiumWebBrowser() {Address = Browser.Address});

            MainControl.Items.Add(new TabItem()
            {
                Name=$"TBC{CommonUID}",
                Header = generatedGrid,
                Content = innerGrid,
                
            });
        }

        private void PinBtn_Click(object sender, RoutedEventArgs e){
            PinURL();
        }

        private void BookmarkBtn_Click(object sender, RoutedEventArgs e)
        {
            var guid = Guid.NewGuid().ToString();
            var result = bookmarks.AddNode(guid, Browser.Address);
            if (result != null)
                BookmarksPage.BookmarkAdd(new BookmarkItem() {Link = Browser.Address, Name = result });


        }

        public static List<String> GetBookmarks()
        {
            return bookmarks.ReadBookmarks().ToList();
        }

        private void btnBookmarks_Click(object sender, RoutedEventArgs e)
        {
            if (btnRightMenuHide.Visibility != Visibility.Visible)
                btnRightMenuShow_Click(null, null);

            
            SideMenuFrame.Navigate(Pages["Bookmarks"]);
        }

        public static bool DeleteBookmark(string ID)
        {
            return bookmarks.DeleteBookmark(ID);
        }
     
    }
}
