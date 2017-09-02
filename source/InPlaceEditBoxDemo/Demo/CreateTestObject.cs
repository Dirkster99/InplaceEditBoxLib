namespace InPlaceEditBoxDemo.Demo
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// implements a simple pojo type class to support the generation of test data.
    /// </summary>
    internal class CreateTestObject
    {
        public CreateTestObject(
              string project
            , string[] folders
            , string[] files)
            : this()
        {
            this.Project = project;

            if (folders != null)
                this.Folders = folders.ToList();

            if (files != null)
                this.Files = files.ToList();
        }

        protected CreateTestObject()
        {
            Project = string.Empty;
            Folders = null;
            Files = null;
        }

        public string Project { get; protected set; }

        public List<string> Folders { get; protected set; }

        public List<string> Files { get; protected set; }
    }
}
