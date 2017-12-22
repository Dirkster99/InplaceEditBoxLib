namespace ExplorerLib
{
    /// <summary>
    /// Implements a simple result structure that is returned when the
    /// user selected a file in a dialog.
    /// </summary>
    public interface IExplorerResult
    {
        /// <summary>
        /// Gets the full file path of the selected file.
        /// </summary>
        string Filepath { get; }

        /// <summary>
        /// Gets the extension of the selected file.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Gets the directory path information of the selected file.
        /// </summary>
        string FileDirectory { get; }

        /// <summary>
        /// Gets the filterindex information of the selected file filter.
        /// </summary>
        int SelectedFilterIndex { get; }
    }
}
