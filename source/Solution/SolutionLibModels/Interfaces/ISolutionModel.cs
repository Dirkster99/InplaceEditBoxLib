namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;

    public interface ISolutionModel : IModelBase
    {
        //string Name { get; set; }

        ISolutionRootItemModel Root { get; set; }

        IItemModel AddChild(string itemName, SolutionModelItemType itemType, IItemChildrenModel parent);

        IItemChildrenModel AddSolutionRootItem(string displayName);
    }
}