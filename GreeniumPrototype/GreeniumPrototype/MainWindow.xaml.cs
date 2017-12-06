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
using GreeniumCore.FileTracker;
using GreeniumCore.FileTracker.FileTracker;
using GreeniumPrototype.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using CefSharp;
using GreeniumCore.Network.Discovery;
using GreeniumCoreSQL.Engine;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Dictionary<string, Page> Pages = new Dictionary<string, Page>();
        
        private static readonly XmlUserSettingsProvider configuration
            = new XmlUserSettingsProvider(System.AppDomain.CurrentDomain.BaseDirectory, "grn");
        private static readonly XmlConfigProvider xcp 
            = new XmlConfigProvider(System.AppDomain.CurrentDomain.BaseDirectory, "conf");
        private static readonly XmlBookProvider bookmarks 
            = new XmlBookProvider(System.AppDomain.CurrentDomain.BaseDirectory, "main");
        private static String UUID;
        private static LogModel MySession;

        public static LiteEngine HistoryEngine;


        private static String GreeniumDefaultURL => Properties.GeneralSettings.Default.MSAccountLink;

        private static uint BackupXP
        {
            get { return Properties.GeneralSettings.Default.BackupXP; }
            set {
                Properties.GeneralSettings.Default.BackupXP = value;
                Properties.GeneralSettings.Default.Save();
            }
        }

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
            Pages.Add("Bookmarks", new BookmarksPage());
            Pages.Add("History", new UserHistoryPage());

            HistoryEngine = new LiteEngine();
        }

        private bool Init = false;
        private bool ChangeOnLoad = false;

        public MainWindow()
        {

           
            InitializeComponent();
            
            AccountGrid.DataContext = MySession;

         
            Init = true;

            InitializeSettings();
            ChangeOnLoad = true;

            RunPeriodicAsync(()=> { AutoRegister(); }, new TimeSpan(0,0,1), new TimeSpan(0,0,30),CancellationToken.None);
            bookmarks.OpenFile();
            ((BookmarksPage)Pages["Bookmarks"]).UpdateBookmarks();
            ((BookmarksPage)Pages["Bookmarks"]).Owner = this;
            ((UserHistoryPage)Pages["History"]).Owner = this;
            ((UserHistoryPage) Pages["History"]).UpdateHistory();

            HistoryCountParam.Content = HistoryEngine.Count();
            UCID_BXP_Param.Text = BackupXP + "";

            // -------------------------- EMAIL TEMPLATE TEST

            var emails = new List<Object>();
            for (int i = 0; i < 10; ++i)
                emails.Add(new {Nom = $"Email {i}", Color=$"#{7+((i*4)%2)}{5+((i * 4) % 6)}{(i) % 9}{2+((i * 6) % 7)}{5+((i) % 4)}{3+((i * 5) % 6)}"});

            EmailListbox.ItemsSource = emails ;


            // --------------------------


            //UCID_Param.Text = UniqueSerial.GetVolumeSerial(); To be revised          

        }

        private void InitializeBrowser()
        {
            var BrowserSettings = new CefSettings();
            BrowserSettings.CachePath = $@"{System.AppDomain.CurrentDomain.BaseDirectory}\cache";
            Cef.Initialize(BrowserSettings);
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

            public void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                Browser.Load(txtUrl.Text);
                HistoryEngine.Add(txtUrl.Text);

                if (!MySession.Online){
                    BackupXP = BackupXP + 50; // Exemple
                    UCID_BXP_Param.Text = BackupXP.ToString();
                }
                //Browser.Navigate(txtUrl.Text);
            }

    

        private void ContributeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(btnRightMenuHide.Visibility != Visibility.Visible )
                btnRightMenuShow_Click(null, null);
            SideMenuFrame.Navigate(Pages["Contribute"]);
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
                      
                    var result = JObject.Parse(await RequestHandler.Call($"{GreeniumDefaultURL}/Create", "POST", null, null,
                        new
                        {
                            Name = User,
                            FullName = TruncUser,
                            Emails = new string[] {"nom.prenom@gmail.com"},
                            UID = (UUID = Guid.NewGuid().ToString()),
                        }
                    ));


                    LoadFromResult(result);
                    xcp.OpenFile();
                    xcp.AddNode(UUID, MySession.ID);
                    xcp.SaveFile();
                }
                catch (Exception e)
                {
                    SetOffline();
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

                    var result = JObject.Parse(await RequestHandler.Call($"{GreeniumDefaultURL}/{ConfigArray[1]}", "GET", null, null, null));
                    LoadFromResult(result);

                }
                catch (Exception e)
                {
                    SetOffline();
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
                UCID_Cnt_Param.Text = MySession.Online ? "Online mode" : "Offline mode";
            });

            if (!MySession.Online)
                OfflineModeGrid.Visibility = Visibility.Visible;
            else
                OfflineModeGrid.Visibility = Visibility.Hidden;
        }

        private void LoadFromResult(JObject result)
        {

            MySession.ID = result["ID"].ToString();
            MySession.Name = result["Name"].ToString();
            MySession.FullName = result["FullName"].ToString();
            MySession.XP = int.Parse(result["XP"].ToString());
            MySession.Level = int.Parse(result["Level"].ToString());
            MySession.Creation = result["Creation"].ToString();
            MySession.Online = true;
        }

        private void SetOffline()
        {
            MySession.Online = false;
            MySession.ID = Guid.NewGuid().ToString();
            MySession.Name = "Offline";
            MySession.FullName = "Offline";
            MySession.XP = 0;
            MySession.Level = 0;
            MySession.Creation = DateTime.Now.ToString();
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            var browser = (ChromiumWebBrowser)sender;
            if (browser.Address == null)
                txtUrl.Text = "https://www.ecosia.org/";
            txtUrl.Text = browser.Address.ToString();

        }

   
        private void PinURL()
        {
            var CommonUID = Guid.NewGuid().ToString().Replace("-","");
            var generatedGrid = new Grid()
            {
                Name = $"TH{CommonUID}", Width = 150
            };

            Func<string, string> QuickDiscovery = (url) => {
                SiteDiscovery.Discover(url);
                return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\Data\Site\Cache\{SiteDiscovery.FindDomain(url)}.ico";
            };

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(QuickDiscovery(Browser.Address));
            logo.EndInit();

            generatedGrid.Children.Add(
                new Image() {
                    Width = 20,
                    Height = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Source = logo
                }
            );


            generatedGrid.Children.Add(
                new Label()
                {
                    Margin = new Thickness(25,0,0,0),
                    Content = Browser.Address,
                    FontSize = 9.0,
                    HorizontalAlignment = HorizontalAlignment.Left
                });

            var interaction = new Button()
            {
                Name=$"BTN{CommonUID}",
                Content = "X",
                Width = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };


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


        public bool LoadSettings(){
            try
            {
                if(configuration.OpenedDocument == null) configuration.OpenFile();
                var mySettings = configuration.ReadConfig();
                if (!mySettings.Any()) return false;

                foreach (var currentSetting in mySettings){
                    var stringSetting = currentSetting.Split(';');

                    var selection = stringSetting.First(kv => kv.StartsWith("autolog")).Split(':')[1];
                    AutologSwitch.IsChecked = bool.Parse(selection);

                    selection = stringSetting.First(kv => kv.StartsWith("idsave")).Split(':')[1];
                    PasswordSaveSwitch.IsChecked = bool.Parse(selection);

                    selection = stringSetting.First(kv => kv.StartsWith("privacy")).Split(':')[1];
                    AnonymousSwitch.IsChecked = selection.ToLower().Equals("full");

                    selection = stringSetting.First(kv => kv.StartsWith("ishost")).Split(':')[1];
                    HostSwitch.IsChecked = bool.Parse(selection);

                    selection = stringSetting.First(kv => kv.StartsWith("defaultbrowser")).Split(':')[1];
                    SearchEngineCbox.Text = selection;

                    Browser.Address = $"https://www.{selection.ToLower()}.{(selection.ToLower().Equals("ecosia")?"org":"com")}/";
                }
                ChangeOnLoad = false;

                return true;
            }
            catch
            {
                return false;
            }
        }
 
        public bool InitializeSettings(bool ForceUpdate = false){
            try
            {
                if (configuration.OpenedDocument == null)
                    configuration.OpenFile();
                if (LoadSettings() && !ForceUpdate) return true;
                var result = configuration.InsertOrUpdateConfig(
                    new Dictionary<string, string>() {
                        { "username",""},
                        { "emails", ""},
                        {"autolog","false" },
                        {"idsave","false" },
                        {"defaultbrowser","Google" },
                        {"ishost","false" },
                        {"privacy","partial" },
                        {"isfixedseed","true" },
                        {"seed", Guid.NewGuid().ToString().Replace("-","")},
                        {"p2pstatus","offline" },
                        {"p2presolver","offline" },
                        {"p2pprotocol","Grn" }
                    });
                configuration.SaveFile();
                return result;
            }
            catch
            {
                return false;    
            }

        }

        //

        private async void UpdateSettings()
        {
            if (configuration.OpenedDocument == null)
                configuration.OpenFile();

            var result = configuration.InsertOrUpdateConfig(
                new Dictionary<string, string>() {
                    { "username","TBA"},
                    { "emails", JsonConvert.SerializeObject(new string[]{"nom.prenom@gmail.com","username@gmail.com"})},
                    {"autolog", AutologSwitch.IsChecked.ToString() },
                    {"idsave", PasswordSaveSwitch.IsChecked.ToString() },
                    {"defaultbrowser", ((ComboBoxItem)SearchEngineCbox.SelectedValue).Content.ToString() },
                    {"ishost",HostSwitch.IsChecked.ToString() },
                    {"isfixedseed","True" },
                    {"privacy",AnonymousSwitch.IsChecked ? "full" : "partial" },
                    {"seed", Guid.NewGuid().ToString().Replace("-","")},
                    {"p2pstatus","offline" },
                    {"p2presolver","offline" },
                    {"p2pprotocol","Grn" }
                });
            configuration.SaveFile();
        }

        private void AutologSwitch_Checked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void HostSwitch_Checked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void PasswordSaveSwitch_Checked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void AnonymousSwitch_Checked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void AnonymousSwitch_Unchecked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void PasswordSaveSwitch_Unchecked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void HostSwitch_Unchecked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void AutologSwitch_Unchecked(object sender, RoutedEventArgs e) {
            UpdateSettings();
        }

        private void SearchEngineCbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Init && ChangeOnLoad) UpdateSettings();
        }



        private void HostSwitchInfoBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            HostSwitchDescriptor.Visibility = Visibility.Visible;
        }

        private void HostSwitchInfoBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            HostSwitchDescriptor.Visibility = Visibility.Hidden;
        }

        private void IdSaveSwitchBtnInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            PwdSwitchDescriptor.Visibility = Visibility.Visible;
        }

        private void IdSaveSwitchBtnInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            PwdSwitchDescriptor.Visibility = Visibility.Hidden;
        }

        private void AutologSwitchInfoBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            AutologSwitchDescriptor.Visibility = Visibility.Visible;
        }

        private void AutologSwitchInfoBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            AutologSwitchDescriptor.Visibility = Visibility.Hidden;
        }

        private void PrivacySwitchInfoBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            PrivacySwitchDescriptor.Visibility = Visibility.Visible;
        }

        private void PrivacySwitchInfoBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            PrivacySwitchDescriptor.Visibility = Visibility.Hidden;
        }

        private void HistoryClearBtn_Click(object sender, RoutedEventArgs e)
        {
            HistoryEngine.Clear();
        }

        private void btn_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void History_btn_Click(object sender, RoutedEventArgs e)
        {
            if (btnRightMenuHide.Visibility != Visibility.Visible)
                btnRightMenuShow_Click(null, null);


            SideMenuFrame.Navigate(Pages["History"]);
        }

        private void GoToBtn_Click(object sender, RoutedEventArgs e)
        {
            GoToPage_Executed(this, null);
        }

        private void MailCleanerInfoBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            CleanerSwitchDescriptor.Visibility = Visibility.Visible;
        }

        private void MailCleanerInfoBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            CleanerSwitchDescriptor.Visibility = Visibility.Hidden;
        }

        //
    }
}
