namespace ExplorerLib
{
    internal class ExplorerResult : IExplorerResult
    {
        public ExplorerResult(string filepath
                            , int selectedFilterIndex)
            : this()
        {
            Filepath = filepath;
            SelectedFilterIndex = selectedFilterIndex;
        }

        protected ExplorerResult()
        {
        }

        public string Filepath { get; private set; }

        public string FileExtension
        {
            get
            {
                try
                {
                    return System.IO.Path.GetExtension(Filepath);
                }
                catch{}

                return string.Empty;
            }
        }

        public string FileDirectory
        {
            get
            {
                try
                {
                    return System.IO.Path.GetDirectoryName(Filepath);
                }
                catch { }

                return string.Empty;
            }
        }

        public int SelectedFilterIndex { get; private set; }
    }
}
