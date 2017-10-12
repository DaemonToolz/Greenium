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
    /// Interaction logic for AddonItem.xaml
    /// </summary>
    public partial class AddonItem : UserControl {
        public AddonItem() {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("AddonTitle", typeof(String), typeof(AddonItem), new FrameworkPropertyMetadata(string.Empty));

        public String AddonTitle
        {
            get { return GetValue(TitleProperty)?.ToString(); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(String), typeof(AddonItem), new FrameworkPropertyMetadata(string.Empty));

        public String ImageSource
        {
            get { return GetValue(ImageSourceProperty)?.ToString(); }
            set { SetValue(ImageSourceProperty, value); }
        }
    


        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(Int32), typeof(AddonItem), new FrameworkPropertyMetadata(0));

        public Int32 Rating{
            get{
                return int.Parse(GetValue(RatingProperty)?.ToString());
            }

            set{
                SetValue(RatingProperty, value);
            }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(String), typeof(AddonItem), new FrameworkPropertyMetadata(string.Empty));

        public String Description
        {
            get { return GetValue(DescriptionProperty)?.ToString(); }
            set { SetValue(DescriptionProperty, value); }
        }
    }
}
