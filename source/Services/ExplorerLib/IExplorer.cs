namespace ExplorerLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements an interface to an object that
    /// implements a set of standard functions for accessing the
    /// file system via file open and file save dialogs.
    /// </summary>
    public interface IExplorer
    {
        /// <summary>
        /// Let the user select a file to open
        /// -> return its path if file open was OK'ed
        ///    or return null on cancel.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <param name="defaultExtension"></param>
        /// <param name="selectedExtensionIndex"></param>
        /// <returns></returns>
        IExplorerResult FileOpen(string fileFilter,
                                string lastFilePath,
                                string myDocumentsUserDir = null,
                                string defaultExtension = null,
                                int selectedExtensionIndex = 1);

        /// <summary>
        /// Method can be used to open mutlipe files via standard Windows Explorer
        /// File open dialog.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <param name="defaultExtension"></param>
        /// <param name="selectedExtensionIndex"></param>
        /// <returns></returns>
        IExplorerMultiFileResult FileOpenMultipleFiles(string fileFilter,
                                                       string lastFilePath,
                                                       string myDocumentsUserDir = null,
                                                       string defaultExtension = null,
                                                       int selectedExtensionIndex = 1);

        /// <summary>
        /// Save a file with a given path <paramref name="path"/> (that may be ommited -> results in SaveAs)
        /// using a given save function <paramref name="saveDocumentFunction"/> that takes a string parameter and returns bool on success.
        /// The <param name="saveAsFlag"/> can be set to true to indicate if whether SaveAs function is intended.
        /// The <param name="FileExtensionFilter"/> can be used to filter files when using a SaveAs dialog.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveDocumentFunction"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        string GetDirectoryFromFilePath(string lastFilePath);

        /// <summary>
        /// Save a file with a given path <paramref name="path"/> (that may be ommited -> results in SaveAs)
        ///
        /// The <param name="saveAsFlag"/> can be set to true to indicate whether SaveAs function is intended.
        /// The <param name="FileExtensionFilter"/> can be used to filter files when using a SaveAs dialog.
        /// 
        /// The Save Dialog wrapper function returns a valid string if dialog was exit with OK and
        /// otherwise null (if user Cancelled function).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <returns></returns>
        IExplorerResult SaveDocumentFile(string path,
                                         string myDocumentsUserDir = null,
                                         bool saveAsFlag = false,
                                         string FileExtensionFilter = "",
                                         string defaultExtension = null,
                                         int selectedExtensionIndex = 1);
    }
}
