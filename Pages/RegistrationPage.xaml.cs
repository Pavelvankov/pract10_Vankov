using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace PriemPatient.Pages
{
    public partial class RegistrationPage : Page
    {
        public User CurrentUser { get; set; } = new User();
        public string ConfirmPassword { get; set; } = "";

        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Name) ||
                string.IsNullOrWhiteSpace(CurrentUser.LastName) ||
                string.IsNullOrWhiteSpace(CurrentUser.MiddleName) ||
                string.IsNullOrWhiteSpace(CurrentUser.Specialisation) ||
                string.IsNullOrWhiteSpace(CurrentUser.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            if (ConfirmPassword != CurrentUser.Password)
            {
                MessageBox.Show("Пароль должен совпадать");
                return;
            }

            int newUserID = GenerateId();
            if (newUserID == -1)
            {
                MessageBox.Show("Ошибка генерации ID");
                return;
            }

            var newUser = new User
            {
                Id = newUserID,
                Name = CurrentUser.Name,
                LastName = CurrentUser.LastName,
                MiddleName = CurrentUser.MiddleName,
                Specialisation = CurrentUser.Specialisation,
                Password = CurrentUser.Password
            };

            if (SaveUser(newUser))
            {
                CurrentUser.Id = newUser.Id;
                MessageBox.Show($"Регистрация успешна! Ваш ID: {newUserID}");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении пользователя");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private int GenerateId()
        {
            try
            {
                var rnd = new Random();
                for (int i = 0; i < 1000; i++)
                {
                    int id = rnd.Next(1000, 10000);
                    string filePath = GetUser(id);
                    if (!File.Exists(filePath))
                        return id;
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }

        private string GetUser(int userID)
        {
            return $"D_{userID}.json";
        }

        private bool SaveUser(User user)
        {
            try
            {
                string filePath = GetUser(user.Id);
                string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
