using System.Globalization;
using System.Windows.Controls;

namespace PriemPatient.ValidationRules
{
    public class InputValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();
            if (input == string.Empty)
            {
                return new ValidationResult(false, "Поле обязательно для заполнения");
            }

            return ValidationResult.ValidResult;
        }
    }
}
