namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;
    using System.Xml.Serialization;

    public interface ISolutionModel : IModelBase, IXmlSerializable
    {
        //string Name { get; set; }

        ISolutionRootItemModel Root { get; set; }

        IItemModel AddChild(string itemName, SolutionModelItemType itemType, IItemChildrenModel parent);

        IItemChildrenModel AddSolutionRootItem(string displayName);
    }
}