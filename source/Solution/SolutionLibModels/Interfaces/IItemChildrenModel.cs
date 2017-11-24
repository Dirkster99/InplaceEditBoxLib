namespace SolutionModelsLib.Interfaces
{
    using System.Collections.Generic;

    public interface IItemChildrenModel : IItemModel
    {
        #region properties
        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        IList<IItemModel> Children { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Finds a child item based on the given key in <paramref name="displayName"/>.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItemModel FindChild(string displayName);

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        void AddChild(IItemModel item);
        #endregion methods
    }
}
