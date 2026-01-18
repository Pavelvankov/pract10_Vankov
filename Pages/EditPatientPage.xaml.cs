using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PriemPatient;

namespace pract8.Pages
{
    public partial class EditPatientPage : Page
    {
        public Patient CurrentPatient { get; set; } = new Patient();

        private User _currentUser;

        public EditPatientPage(Patient patient, User currentUser)
        {
            InitializeComponent();
            CurrentPatient = patient;
            _currentUser = currentUser;
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPatient.NameP) ||
                string.IsNullOrWhiteSpace(CurrentPatient.LastNameP) ||
                string.IsNullOrWhiteSpace(CurrentPatient.MiddleNameP) ||
                string.IsNullOrWhiteSpace(CurrentPatient.Birthday) ||
                string.IsNullOrWhiteSpace(CurrentPatient.PhoneNumber))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            try
            {
                string filePath = $"P_{CurrentPatient.IdP}.json";
                string json = JsonConvert.SerializeObject(CurrentPatient, Formatting.Indented);
                File.WriteAllText(filePath, json);
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
