namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    /// <summary>
    /// Implements a viewmodel for file items in a tree structured viewmodel collection.
    /// </summary>
    internal class FileViewModel : BaseItemViewModel, IFile
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public FileViewModel(IBaseItem parent, string displayName)
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
    }
}
