namespace InPlaceEditBoxDemo.Converters
{
    using SolutionLib.Models;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// XAML converter to convert <seealso cref="SolutionItemType"/> enum members
    /// into <seealso cref="ImageSource"/> from ResourceDictionary or fallback from
    /// static resource.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class ItemTypeDisplayNameToTextConverter : IMultiValueConverter
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

            var item = values[0] as string;

            if (item == null)
                return Binding.DoNothing;

            if (values[1] is SolutionItemType == false)
                return Binding.DoNothing;

            var itemType = (SolutionItemType)values[1];

            string itemTypeText = string.Empty;

            switch (itemType)
            {
                case SolutionItemType.SolutionRootItem:
                    itemTypeText = "Solution";
                    break;
                case SolutionItemType.File:
                    itemTypeText = "File";
                    break;
                case SolutionItemType.Folder:
                    itemTypeText = "Folder";
                    break;

                case SolutionItemType.Project:
                    itemTypeText = "Project";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(itemType.ToString());
            }

            return string.Format("{0} ({1})", item, itemTypeText);
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
