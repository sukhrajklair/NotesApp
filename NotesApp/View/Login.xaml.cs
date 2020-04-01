using NotesApp.ViewModel;
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
using System.Windows.Shapes;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            LoginVM vm = new LoginVM();
            containerGrid.DataContext = vm;
            App.HasLoggedIn += App_HasLoggedIn;
        }

        private void App_HasLoggedIn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExistingAccountButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterStackPanel.Visibility = Visibility.Collapsed;
            LoginStackPanel.Visibility = Visibility.Visible;
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterStackPanel.Visibility = Visibility.Visible;
            LoginStackPanel.Visibility = Visibility.Collapsed;
        }
    }
}
