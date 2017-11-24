namespace SolutionLib.Converters
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// XAML converter to convert <seealso cref="SolutionItemType"/> enum members
    /// into <seealso cref="ImageSource"/> from ResourceDictionary or fallback from
    /// static resource.
    /// </summary>
    [ValueConversion(typeof(IItem), typeof(ImageSource))]
    public class ISolutionBaseItemToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as IItem;

            if (item == null)
                return Binding.DoNothing;

            return GetImage(item.ItemType, item.IsItemExpanded);
        }

        /// <summary>
        /// Converts a value.
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


        /// <summary>
        /// Get a DynamicResource from ResourceDictionary or a static ImageSource (as fallback) for not expanded folder item.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="isItemExpanded"></param>
        /// <returns></returns>
        private object GetImage(SolutionItemType itemType, bool isItemExpanded)
        {
            string uriPath = null;

            switch (itemType)
            {
                case Models.SolutionItemType.SolutionRootItem:
                    uriPath = "SolutionIcon";
                    break;

                case Models.SolutionItemType.File:
                    uriPath = "FileIcon";
                    break;

                case Models.SolutionItemType.Folder:
                    if (isItemExpanded == true)
                        uriPath = "FolderIconOpen";
                    else
                        uriPath = "FolderIcon";
                    break;

                case Models.SolutionItemType.Project:
                    uriPath = "ProjectIcon";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(itemType.ToString());
            }

            object item = null;

            // Find Icon source in ResouceDictionary and return it
            if (uriPath != null)
            {
                item = Application.Current.Resources[uriPath];

                if (item != null)
                    return item;
            }

            // item was not available in resourcedictionary, so as a fallback
            // we try a static reference since that should be better than returning null
            string pathValue = null;

            switch (itemType)
            {
                case Models.SolutionItemType.SolutionRootItem:
                    pathValue = "pack://application:,,,/SolutionLib;component/Resources/Solutions/Light/AppFlyout_16x.png";
                    break;

                case Models.SolutionItemType.File:
                    uriPath = "pack://application:,,,/SolutionLib;component/Resources/Files/Light/Document_16x.png";
                    break;

                case Models.SolutionItemType.Folder:
                    if (isItemExpanded == true)
                        uriPath = "pack://application:,,,/SolutionLib;component/Resources/Folders/Light/FolderOpen_32x.png";
                    else
                        uriPath = "pack://application:,,,/SolutionLib;component/Resources/Folders/Light/Folder_32x.png";
                    break;

                case Models.SolutionItemType.Project:
                    uriPath = "pack://application:,,,/SolutionLib;component/Resources/Projects/Light/Application_16x.png";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(itemType.ToString());
            }

            if (pathValue != null)
            {
                try
                {
                    Uri imagePath = new Uri(pathValue, UriKind.RelativeOrAbsolute);
                    ImageSource source = new System.Windows.Media.Imaging.BitmapImage(imagePath);

                    return source;
                }
                catch
                {
                }
            }

            // Attempt to load fallback folder from ResourceDictionary
            item = Application.Current.Resources["FolderIcon"];

            if (item != null)
                return item;
            else
            {
                // Attempt to load fallback folder from fixed Uri
                pathValue = "pack://application:,,,/SolutionLib;component/Resources/Folders/Light/Folder_32x.png";

                try
                {
                    Uri imagePath = new Uri(pathValue, UriKind.RelativeOrAbsolute);
                    ImageSource source = new System.Windows.Media.Imaging.BitmapImage(imagePath);

                    return source;
                }
                catch
                {
                }
            }

            return null;
        }

    }
}
