using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace PriemPatient
{
    public class Patient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private int _idP = 0;
        public int IdP
        {
            get => _idP;
            set
            {
                if (_idP != value)
                {
                    _idP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _nameP = "";
        public string NameP
        {
            get => _nameP;
            set
            {
                if (_nameP != value)
                {
                    _nameP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lastnameP = "";
        public string LastNameP
        {
            get => _lastnameP;
            set
            {
                if (_lastnameP != value)
                {
                    _lastnameP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _middlenameP = "";
        public string MiddleNameP
        {
            get => _middlenameP;
            set
            {
                if (_middlenameP != value)
                {
                    _middlenameP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lastAppointment = "";
        public string LastAppointment
        {
            get => _lastAppointment;
            set
            {
                if (_lastAppointment != value)
                {
                    _lastAppointment = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _birthday = "";
        public string Birthday
        {
            get => _birthday;
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(AdultStatus));
                }
            }
        }
        private string _diagnosisP = "";
        public string DiagnosisP
        {
            get => _diagnosisP;
            set
            {
                if (_diagnosisP != value)
                {
                    _diagnosisP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _recomendationsP = "";
        public string RecomendationsP
        {
            get => _recomendationsP;
            set
            {
                if (_recomendationsP != value)
                {
                    _recomendationsP = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PhoneNumberDigits));
            }
        }

        [JsonIgnore]
        public long? PhoneNumberDigits
        {
            get
            {
                string digits = "";
                for (int i = 0; i < (_phoneNumber ?? "").Length; i++)
                {
                    char c = _phoneNumber[i];
                    if (char.IsDigit(c))
                        digits += c;
                }

                if (digits.Length == 11 && (digits[0] == '8' || digits[0] == '7'))
                    digits = digits.Substring(1);

                if (digits.Length != 10)
                    return null;

                if (long.TryParse(digits, out long result))
                    return result;

                return null;
            }
        }

        [JsonIgnore]
        public int Age
        {
            get
            {
                if (!DateTime.TryParse(Birthday, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateTime bday))
                    return 0;

                DateTime today = DateTime.Today;
                int age = today.Year - bday.Year;
                if (bday.Date > today.AddYears(-age))
                    age--;

                if (age < 0)
                    age = 0;

                return age;
            }
        }

        [JsonIgnore]
        public string AdultStatus
        {
            get
            {
                return Age >= 18 ? "Совершеннолетний" : "Несовершеннолетний";
            }
        }

        private Appointment[] _appointmentStories = new Appointment[0];
        public Appointment[] AppointmentStories
        {
            get => _appointmentStories;
            set
            { 
                _appointmentStories = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(DaysFromPreviousAppointmentText));
            }
        }

        [JsonIgnore]
        public string DaysFromPreviousAppointmentText
        {
            get
            {
                if (AppointmentStories == null || AppointmentStories.Length == 0)
                    return "Первый прием в клинике";

                DateTime? last = null;
                for (int i = 0; i < AppointmentStories.Length; i++)
                {
                    var a = AppointmentStories[i];
                    if (a == null) continue;

                    if (DateTime.TryParse(a.Date, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateTime dt))
                    {
                        if (last == null || dt.Date > last.Value.Date)
                            last = dt.Date;
                    }
                }

                if (last == null)
                    return "Первый прием в клинике";

                int days = (DateTime.Today - last.Value.Date).Days;
                if (days < 0) days = 0;
                return $"Дней с предыдущего приема: {days}";
            }
        }
    }

    public class Appointment
    {
        public string Date { get; set; } = "";

        public int DoctorId { get; set; }

        public string Diagnosis { get; set; } = "";
        public string Recomendations { get; set; } = "";
    }
}
