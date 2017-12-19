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

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for ModuleItem.xaml
    /// </summary>
    public partial class ModuleItem : UserControl
    {
        public ModuleItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("AddonTitle", typeof(String), typeof(ModuleItem), new FrameworkPropertyMetadata(string.Empty));

        public String AddonTitle
        {
            get { return GetValue(TitleProperty)?.ToString(); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(String), typeof(ModuleItem), new FrameworkPropertyMetadata(string.Empty));

        public String ImageSource
        {
            get { return GetValue(ImageSourceProperty)?.ToString(); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(String), typeof(ModuleItem), new FrameworkPropertyMetadata(string.Empty));

        public String Description
        {
            get { return GetValue(DescriptionProperty)?.ToString(); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register("Details", typeof(String), typeof(ModuleItem), new FrameworkPropertyMetadata(string.Empty));

        public String Details
        {
            get { return GetValue(DetailsProperty)?.ToString(); }
            set { SetValue(DetailsProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(String), typeof(ModuleItem), new FrameworkPropertyMetadata(string.Empty));

        public String Target
        {
            get { return GetValue(TargetProperty)?.ToString(); }
            set { SetValue(TargetProperty, value); }
        }
    }
}
