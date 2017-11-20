namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class FolderViewModel : BaseItemChildrenViewModel, IFolder
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public FolderViewModel(IBaseItem parent, string displayName)
           : base(parent, Models.SolutionItemType.Folder)
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
