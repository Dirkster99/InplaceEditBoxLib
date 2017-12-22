namespace SolutionModelsLib.Interfaces
{
    using SolutionModelsLib.Enums;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements an interface to the root model class that manages the complete
    /// tree model data structure that is mainly used for reading and writing data
    /// from and to file based persistence.
    /// </summary>
    public interface ISolutionModel : IModelBase, IXmlSerializable
    {
        /// <summary>
        /// Gets the root item of the tree structure managed in the implementing object
        /// (this tree has only one root item which is why we have no collection here).
        /// </summary>
        ISolutionRootItemModel Root { get; set; }

        /// <summary>
        /// Adds a new item of the requested type and name below the given
        /// <paramref name="parent"/> item.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        IItemModel AddChild(string itemName,
                            SolutionModelItemType itemType,
                            IItemChildrenModel parent);

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// 
        /// This wrapper uses a long input for conversion when reading from file.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IItemModel AddChild(string displayName
                          , long longType
                          , IItemChildrenModel parent);

        /// <summary>
        /// Creates a new solution root item from the given parameters
        /// (replacing the current root item if there is any)
        /// and returns its interface.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IItemChildrenModel AddSolutionRootItem(string displayName, long id = -1);
    }
}