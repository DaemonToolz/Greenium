using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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
using GreeniumCore.Network.Discovery;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for BookmarksPage.xaml
    /// </summary>
    public partial class BookmarksPage : Page
    {

        private static ObservableCollection<BookmarkItem> bookmarks = new ObservableCollection<BookmarkItem>();
        public MainWindow Owner { get; set; }

        public BookmarksPage(){
            InitializeComponent();
            UpdateBookmarks();
        }

        public void UpdateBookmarks()
        {

            
            Func<string,string> QuickDiscovery = (url) => {
                SiteDiscovery.Discover(url);
                return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\Data\Site\Cache\{SiteDiscovery.FindDomain(url)}.ico";
            };
            
            bookmarks.Clear();
            var tmp = MainWindow.GetBookmarks().Select(
                item => 
                new BookmarkItem()
                {
                    Link = item.Split(';')[1],
                    Name = item.Split(';')[0].Replace("\\","").Replace("\"", "").Replace("bookmark","").Replace("=",""),
                    Favicon = QuickDiscovery(item.Split(';')[1])
            }).ToList();

            foreach (var t in tmp)
                bookmarks.Add(t);
            bookmarksList.ItemsSource = bookmarks;


        }

        internal static void BookmarksRemove(string ID){
            if (MainWindow.DeleteBookmark(ID))
                bookmarks.Remove(bookmarks.First(item => item.Name.Equals(ID)));
        }

        internal static void BookmarkAdd(BookmarkItem @new){
            bookmarks.Add(@new);

        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(bookmarksList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                Owner.txtUrl.Text = ((BookmarkItem)item.Content).Link.ToString();
                Owner.GoToPage_Executed(this, null);
            }
 
        }
    }
}
