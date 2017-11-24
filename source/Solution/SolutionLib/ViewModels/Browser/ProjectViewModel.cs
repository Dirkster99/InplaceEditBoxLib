namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;

    internal class ProjectViewModel : Base.ItemChildrenViewModel, IProject
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        /// <param name="addDummyChild"></param>
        public ProjectViewModel(IItem parent
                                , string displayName
                                , bool addDummyChild = true)
            : base(parent, Models.SolutionItemType.Project, addDummyChild)
        {
            SetDisplayName(displayName);
        }

        /// <summary>
        /// Standard Class constructor
        /// </summary>
        protected ProjectViewModel()
           : base(null, Models.SolutionItemType.Project)
        {
        }
        #endregion constructors
    }
}
