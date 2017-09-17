namespace SolutionLib.ViewModels.Collections
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System.Collections.Generic;

    internal class SortableObservableDictionaryCollection : SortableObservableCollection<ISolutionBaseItem>
    {
        Dictionary<string, ISolutionBaseItem> _dictionary = null;

        #region constructors
        public SortableObservableDictionaryCollection()
        {
            _dictionary = new Dictionary<string, ISolutionBaseItem>();
        }
        #endregion constructors

        #region methods
        public bool AddItem(ISolutionBaseItem item)
        {
            item.SortKey = GenSortKey(item);
            _dictionary.Add(item.DisplayName, item);
            this.Add(item);

            return true;
        }

        public bool RemoveItem(ISolutionBaseItem item)
        {
            item.SortKey = GenSortKey(item);

            _dictionary.Remove(item.DisplayName);
            this.Remove(item);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ISolutionBaseItem TryGet(string key)
        {
            ISolutionBaseItem o;

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
        public void RenameItem(ISolutionBaseItem item, string newName)
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
        public string GenSortKey(ISolutionBaseItem item)
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
