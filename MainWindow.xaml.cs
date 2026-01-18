using System.Windows;
using PriemPatient.Pages;

namespace PriemPatient
{
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; set; } = new User();

        public MainWindow()
        {
            InitializeComponent();
            Main.Navigate(new LoginPage());
        }

        private void MenuTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.Toggle();
        }
    }
}
