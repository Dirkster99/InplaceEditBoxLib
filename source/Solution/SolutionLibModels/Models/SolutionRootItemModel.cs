namespace SolutionModelsLib.Models
{
    using System.Xml;
    using System.Xml.Schema;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;

    /// <summary>
    /// Implements an interface for a model class of the first visible item in the treeview.
    /// 
    /// Normally, there is only one root in any given tree - so this class implements
    /// that one item visually representing that root (eg.: Computer item in Windows Explorer).
    /// </summary>
    public class SolutionRootItemModel : ItemChildrenModel, ISolutionRootItemModel
    {
        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="parent"></param>
        public SolutionRootItemModel(string displayName)
            : base(Enums.SolutionModelItemType.SolutionRootItem)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected SolutionRootItemModel()
            : base(Enums.SolutionModelItemType.SolutionRootItem)
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
