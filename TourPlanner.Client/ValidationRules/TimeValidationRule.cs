using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TourPlanner.Client.ValidationRules
{
    public class TimeValidationRule : ValidationRule
    {
        const string pattern = @"((\d*:\d{2})|(\d*)):\d{2}:\d{2}";
        public readonly Regex regex = new(pattern);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, "Value cannot be converted to string");

            return regex.IsMatch(strValue) ? new ValidationResult(true, null) : new ValidationResult(false, "Input has to be in format d*:hh:mm or h*:mm");
        }
    }
}
