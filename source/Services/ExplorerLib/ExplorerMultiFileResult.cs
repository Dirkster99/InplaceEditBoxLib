namespace ExplorerLib
{
    using System.Collections.Generic;

    internal class ExplorerMultiFileResult : IExplorerMultiFileResult
    {
        private List<string> _FilePath = null;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="filepaths"></param>
        /// <param name="selectedFilterIndex"></param>
        public ExplorerMultiFileResult(IEnumerable<string> filepaths
                                     , int selectedFilterIndex)
            : this()
        {
            if (filepaths != null)
            {
                foreach (var item in filepaths)
                    _FilePath.Add(item);
            }

            SelectedFilterIndex = selectedFilterIndex;
        }

        /// <summary>
        /// Hidden parameterless ctor
        /// </summary>
        protected ExplorerMultiFileResult()
        {
            _FilePath = new List<string>();
        }

        /// <summary>
        /// Gets the full file path of the selected file.
        /// </summary>
        public IEnumerable<string> Filepaths
        {
            get
            {
                return _FilePath;
            }
        }

        /// <summary>
        /// Gets the filterindex information of the selected file filter.
        /// </summary>
        public int SelectedFilterIndex { get; private set; }
    }
}
