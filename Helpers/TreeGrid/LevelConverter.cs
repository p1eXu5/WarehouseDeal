using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DirectoryView
{
    public class LevelConverter : DependencyObject, IMultiValueConverter
    {
        public object Convert(
            object[] values, Type targetType, 
            object parameter, CultureInfo culture)
        {
            int level = (int)values[0];
            double indent = (double)values[1];
            return indent * level;
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, 
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
