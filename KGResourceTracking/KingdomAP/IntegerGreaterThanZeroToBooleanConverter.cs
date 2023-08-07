using System.Windows.Data;

namespace KGResourceTracking.KingdomAP
{
    public class IntegerGreaterThanZeroToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int val)
            {
                if (val > 0)
                    return false;
            }
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
