namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class FolderViewModel : Base.ItemChildrenViewModel, IFolder
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        /// <param name="addDummyChild"></param>
        public FolderViewModel(IItem parent
                             , string displayName
                             , bool addDummyChild = true)
           : base(parent, Models.SolutionItemType.Folder, addDummyChild)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected FolderViewModel()
           : base(null, Models.SolutionItemType.Folder)
        {
        }
        #endregion constructors
    }
}
