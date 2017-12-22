using System.Collections.Generic;

namespace ExplorerLib
{
    /// <summary>
    /// Implements a simple result structure that is returned when the
    /// user selected multiple files in a dialog.
    /// </summary>
    public interface IExplorerMultiFileResult
    {
        /// <summary>
        /// Gets the full file path of the selected file.
        /// </summary>
        IEnumerable<string> Filepaths { get; }

        /// <summary>
        /// Gets the filterindex information of the selected file filter.
        /// </summary>
        int SelectedFilterIndex { get; }
    }
}
