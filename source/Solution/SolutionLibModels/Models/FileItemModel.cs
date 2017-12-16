namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System.Xml;
    using System.Xml.Schema;

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
    public class FileItemModel : ItemModel, IFileItemModel
    {
        #region constructors
        public FileItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.File)
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
            try
            {
                writer.WriteStartElement(GetXmlName(ItemType));
                writer.WriteAttributeString("name", this.DisplayName);
                writer.WriteAttributeString("id", this.Id.ToString());
                writer.WriteEndElement();
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
