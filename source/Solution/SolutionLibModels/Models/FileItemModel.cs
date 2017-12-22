namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements an interface to a viewmodel of a file item in
    /// a tree structured collection of items.
    /// 
    /// Types of collection items can be distinguished via the:
    /// 1) an interface
    ///    (eg. to select a template in an ItemTemplateSelector"/> or
    ///    for usage in an HierarchicalDataTemplate./>,
    ///    
    /// or
    /// 
    /// 2) thrpigh enumeration in <see cref="SolutionLib.Models.SolutionItemType"/>.
    /// </summary>
    [XmlRoot("File")]
    internal class FileItemModel : ItemModel, IFileItemModel
    {
        #region constructors
        /// <summary>
        /// Parameterized constructor for normal usage when new elements are created
        /// via other viewmodels through the UI.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        public FileItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.File)
        {
        }

        /// <summary>
        /// Parameterless default constructor required for deserializing XML.
        /// </summary>
        internal FileItemModel()
            : base(null, string.Empty, Enums.SolutionModelItemType.File)
        {

        }
        #endregion constructors

        #region methods
        #region IXmlSerializable methods
        /// <summary>
        /// Implements the ReadXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                reader.Read();

            DisplayName = reader.GetAttribute("name");

            long idValue = -1;
            long.TryParse(reader.GetAttribute("id"), out idValue);
            this.Id = idValue;

            reader.ReadStartElement();  // Consum File Tag
        }

        /// <summary>
        /// Implements the WriteXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            try
            {
                writer.WriteAttributeString("name", this.DisplayName);
                writer.WriteAttributeString("id", this.Id.ToString());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        #endregion IXmlSerializable methods
        #endregion methods
    }
}
