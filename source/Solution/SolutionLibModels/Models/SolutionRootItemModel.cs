namespace SolutionModelsLib.Models
{
    using System.Xml;
    using System.Xml.Serialization;
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;

    /// <summary>
    /// Implements an interface for a model class of the first visible item in the treeview.
    /// 
    /// Normally, there is only one root in any given tree - so this class implements
    /// that one item visually representing that root (eg.: Computer item in Windows Explorer).
    /// </summary>
    [XmlRoot("RootItem")]
    internal class SolutionRootItemModel : ItemChildrenModel, ISolutionRootItemModel
    {
        #region constructors
        /// <summary>
        /// Parameterized constructor for normal usage when new elements are created
        /// via other viewmodels through the UI.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        public SolutionRootItemModel(string displayName)
            : base(Enums.SolutionModelItemType.SolutionRootItem)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Parameterless default constructor required for deserializing XML.
        /// </summary>
        protected SolutionRootItemModel()
            : base(Enums.SolutionModelItemType.SolutionRootItem)
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

                this.DisplayName = reader.GetAttribute("name");

                long idValue = -1;
                long.TryParse(reader.GetAttribute("id"), out idValue);
                this.Id = idValue;

                reader.ReadStartElement();  // Consum RootItem Tag

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
