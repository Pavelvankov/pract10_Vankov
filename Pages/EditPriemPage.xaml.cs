using Newtonsoft.Json;
using PriemPatient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace pract8.Pages
{
    public partial class EditPriemPage : Page
    {
        public Patient CurrentPatient { get; set; } = new Patient();

        private User _currentUser;

        public EditPriemPage(Patient patient, User currentUser)
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
                string.IsNullOrWhiteSpace(CurrentPatient.PhoneNumber) ||
                string.IsNullOrWhiteSpace(CurrentPatient.LastAppointment) ||
                string.IsNullOrWhiteSpace(DiagnosisBox.Text) ||
                string.IsNullOrWhiteSpace(RecommendadionBox.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            try
            {
                var newAppointment = new Appointment
                {
                    Date = CurrentPatient.LastAppointment,
                    DoctorId = _currentUser.Id,
                    Diagnosis = DiagnosisBox.Text.Trim(),
                    Recomendations = RecommendadionBox.Text.Trim()
                };

                Appointment[] currentStories;
                if (CurrentPatient.AppointmentStories == null)
                    currentStories = new Appointment[0];
                else
                    currentStories = CurrentPatient.AppointmentStories;

                var list = new List<Appointment>(currentStories);
                list.Add(newAppointment);
                CurrentPatient.AppointmentStories = list.ToArray();

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
