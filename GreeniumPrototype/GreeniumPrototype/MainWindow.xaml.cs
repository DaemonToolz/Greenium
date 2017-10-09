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

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Dictionary<string, Page> Pages = new Dictionary<string, Page>();

        static MainWindow()
        {
            Pages.Add("Contribute", new Contribute());
        }

        public MainWindow() {
            InitializeComponent();
        }

        private void btnRightMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
        }

        private void btnRightMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
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
            SideMenuFrame.Navigate(Pages["Contribute"]);
        }
    }
}
