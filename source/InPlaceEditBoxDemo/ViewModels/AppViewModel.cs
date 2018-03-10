namespace InPlaceEditBoxDemo.ViewModels
{
    using SolutionLib.Interfaces;
    using System.Windows.Input;
    using System;
    using ExplorerLib;
    using SolutionModelsLib.Models;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Windows;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Xml;

    /// <summary>
    /// Manages backend objects and functions for the Application.
    /// </summary>
    internal class AppViewModel : Base.BaseViewModel
    {
        #region fields
        private readonly SolutionLib.Interfaces.ISolution _SolutionBrowser;

        private ICommand _SaveSolutionCommand;
        private ICommand _LoadSolutionCommand;
        private bool _IsProcessing;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class Constructor
        /// </summary>
        public AppViewModel()
        {
            _IsProcessing = false;
            _SolutionBrowser = SolutionLib.Factory.RootViewModel();

            LastFileAccess = UserDocDir + "\\" + "New Solution";

            // This is the Selected FilterIndex + 1 (starting at 1)
            SelectedFileExtFilterIndex = 1;
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
        /// Gets a property to determine if application is currently processing
        /// data (loading or searching for matches in the tree view) or not.
        /// </summary>
        public bool IsProcessing
        {
            get { return _IsProcessing; }
            protected set
            {
                if (_IsProcessing != value)
                {
                    _IsProcessing = value;
                    NotifyPropertyChanged(() => IsProcessing);
                }
            }
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
                    _SaveSolutionCommand = new Base.RelayCommand<object>(async (p) =>
                    {
                        var solutionRoot = p as ISolution;

                        if (solutionRoot == null)
                            return;

                        await SaveSolutionCommand_ExecutedAsync(solutionRoot);
                    });
                }

                return _SaveSolutionCommand;
            }
        }

        /// <summary>
        /// Gets a command that save the current <see cref="Solution"/> to storge.
        /// </summary>
        public ICommand LoadSolutionCommand
        {
            get
            {
                if (_LoadSolutionCommand == null)
                {
                    _LoadSolutionCommand = new Base.RelayCommand<object>((p) =>
                    {
                        var solutionRoot = p as ISolution;

                        if (solutionRoot == null)
                            return;

                        LoadSolutionCommand_ExecutedAsync(solutionRoot);
                    });
                }

                return _LoadSolutionCommand;
            }
        }

        private string LastFileAccess { get; set; }

        private int SelectedFileExtFilterIndex { get; set; }

        /// <summary>
        /// Gets the default directory for opening the Save As ... dialog.
        /// </summary>
        private string UserDocDir => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        #endregion properties

        #region methods
        public void ResetDefaults()
        {
            _SolutionBrowser.ResetToDefaults();
        }

        /// <summary>
        /// Loads initial data items for the Solution demo.
        /// </summary>
        internal async Task LoadSampleDataAsync()
        {
            try
            {
                IsProcessing = true;
                await Demo.Create.ObjectsAsync(_SolutionBrowser);
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async Task SaveSolutionCommand_ExecutedAsync(ISolution solutionRoot)
        {
            var explorer = ServiceLocator.ServiceContainer.Instance.GetService<IExplorer>();

            var result = explorer.SaveDocumentFile(LastFileAccess,
                                                   UserDocDir,
                                                   true,
                                                   solutionRoot.SolutionFileFilter,
                                                   solutionRoot.SolutionFileFilterDefault,
                                                   SelectedFileExtFilterIndex);

            if (result == null) // User clicked Cancel ...
                return;

            IsProcessing = true;
            try
            {
                LastFileAccess = result.Filepath;
                SelectedFileExtFilterIndex = result.SelectedFilterIndex;

                // Convert to model and save model to file system
                var solutionModel = new ViewModelModelConverter().ToModel(solutionRoot);

                var filename_ext = System.IO.Path.GetExtension(result.Filepath);
                switch (filename_ext)
                {
                    case ".solxml":
                        SolutionModelsLib.Xml.Storage.WriteXmlToFile(result.Filepath, solutionModel);
                        break;

                    default:
                        var st_result = await SaveSolutionFileAsync(result.Filepath, solutionModel);
                        break;
                }
            }
            finally
            {
                IsProcessing = false;
            }
        }

        /// <summary>
        /// Method is executed to save a solutions content into the filesystem
        /// (Save As dialog should be called before this function if required
        /// This method executes after a user approved a dialog to Save in this
        /// location with this name).
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="solutionRoot"></param>
        /// <returns></returns>
        private async Task<bool> SaveSolutionFileAsync(string sourcePath
                                                     , ISolutionModel solutionRoot)
        {
            return await Task.Run<bool>(() =>
            {
                return SaveSolutionFile(sourcePath, solutionRoot);
            });
        }

        /// <summary>
        /// Method is executed to save a solutions content into the filesystem
        /// (Save As dialog should be called before this function if required
        /// This method executes after a user approved a dialog to Save in this
        /// location with this name).
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="solutionRoot"></param>
        /// <returns></returns>
        private bool SaveSolutionFile(string sourcePath
                                     ,ISolutionModel solutionRoot)
        {
            SolutionModelsLib.SQLite.SolutionDB db = new SolutionModelsLib.SQLite.SolutionDB();
            db.SetFileNameAndPath(sourcePath);

            Console.WriteLine("1) Writting data into SQLite file: '{0}'", db.DBFileNamePath);
            int recordCount = 0;
            int itemTypeCount = 0;

            try
            {
                // Overwrites the existing file (if any)
                db.OpenConnection(true);

                if (db.ConnectionState == false)
                {
                    Console.WriteLine("ERROR: Cannot open Database connectiton.\n" + db.Status);
                    return false;
                }

                db.ReCreateDBTables(db);

                // Write itemtype enumeration into file
                var names = Enum.GetNames(typeof(SolutionModelsLib.Enums.SolutionModelItemType));
                var values = Enum.GetValues(typeof(SolutionModelsLib.Enums.SolutionModelItemType));
                itemTypeCount = db.InsertItemTypeEnumeration(names, values);

                // Write solution tree data file
                recordCount = db.InsertSolutionData(solutionRoot);
            }
            catch (Exception exp)
            {
                Console.WriteLine("\n\nAN ERROR OCURRED: " + exp.Message + "\n");
            }
            finally
            {
                db.CloseConnection();
            }

            Console.WriteLine("{0:000} records written to itemtype enumeration table...", itemTypeCount);
            Console.WriteLine("{0:000} records written to solution data table...", recordCount);

            return true;
        }

        private void LoadSolutionCommand_ExecutedAsync(ISolution solutionRoot)
        {
            var explorer = ServiceLocator.ServiceContainer.Instance.GetService<IExplorer>();

            var result = explorer.FileOpen(solutionRoot.SolutionFileFilter,
                                            LastFileAccess,
                                            UserDocDir,
                                            solutionRoot.SolutionFileFilterDefault,
                                            SelectedFileExtFilterIndex);

            if (result == null) // User clicked Cancel ...
                return;

            LastFileAccess = result.Filepath;
            SelectedFileExtFilterIndex = result.SelectedFilterIndex;

            // Read model from file system and convert model to viewmodel
            var filename_ext = result.FileExtension;
            int recordCount = 0;
            ISolutionModel solutionModel = null;
            switch (filename_ext)
            {
                case ".solxml":
                    solutionModel = Storage.ReadXmlFromFile<ISolutionModel>(result.Filepath);
                    break;

                default:
                    solutionModel = LoadSolutionFile(result.Filepath, out recordCount);
                    break;
            }

            new ViewModelModelConverter().ToViewModel(solutionModel, solutionRoot);

            var rootItem = solutionRoot.GetRootItem();  // Show items below root by default
            if (rootItem != null)
                rootItem.IsItemExpanded = true;
        }

        /// <summary>
        /// Method is executed to load a solutions content from the filesystem
        /// (Open file dialog should be called before this function if required).
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        private ISolutionModel LoadSolutionFile(string sourcePath, out int recordCount)
        {
            recordCount = 0;
            ISolutionModel solutionRoot = null;

            var db = new SolutionModelsLib.SQLite.SolutionDB();
            try
            {
                db.SetFileNameAndPath(sourcePath);

                db.OpenConnection();

                if (db.ConnectionState == false)
                {
                    MessageBox.Show("ERROR: Cannot open Database connectiton.\n" + db.Status);
                    return null;
                }

                solutionRoot = SolutionModelsLib.Factory.CreateSolutionModel();  // Select Result from Database

                var mapKeyToItem = db.ReadItemTypeEnum();
                bool checkResult = CompareItemTypeEnums(mapKeyToItem);

                if (checkResult == false)
                {
                    MessageBox.Show("ERROR: Cannot open file: itemtype enumeration is not consistent.");
                    return null;
                }

                recordCount = db.ReadSolutionData(solutionRoot, db);
            }
            catch (Exception exp)
            {
                MessageBox.Show("\n\nAN ERROR OCURRED: " + exp.Message + "\n");
            }
            finally
            {
                db.CloseConnection();
            }

            return solutionRoot;
        }

        /// <summary>
        /// Compares a dictionary of long, string values to a given enumeration
        /// and returns true if all values and string names of the enumeration
        /// are present in the dictionary, otherwise false.
        /// </summary>
        /// <param name="mapKeyToItem"></param>
        /// <returns></returns>
        private bool CompareItemTypeEnums(Dictionary<long, string> mapKeyToItem)
        {
            var names = Enum.GetNames(typeof(SolutionModelsLib.Enums.SolutionModelItemType));
            var values = Enum.GetValues(typeof(SolutionModelsLib.Enums.SolutionModelItemType));

            for (int i = 0; i < names.Length; i++)
            {
                string name;
                if (mapKeyToItem.TryGetValue((int)values.GetValue(i), out name) == false)
                    return false;

                if (name != names[i].ToString())
                    return false;
            }

            return true;
        }
        #endregion methods
    }
}

