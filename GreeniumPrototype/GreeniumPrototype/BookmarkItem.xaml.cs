using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for BookmarkItem.xaml
    /// </summary>
    public partial class BookmarkItem : UserControl
    {

        public BookmarkItem()
        {
            InitializeComponent();
        }

        internal string Name { get; set; }

        public static readonly DependencyProperty LinkDependency = DependencyProperty.Register("Link", typeof(String), typeof(AddonItem), new FrameworkPropertyMetadata(string.Empty));

        public String Link
        {
            get { return GetValue(LinkDependency)?.ToString(); }
            set { SetValue(LinkDependency, value); }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            BookmarksPage.BookmarksRemove(Name);
        }

    }
}
