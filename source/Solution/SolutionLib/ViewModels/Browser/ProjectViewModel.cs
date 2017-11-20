namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class ProjectViewModel : BaseItemChildrenViewModel, IProject
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public ProjectViewModel(IBaseItem parent, string displayName)
            : base(parent, Models.SolutionItemType.Project)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ProjectViewModel()
           : base(null, Models.SolutionItemType.Project)
        {
        }
        #endregion constructors
    }
}
