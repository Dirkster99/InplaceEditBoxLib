﻿namespace SolutionLib.ViewModels.Browser
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;

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
            return AddChild(displayName,  SolutionItemType.Folder);
        }

        /// <summary>
        /// Adds another project (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem ISolutionItem.AddProject(string displayName)
        {
            return AddChild(displayName, SolutionItemType.Project);
        }

        /// <summary>
        /// Adds another file (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem ISolutionItem.AddFile(string displayName)
        {
            return AddChild(displayName, SolutionItemType.File);
        }
        #endregion methods
    }
}
