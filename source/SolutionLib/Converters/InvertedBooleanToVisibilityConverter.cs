namespace SolutionLib.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// XAML converter to convert <seealso cref="Boolean"/> values
    /// into <seealso cref="Visibility"/> output values.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                return null;

            bool boolValue = (bool)value;

            if (boolValue)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
