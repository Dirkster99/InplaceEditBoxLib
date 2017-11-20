namespace InPlaceEditBoxDemo.ViewModels
{
    using SolutionLib.Interfaces;
    using System.Windows.Input;
    using System;
    using ExplorerLib;

    internal class AppViewModel : Base.BaseViewModel
    {
        #region fields
        private readonly SolutionLib.Interfaces.ISolution _SolutionBrowser;
        private ICommand _SaveSolutionCommand;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class Constructor
        /// </summary>
        public AppViewModel()
        {
            _SolutionBrowser = SolutionLib.Factory.RootViewModel();
            Demo.Create.Objects(_SolutionBrowser);
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the root viewmodel of the <see cref="ISolution"/> TreeView.
        /// </summary>
        public ISolution Solution
        {
            get { return _SolutionBrowser; }
        }

        /// <summary>
        /// Gets a command that save the current <see cref="Solution"/> to storge.
        /// </summary>
        public ICommand SaveSolutionCommand
        {
            get
            {
                if (_SaveSolutionCommand == null)
                {
                    _SaveSolutionCommand = new Base.RelayCommand<object>((p) =>
                    {
                        var solutionRoot = p as ISolution;

                        if (solutionRoot == null)
                            return;

                        SaveSolutionCommand_Executed(solutionRoot);
                    });
                }

                return _SaveSolutionCommand;
            }
        }

        private string UserDocDir => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        #endregion properties

        #region methods
        private void SaveSolutionCommand_Executed(ISolution solutionRoot)
        {
            var explorer = ServiceLocator.ServiceContainer.Instance.GetService<IExplorer>();

            var filepath = explorer.SaveDocumentFile(UserDocDir + "\\" + "New Solution",
                                                     UserDocDir,
                                                     true,
                                                     solutionRoot.SolutionFileFilter);

            if (string.IsNullOrEmpty(filepath) == false)
            {
                SaveTargetFile(filepath);
            }
        }

        /// <summary>
        /// Method is executed to save a solutions content into the filesystem
        /// (Save As dialog should be called before this function if required
        /// This method executes after a user approved a dialog to Save in this location with this name).
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        private bool SaveTargetFile(string sourcePath)
        {
            //SaveFile(sourcePath, ProcessDestination.Target);

            return true;
        }
        #endregion methods
    }
}

