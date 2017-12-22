namespace ExplorerLib
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Implements a set of standard functions for accessing the
    /// file system via file open and file save dialogs.
    /// </summary>
    public class Explorer : ExplorerLib.IExplorer
    {
        private string DefaultDocumentsUserDir = @"C:\";

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
        public IExplorerResult FileOpen(string fileFilter,
                                        string lastFilePath,
                                        string myDocumentsUserDir = null,
                                        string defaultExtension = null,
                                        int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            var dlg = new OpenFileDialog();

            dlg.Multiselect = false;

            string dir = lastFilePath;

            try
            {
                if (System.IO.Directory.Exists(lastFilePath) == false)
                    dir = GetDirectoryFromFilePath(lastFilePath);
            }
            catch
            {
            }

            dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);

            dlg.Filter = fileFilter;
            dlg.FilterIndex = selectedExtensionIndex;

            if (string.IsNullOrEmpty(defaultExtension) == false)
            {
                try
                {
                    dlg.DefaultExt = "." + defaultExtension;
                }
                catch {}
            }

            if (dlg.ShowDialog().GetValueOrDefault() == true)
            {
                if (string.IsNullOrEmpty(dlg.FileName) == false)
                    return new ExplorerResult(dlg.FileName, dlg.FilterIndex);
            }

            return null;
        }

        /// <summary>
        /// Method can be used to open mutlipe files via standard Windows Explorer
        /// File open dialog.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <returns></returns>
        public IExplorerMultiFileResult FileOpenMultipleFiles(string fileFilter,
                                                              string lastFilePath,
                                                              string myDocumentsUserDir = null,
                                                              string defaultExtension = null,
                                                              int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            var dlg = new OpenFileDialog();

            dlg.Multiselect = false;

            string dir = lastFilePath;

            try
            {
                if (System.IO.Directory.Exists(lastFilePath) == false)
                    dir = GetDirectoryFromFilePath(lastFilePath);
            }
            catch
            {
            }

            dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);
            dlg.Multiselect = true;
            dlg.Filter = fileFilter;
            dlg.FilterIndex = selectedExtensionIndex;

            if (string.IsNullOrEmpty(defaultExtension) == false)
            {
                try
                {
                    dlg.DefaultExt = "." + defaultExtension;
                }
                catch { }
            }

            if (dlg.ShowDialog().GetValueOrDefault())
                return new ExplorerMultiFileResult(dlg.FileNames, dlg.FilterIndex);

            return null;
        }

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
        public IExplorerResult SaveDocumentFile(string path,
                                                string myDocumentsUserDir = null,
                                                bool saveAsFlag = false,
                                                string FileExtensionFilter = "",
                                                string defaultExtension = null,
                                                int selectedExtensionIndex = 1)
        {
            if (string.IsNullOrEmpty(myDocumentsUserDir) == true)
                myDocumentsUserDir = DefaultDocumentsUserDir;

            string filePath = (path == null ? string.Empty : path);

            // Offer SaveAs file dialog if file has never been saved before (was created with new command)
            //  saveAsFlag = saveAsFlag | !fileToSave.IsFilePathReal;

            try
            {
                if (filePath == string.Empty || saveAsFlag == true)   // Execute SaveAs function
                {
                    var dlg = new SaveFileDialog();

                    try
                    {
                        dlg.FileName = System.IO.Path.GetFileName(filePath);
                    }
                    catch
                    {
                    }

                    string dir = GetDirectoryFromFilePath(path);
                    dlg.InitialDirectory = (dir == null ? myDocumentsUserDir : dir);

                    if (string.IsNullOrEmpty(FileExtensionFilter) == false)
                    {
                        dlg.Filter = FileExtensionFilter;
                        dlg.FilterIndex = selectedExtensionIndex;

                        if (string.IsNullOrEmpty(defaultExtension) == false)
                        {
                            try
                            {
                                dlg.DefaultExt = "." + defaultExtension;
                            }
                            catch { }
                        }
                    }

                    if (dlg.ShowDialog().GetValueOrDefault() == true)     // SaveAs file if user OK'ed it so
                    {
                        filePath = dlg.FileName;

                        return new ExplorerResult(dlg.FileName, dlg.FilterIndex);
                    }
                    else
                        return null;
                }                      // Execute Save function

                return null;
            }
            catch (Exception Exp)
            {
                string sMsg = Local.Strings.STR_MSG_ErrorSavingFile;

                if (filePath.Length > 0)
                    sMsg = string.Format(CultureInfo.CurrentCulture, Local.Strings.STR_MSG_ErrorWhileSavingFileX, Exp.Message, filePath);
                else
                    sMsg = string.Format(CultureInfo.CurrentCulture, Local.Strings.STR_MSG_ErrorWhileSavingAFile, Exp.Message);

                throw new Exception(sMsg, Exp);
            }
        }

        /// <summary>
        /// Get a file path name reference and return the containing path (if any)
        /// </summary>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        public string GetDirectoryFromFilePath(string lastFilePath)
        {
            string dir = null;

            try
            {
                if (string.IsNullOrEmpty(lastFilePath) == false)
                {
                    dir = System.IO.Path.GetDirectoryName(lastFilePath);

                    if (System.IO.Directory.Exists(dir) == false)
                        dir = null;
                }
            }
            catch
            {
            }

            return dir;
        }
    }
}
