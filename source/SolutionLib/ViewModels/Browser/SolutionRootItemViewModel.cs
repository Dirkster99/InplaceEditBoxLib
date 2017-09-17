namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    /// <summary>
    /// Implements a viewmodel class for the first visible item in the treeview.
    /// Normally, there is only one root in any given tree - so this class implements
    /// that one item visually representing that root (eg.: Computer item in Windows Explorer).
    /// </summary>
    internal class SolutionRootItemViewModel : SolutionBaseItemViewModel, ISolutionRootItem
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        public SolutionRootItemViewModel(ISolutionBaseItem parent, string displayName)
            : base(parent, Models.SolutionItemType.SolutionRootItem)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected SolutionRootItemViewModel()
            : base(null, Models.SolutionItemType.SolutionRootItem)
        {
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Rename the display item of the root item.
        /// </summary>
        /// <param name="newName"></param>
        public void RenameRootItem(string newName)
        {
            SetDisplayName(newName);
        }

        /// <summary>
        /// Adds another folder (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem ISolutionItem.AddFolder(string displayName)
        {
            return AddChild(displayName, new FolderViewModel(this, displayName));
        }

        /// <summary>
        /// Adds another project (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem ISolutionItem.AddProject(string displayName)
        {
            return AddChild(displayName, new ProjectViewModel(this, displayName));
        }

        /// <summary>
        /// Adds another file (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem ISolutionItem.AddFile(string displayName)
        {
            return AddChild(displayName, new FileViewModel(this, displayName));
        }
        #endregion methods
    }
}
