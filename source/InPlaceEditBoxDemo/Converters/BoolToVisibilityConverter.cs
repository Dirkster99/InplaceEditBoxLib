namespace InPlaceEditBoxDemo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts from boolean true or false to <see cref="Visibility"/> as defined in
    /// <see cref="True"/> and <see cref="False"/> properties of this object.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
        {
            this.True = Visibility.Visible;
            this.False = Visibility.Collapsed;
        }

        /// <summary>
        /// Converts from boolean true or false to <see cref="Visibility"/> as defined in
        /// <see cref="True"/> and <see cref="False"/> properties of this object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if (value is bool == false)
                return Binding.DoNothing;

            bool input = (bool)value;

            if (input == true)
                return True;

            return False;
        }

        /// <summary>
        /// Gets/sets the <see cref="Visibility"/> value that is associated with boolean true.
        /// </summary>
        public Visibility True { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="Visibility"/> value that is associated with boolean false.
        /// </summary>
        public Visibility False { get; set; }

        /// <summary>
        /// Throws a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
