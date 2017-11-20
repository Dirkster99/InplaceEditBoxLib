namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;

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
            Name = "DummyKeyName";
        }
        #endregion constructors

        #region properties
        public string Name { get; set; }

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
        public IBaseItemModel AddRootChild(string itemName, SolutionModelItemType itemType)
        {
            return AddChild(itemName, itemType, Root);
        }

        public IBaseItemModel AddSolutionRootItem(string displayName)
        {
            Root = new SolutionRootItemModel(displayName);

            return Root;
        }

        public IBaseItemModel AddChild(string itemName
                                    , SolutionModelItemType itemType
                                    , IBaseItemChildrenModel parent)
        {
            return parent.AddChild(parent, itemName, itemType);
        }
        #endregion methods
    }
}
