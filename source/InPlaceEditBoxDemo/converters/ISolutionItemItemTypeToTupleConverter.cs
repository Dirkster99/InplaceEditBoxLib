namespace InPlaceEditBoxDemo.Converters
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// XAML converter to convert <seealso cref="IItemChildren"/> and
    /// <seealso cref="SolutionItemType"/> enum members
    /// into a <seealso cref="Tuple{T1, T2}"/> representaton for command binding.
    /// </summary>
    [ValueConversion(typeof(IItemChildren), typeof(Tuple<IItemChildren, SolutionItemType>))]
    [ValueConversion(typeof(SolutionItemType), typeof(Tuple<IItemChildren, SolutionItemType>))]
    public class ISolutionItemItemTypeToTupleConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return Binding.DoNothing;

            if (values.Length != 2)
                return Binding.DoNothing;

            var item = values[0] as IItemChildren;

            if (item == null)
                return Binding.DoNothing;

            if (values[1] is SolutionItemType == false)
                return Binding.DoNothing;

            var itemType = (SolutionItemType)values[1];

            return new Tuple<IItemChildren, SolutionItemType>(item, itemType);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
