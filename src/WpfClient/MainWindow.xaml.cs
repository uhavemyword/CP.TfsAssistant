using CP.TfsAssistant.Libraires;
using System.Collections.Generic;
using System.Windows;

namespace CP.TfsAssistant.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenSettingsForm();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenWorkItemForm();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var fields = new Dictionary<string, object>();
            fields.Add("System.Description", "Test <br />description");
            UIHelper.OpenWorkItemForm("Task", "My Title", fields);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(this.IdTextBox.Text);
            UIHelper.OpenWorkItemForm(id);
        }
    }
}
