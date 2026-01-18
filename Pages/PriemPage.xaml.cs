using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PriemPatient;

namespace pract8.Pages
{
    public partial class PriemPage : Page
    {
        public Patient CurrentPatient { get; set; } = new Patient();
        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        private User _currentUser;

        public PriemPage(Patient patient, User currentUser)
        {
            InitializeComponent();
            CurrentPatient = patient;
            _currentUser = currentUser;
            DataContext = this;
        }

        private void EditPatientButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPriemPage(CurrentPatient, _currentUser));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
