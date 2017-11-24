namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;

    /// <summary>
    /// Implements an interface for a model class of the first visible item in the treeview.
    /// 
    /// Normally, there is only one root in any given tree - so this class implements
    /// that one item visually representing that root (eg.: Computer item in Windows Explorer).
    /// </summary>
    public class SolutionRootItemModel : ItemChildrenModel, ISolutionRootItemModel
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        public SolutionRootItemModel(string displayName)
            : base(Enums.SolutionModelItemType.SolutionRootItem)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected SolutionRootItemModel()
            : base(Enums.SolutionModelItemType.SolutionRootItem)
        {
        }
        #endregion constructors
    }
}
