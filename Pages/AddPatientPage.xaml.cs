using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PriemPatient;

namespace pract8.Pages
{
    public partial class AddPatientPage : Page
    {
        public User CurrentUser { get; set; } = new User();
        public Patient CurrentPatient { get; set; } = new Patient();

        public AddPatientPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
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

            int newPatientID = GenerateIdPatient();
            if (newPatientID == -1)
            {
                MessageBox.Show("Ошибка генерации ID");
                return;
            }

            var newPatient = new Patient
            {
                IdP = newPatientID,
                NameP = CurrentPatient.NameP,
                LastNameP = CurrentPatient.LastNameP,
                MiddleNameP = CurrentPatient.MiddleNameP,
                Birthday = CurrentPatient.Birthday,
                PhoneNumber = CurrentPatient.PhoneNumber
            };

            if (SavePatient(newPatient))
            {
                CurrentPatient.IdP = newPatient.IdP;
                CurrentPatient.NameP = newPatient.NameP;
                CurrentPatient.LastNameP = newPatient.LastNameP;
                CurrentPatient.MiddleNameP = newPatient.MiddleNameP;
                CurrentPatient.Birthday = newPatient.Birthday;
                CurrentPatient.PhoneNumber = newPatient.PhoneNumber;
                MessageBox.Show($"Пациент добавлен! ID: {newPatientID}");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении пациента");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private string GetPatient(int patientID)
        {
            return $"P_{patientID}.json";
        }

        private int GenerateIdPatient()
        {
            try
            {
                var rnd = new Random();
                for (int i = 0; i < 10000; i++)
                {
                    int id = rnd.Next(10000, 100000);
                    string filePath = GetPatient(id);
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

        private bool SavePatient(Patient patient)
        {
            try
            {
                string filePath = GetPatient(patient.IdP);
                string json = JsonConvert.SerializeObject(patient, Formatting.Indented);
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
