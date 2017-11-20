namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;

    public interface ISolutionModel : IModelBase
    {
        string Name { get; set; }

        ISolutionRootItemModel Root { get; set; }

        IBaseItemModel AddChild(string itemName, SolutionModelItemType itemType, IBaseItemChildrenModel parent);
        IBaseItemModel AddRootChild(string itemName, SolutionModelItemType itemType);
        IBaseItemModel AddSolutionRootItem(string displayName);
    }
}