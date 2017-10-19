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
using GreeniumCore;
using GreeniumCore.Mail;

namespace GreeniumPrototype
{
    /// <summary>
    /// Interaction logic for Emails.xaml
    /// </summary>
    public partial class Emails : Page
    {
        private static Dictionary<string, string[]> AvailableEngines = new Dictionary<string, string[]>();
        private static List<IGenericMailConnector> Mails = new List<IGenericMailConnector>();

        static Emails()
        {
            AvailableEngines.Add("Google Mail", new string[] {"fr", "com"});
            AvailableEngines.Add("Microsoft Exchange", new string[] { "com" });

        }

        public Emails(){
            InitializeComponent();
            EngineComboBox.ItemsSource = AvailableEngines.Keys;

        }

        private void AddMail_Click(object sender, RoutedEventArgs e)
        {
      
            Mails.Add(new Outlook(MailUsername.Text + EngineLabel.Content + ExtensionSelector.SelectedItem.ToString(), MailPassword.Password, "TryIt"));

            var generatedGrid = new Grid() { Width = 75 };
            generatedGrid.Children.Add(new Label() { Content = "Email", HorizontalAlignment = HorizontalAlignment.Left });
            var interaction = new Button() { Content = "X", Width = 20, Height = 20, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right };
            generatedGrid.Children.Add(interaction);

            Grid innerGrid = new Grid();

            Mails[0].Connect();
            innerGrid.Children.Add(new Label()
            {
                Content = $"{Mails[0].GetWeight()}g de CO² pour votre compte"
            });
            Mails[0].Disconnect();


            MailControls.Items.Add(new TabItem()
            {
                Header = generatedGrid,
                Content = innerGrid,

            });

            
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EngineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            ExtensionSelector.ItemsSource = AvailableEngines[combobox.SelectedItem.ToString()];
            switch (combobox.SelectedIndex)
            {
                case 0:
                    EngineLabel.Content = "@gmail";
                    break;
                case 1:
                    EngineLabel.Content = "@outlook";
                    break;
            }
        }
    }
}
