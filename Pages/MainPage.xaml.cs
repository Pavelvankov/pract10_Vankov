using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using pract8.Pages;

namespace PriemPatient.Pages
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _statusBarText = "Всего файлов: 0 | Пользователей: 0 | Пациентов: 0";
        public string StatusBarText
        {
            get => _statusBarText;
            set { _statusBarText = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateFileCounts()
        {
            try
            {
                int users = Directory.GetFiles(".", "D_*.json").Length;
                int patients = Directory.GetFiles(".", "P_*.json").Length;
                int total = users + patients;
                StatusBarText = $"Всего файлов: {total} | Пользователей: {users} | Пациентов: {patients}";
            }
            catch
            {
                StatusBarText = "Ошибка подсчёта файлов";
            }
        }

        public User CurrentUser { get; set; } = new User();
        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        private User _currentUser;
        public Patient? SelectedUser { get; set; }

        public MainPage(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            DataContext = this;
            LoadUserInfo();
            LoadPatientInfo();
        }

        private void LoadUserInfo()
        {
            string filePath = GetUser(_currentUser.Id);
            try
            {
                string json = File.ReadAllText(filePath);
                var user = JsonConvert.DeserializeObject<User>(json);
                if (user != null)
                {
                    CurrentUser.Id = user.Id;
                    CurrentUser.Name = user.Name;
                    CurrentUser.LastName = user.LastName;
                    CurrentUser.MiddleName = user.MiddleName;
                    CurrentUser.Specialisation = user.Specialisation;
                    CurrentUser.Password = user.Password;
                }
                UpdateFileCounts();
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке пользователя");
            }
        }

        private void LoadPatientInfo()
        {
            Patients.Clear();
            string[] allFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "P_*.json");
            for (int i = 0; i < allFiles.Length; i++)
            {
                string filePath = allFiles[i];
                try
                {
                    string json = File.ReadAllText(filePath);
                    Patient patient = JsonConvert.DeserializeObject<Patient>(json);
                    if (patient != null)
                    {
                        Patients.Add(patient);
                        UpdateFileCounts();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки пациента: {ex.Message}");
                }
            }
        }

        private void CreatePatient_btn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPatientPage());
        }

        private void Priem_btn(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }

            NavigationService.Navigate(new PriemPage(SelectedUser, _currentUser));
        }

        private void EditPatient_btn(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }

            NavigationService.Navigate(new EditPatientPage(SelectedUser, _currentUser));
        }

        private void DeletePatient_btn(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }

            try
            {
                string filePath = $"P_{SelectedUser.IdP}.json";
                if (File.Exists(filePath))
                    File.Delete(filePath);

                Patients.Remove(SelectedUser);
                UpdateFileCounts();
                MessageBox.Show("Пациент удален");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        private string GetUser(int userID)
        {
            return $"D_{userID}.json";
        }

        private void ResetUser(object sender, RoutedEventArgs e)
        {
            CurrentUser.Id = 0;
            CurrentUser.Name = "";
            CurrentUser.LastName = "";
            CurrentUser.MiddleName = "";
            CurrentUser.Specialisation = "";
            CurrentUser.Password = "";
            NavigationService.GoBack();
        }
    }
}
