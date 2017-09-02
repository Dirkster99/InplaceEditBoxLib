namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class FolderViewModel : SolutionBaseItemViewModel, IFolder
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public FolderViewModel(ISolutionBaseItem parent, string displayName)
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

        #region methods
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
