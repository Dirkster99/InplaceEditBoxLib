namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using System;

    /// <summary>
    /// A Solution root is the class that hosts all other solution related items.
    /// Even the SolutionRootItem that is part of the displayed collection is hosted in
    /// the collection below.
    /// </summary>
    public class SolutionModel : ISolutionModel
    {
        #region constructors
        public SolutionModel()
        {
////            Name = "DummyKeyName";
        }
        #endregion constructors

        #region properties
////        public string Name { get; set; }

        /// <summary>
        /// Gets the root of the treeview. That is, there is only
        /// 1 item in the ObservableCollection and that item is the root.
        /// 
        /// The Children property of that one <see cref="SolutionRootItemModel"/>
        /// represent the rest of the tree.
        /// </summary>
        public ISolutionRootItemModel Root { get; set; }
        #endregion properties

        #region methods
////        public IBaseItemModel AddRootChild(string itemName, SolutionModelItemType itemType)
////        {
////            return AddChild(itemName, itemType, Root);
////        }

        public IItemModel AddSolutionRootItem(string displayName, long id = -1)
        {
            Root = new SolutionRootItemModel(displayName) { Id = id };

            return Root;
        }

        public IItemChildrenModel AddSolutionRootItem(string displayName)
        {
            Root = new SolutionRootItemModel(displayName);

            return Root;
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItemModel AddChild(string displayName
                                     , SolutionModelItemType type
                                     , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            IItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Project:
                    newItem = new ProjectItemModel(parent, displayName);
                    break;

                // This should be created via AddSolutionRootItem() method
                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new ArgumentException(type.ToString());
            }

            parent.AddChild(newItem);

            return newItem;
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// 
        /// This wrapper uses a long input for conversion when reading from file.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal IItemModel AddChild(string displayName
                                       , long longType
                                       , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            SolutionModelItemType type = (SolutionModelItemType)longType;

            return AddChild(displayName, type, parent);
        }
        #endregion methods
    }
}
