using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KGResourceTracking.KingdomAP
{
    public class IntegerLessThanZeroValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var toString = value.ToString();
            if (string.IsNullOrEmpty(toString))
                return new ValidationResult(false, "Invalid number");
            
            return int.TryParse(toString,out int result) ? result >= 0 ? ValidationResult.ValidResult : new ValidationResult(false, "Invalid number") : new ValidationResult(false, "Invalid number");
        }
    }
}
