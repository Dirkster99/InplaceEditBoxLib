namespace SolutionModelsLib.Models.Base
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;

    /// <summary>
    /// Defines a model with properties and members of all objects displayed in a solution.
    /// </summary>
    public abstract class ItemModel : IItemModel
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemModel(IItemModel parent
                                , string displayName
                                , SolutionModelItemType itemType)
            : this()
        {
            Parent = parent;
            DisplayName = displayName;
            ItemType = itemType;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemModel(SolutionModelItemType itemType)
            : this()
        {
            ItemType = itemType;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemModel()
        {
            //            IsItemExpanded = false;
            //            IsItemSelected = false;

////            IsReadOnly = false;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets/Sets the Id for this item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets/sets the parent item of this item.
        /// </summary>
        public IItemModel Parent { get; set; }

        /// <summary>
        /// Gets/sets a technical type of this item to id the item
        /// in terms of its capabilities that are associated with this type of item.
        /// </summary>
        public SolutionModelItemType ItemType { get; set; }

        /// <summary>
        /// Gets/sets a name for display in UI.
        /// </summary>
        public string DisplayName { get; set; }

////        /// <summary>
////        /// Gets/sets whether the <see cref="DisplayName"/> of this treeview item
////        /// can be edit by the user or not.
////        /// </summary>
////        public bool IsReadOnly { get; set; }

        /*/ <summary>
        /// Gets a description of the item - for usage in tool tip etc..
        /// </summary>
        public string Description { get; set; } ***/

        ////        /// <summary>
        ////        /// Gets the parent object where this object is the child in the treeview.
        ////        /// </summary>
        ////        public SolutionItem_Model Parent { get; set; }

        ////        /// <summary>
        ////        /// Gets/sets whether this treeview item is expanded or not.
        ////        /// </summary>
        ////        public bool IsItemExpanded { get; set; }
        ////
        ////        /// <summary>
        ////        /// Gets/sets whether this treeview item is selected or not.
        ////        /// </summary>
        ////        public bool IsItemSelected { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Returns the string path either:
        /// 1) for the <paramref name="current"/> item or
        /// 2) for this item (if optional parameter <paramref name="current"/> is not set).
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public string GetStackPath(IItemModel current = null)
        {
            if (current == null)
                current = this;

            string result = string.Empty;

            // Traverse the list of parents backwards and
            // add each child to the path
            while (current != null)
            {
                result = "/" + current.DisplayName + result;

                current = current.Parent;
            }

            return result;
        }
        #endregion methods
    }
}
