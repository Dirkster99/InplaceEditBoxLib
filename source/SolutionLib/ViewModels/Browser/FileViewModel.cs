namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    /// <summary>
    /// Implements a viewmodel for file items in a tree structured viewmodel collection.
    /// </summary>
    internal class FileViewModel : SolutionBaseItemViewModel, IFile
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public FileViewModel(ISolutionBaseItem parent, string displayName)
            : base(parent, Models.SolutionItemType.File)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected FileViewModel()
           : base(null, Models.SolutionItemType.File)
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
