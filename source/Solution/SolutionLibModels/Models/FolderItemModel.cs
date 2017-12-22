namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements an interface to a viewmodel of a folder item in
    /// a tree structured collection of items.
    /// 
    /// Types of collection items can be distinguished via the:
    /// 1) an interface
    ///    (eg. to select a template in an ItemTemplateSelector or
    ///    for usage in an HierarchicalDataTemplate,
    ///    
    /// or
    /// 
    /// 2) through enumeration in <see cref="SolutionLib.Models.SolutionItemType"/>.
    /// </summary>
    [XmlRoot("Folder")]
    internal class FolderItemModel : ItemChildrenModel, IFolderItemModel
    {
        #region constructors
        /// <summary>
        /// Parameterized constructor for normal usage when new elements are created
        /// via other viewmodels through the UI.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="displayName"></param>
        public FolderItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.Folder)
        {
        }

        /// <summary>
        /// Parameterless default constructor required for deserializing XML.
        /// </summary>
        internal FolderItemModel()
            : base(null, string.Empty, Enums.SolutionModelItemType.Folder)
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
            try
            {
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                DisplayName = reader.GetAttribute("name");

                long idValue = -1;
                long.TryParse(reader.GetAttribute("id"), out idValue);
                this.Id = idValue;

                reader.ReadStartElement();  // Consum Folder Tag

                reader.MoveToContent();
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                // Read Items collection and items below it
                if (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                    SolutionModel.ReadItemsCollection(reader, this);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Implements the WriteXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", this.DisplayName);
            writer.WriteAttributeString("id", this.Id.ToString());

            // Child Items are written here...
            writer.WriteStartElement("Items");
            foreach (var item in Children)
            {
                SolutionModel.SerializeItem(writer, item);
            }
            writer.WriteEndElement();
        }
        #endregion IXmlSerializable methods
        #endregion methods
    }
}
