using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSharper.Models
{
    public enum Complexity
    {
        easy,
        medium,
        hard,
        hardcore
    }
     public static class ComplexityExtensions
        {
            new public static string _ToString(this Complexity? complexity)
            {
                switch (complexity)
                {
                    case Complexity.easy:   return "Легкий";
                    case Complexity.medium: return "Средний";
                    case Complexity.hard:   return "Высокий";
                    case Complexity.hardcore: return "Продвинутый";
                }
                return null;
            }
        }
    public class ComplexityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Complexity?)._ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
