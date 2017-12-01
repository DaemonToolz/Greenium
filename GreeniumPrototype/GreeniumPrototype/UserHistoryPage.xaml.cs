using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Linq;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for UserHistoryPage.xaml
    /// </summary>
    public partial class UserHistoryPage : Page
    {
        private static ObservableCollection<TreeViewItem> HistoryLinks = new ObservableCollection<TreeViewItem>();
        public MainWindow Owner { get; set; }

        public UserHistoryPage()
        {
            InitializeComponent();
        }

        public void UpdateHistory()
        {
            HistoryLinks.Clear();
            var History = MainWindow.HistoryEngine.Read("History");
            foreach (var hist in History)
            {
                if (HistoryLinks.Any(obj => obj.Header.Equals($"{hist.Time:dd/MM/yyyy}")))
                    HistoryLinks.First(obj => obj.Header.Equals($"{hist.Time:dd/MM/yyyy}")).Items
                        .Add(new TreeViewItem() {Header = hist.URL});
                else
                {
                    var temporary = new TreeViewItem() { Header = $"{hist.Time:dd/MM/yyyy}"};
                    temporary.Items.Add(new TreeViewItem(){Header = hist.URL});
                    HistoryLinks.Add(temporary);
                }
            }

            HistoryTree.ItemsSource = HistoryLinks;
        }
    }
}
