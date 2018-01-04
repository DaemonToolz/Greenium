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
using CefSharp;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for DownloadListItem.xaml
    /// </summary>
    public partial class DownloadListItem : UserControl {
        public DownloadListItem() {
            InitializeComponent();
            this.DataContext = this;
        }

        public DownloadItem MyLink { get; set; }
        
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register("Filename", typeof(String), typeof(DownloadListItem), new FrameworkPropertyMetadata(string.Empty));

        public String Filename
        {
            get { return GetValue(FilenameProperty)?.ToString(); }
            set { SetValue(FilenameProperty, value); }
        }

        public static readonly DependencyProperty PercentileProperty = DependencyProperty.Register("Percentile", typeof(Double), typeof(DownloadListItem), new FrameworkPropertyMetadata(0.0));

        public Double Percentile
        {
            get { return Double.Parse(GetValue(PercentileProperty)?.ToString()); }
            set { SetValue(PercentileProperty, value); }
        }

        public static readonly DependencyProperty CurrentSizeProperty = DependencyProperty.Register("CurrentSize", typeof(Double), typeof(DownloadListItem), new FrameworkPropertyMetadata(0.0));

        public Double CurrentSize
        {
            get { return Double.Parse(GetValue(CurrentSizeProperty)?.ToString()); }
            set { SetValue(CurrentSizeProperty, value); }
        }

        public static readonly DependencyProperty RealSizeProperty = DependencyProperty.Register("RealSize", typeof(Double), typeof(DownloadListItem), new FrameworkPropertyMetadata(0.0));

        public Double RealSize
        {
            get { return Double.Parse(GetValue(RealSizeProperty)?.ToString()); }
            set { SetValue(RealSizeProperty, value); }
        }
    }
}
