using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PriemPatient.Pages
{
    public partial class LoginPage : Page
    {
        public User CurrentUser { get; set; } = new User();
        public string LogIdText { get; set; } = "";
        public string LogPassText { get; set; } = "";

        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LogIdText) || string.IsNullOrWhiteSpace(LogPassText))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            if (!int.TryParse(LogIdText, out int userId))
            {
                MessageBox.Show("ID должен быть числом");
                return;
            }

            string filePath = GetUser(userId);
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Пользователь с таким ID не найден");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var user = JsonConvert.DeserializeObject<User>(json);
                if (user != null && user.Password == LogPassText)
                {
                    CurrentUser.Id = user.Id;
                    CurrentUser.Name = user.Name;
                    CurrentUser.LastName = user.LastName;
                    CurrentUser.MiddleName = user.MiddleName;
                    CurrentUser.Specialisation = user.Specialisation;
                    CurrentUser.Password = user.Password;
                    NavigationService.Navigate(new MainPage(user));
                }
                else
                {
                    MessageBox.Show("Неверный пароль");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке пользователя");
            }
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private string GetUser(int userID)
        {
            return $"D_{userID}.json";
        }
    }
}
