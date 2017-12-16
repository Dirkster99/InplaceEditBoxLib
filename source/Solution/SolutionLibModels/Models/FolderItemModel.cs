namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System.Xml;
    using System.Xml.Schema;

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
    /// 2) thrpigh enumeration in <see cref="SolutionLib.Models.SolutionItemType"/>.
    /// </summary>
    public class FolderItemModel : ItemChildrenModel, IFolderItemModel
    {
        #region constructors
        public FolderItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.Folder)
        {
        }
        #endregion constructors

        #region methods
        #region IXmlSerializable methods
        public override XmlSchema GetSchema()
        {
            return null;
        }

        public override void ReadXml(XmlReader reader)
        {
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(GetXmlName(ItemType));
            writer.WriteAttributeString("name", this.DisplayName);
            writer.WriteAttributeString("id", this.Id.ToString());

            // Child Items are written here...
            writer.WriteStartElement("Items");
            foreach (var item in Children)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        #endregion IXmlSerializable methods
        #endregion methods
    }
}
