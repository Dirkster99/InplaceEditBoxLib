namespace SolutionLib.ViewModels.Collections
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Implements a custom observable collection that can host items that are typed
    /// through <see cref="SolutionItemType"/> - this items are sorted and kept unique
    /// to resamble a similar structure as in the Solution Explorer of Visual Studio.
    /// </summary>
    internal class SortableObservableDictionaryCollection : SortableObservableCollection<IItem>
    {
        #region fields
        private Dictionary<string, IItem> _dictionary = null;
        private static DispatcherPriority _ChildrenEditPrio = DispatcherPriority.DataBind;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SortableObservableDictionaryCollection()
        {
            _dictionary = new Dictionary<string, IItem>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Adds a new item into the collection of items hosted.
        /// The method throws an exception if the key of the new
        /// item is already present in the current collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(IItem item)
        {
            item.SortKey = GenSortKey(item);
            _dictionary.Add(item.DisplayName, item);

            Application.Current.Dispatcher.Invoke(() => { base.Add(item); }, _ChildrenEditPrio);

            return true;
        }

        /// <summary>
        /// Adds a new item into the collection of items hosted.
        /// The method throws an exception if the key of the new
        /// item is already present in the current collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Add(IItem item)
        {
            return AddItem(item);
        }

        /// <summary>
        /// Removes an item from the collection of items hosted.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(IItem item)
        {
            item.SortKey = GenSortKey(item);

            _dictionary.Remove(item.DisplayName);
            
            Application.Current.Dispatcher.Invoke(() => { base.Remove(item); }, _ChildrenEditPrio);

            return true;
        }

        /// <summary>
        /// Removes an item from the collection of items hosted.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Remove(IItem item)
        {
            return RemoveItem(item);
        }

        /// <summary>
        /// Removes all items in the collection.
        /// </summary>
        public new void Clear()
        {
            _dictionary.Clear();
            Application.Current.Dispatcher.Invoke(() => { base.Clear(); }, _ChildrenEditPrio);
        }

        /// <summary>
        /// Attempts to find an item in the internal dictionary and returns it or
        /// returns null if item was not available.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IItem TryGet(string key)
        {
            IItem o;

            if (_dictionary.TryGetValue(key, out o))
                return o;

            return null;
        }

        /// <summary>
        /// Renames an item in the collection and also adjusts its sortkey
        /// to make sure that the renamed item re-appears in its proper place.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        public void RenameItem(IItem item, string newName)
        {
            _dictionary.Remove(item.DisplayName);
            item.SetDisplayName(newName);
            
            item.SortKey = GenSortKey(item);
            _dictionary.Add(newName, item);
        }

        /// <summary>
        /// Sorts the items in thie collection when requested to do so.
        /// </summary>
        public void SortItems()
        {
            this.Sort(item => item.SortKey);
        }

        /// <summary>
        /// This will generate a sort key fit for sorting
        /// (not for unique identification of an item)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GenSortKey(IItem item)
        {
            string key = item.DisplayName;
            SolutionItemType itemType = item.ItemType;

            // Compute a prefix to establish a group, sort order to diplay items in:
            // Projects (group sorted within alpabetically)
            // Folders  (group sorted within alpabetically)
            // Files    (group sorted within alpabetically)
            string prefix = "";
            switch (itemType)
            {
                case SolutionItemType.SolutionRootItem:
                default:
                    prefix = "000_";
                    break;

                case SolutionItemType.Folder:
                    prefix = "222_";
                    break;

                case SolutionItemType.Project:
                    prefix = "444_";
                    break;

                case SolutionItemType.File:
                    prefix = "666_";
                    break;

            }

            return prefix + key;
        }
        #endregion methods
    }
}
