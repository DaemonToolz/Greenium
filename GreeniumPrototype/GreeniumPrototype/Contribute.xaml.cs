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
    /// Interaction logic for Contribute.xaml
    /// </summary>
    public partial class Contribute : Page
    {
        public Contribute()
        {
            InitializeComponent();

            List<AddonItem> addons = new List<AddonItem>();
            for (int i = 0; i < 20; ++i)
            {
                addons.Add(new AddonItem()
                {
                    Description = "No description",
                    ImageSource = "./Assets/refresh.png",
                    AddonTitle = $"Title {i}",
                    Rating = (i % 10)
                });
            }

            Addons.ItemsSource = addons;
        }

    }
}
