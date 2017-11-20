namespace SolutionLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a base class for a collection specific item type that can have children
    /// and does provide functions to manipulate the collection of children
    /// (remove, rename, add child etc.)
    /// </summary>
    public interface IBaseItemChildren : IBaseItem
    {
        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        IEnumerable<IBaseItem> Children { get; }

        #region methods
        /// <summary>
        /// Finds a child item by the given key or returns null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        IBaseItem FindChild(string displyName);

        /// <summary>
        /// Adding a new next child item via Inplace Edit Box requires that
        /// we know whether 'New Folder','New Folder 1', 'New Folder 2' ...
        /// is the next appropriate name - this method determines that name
        /// and returns it for a given type of a (to be created) child item.
        /// </summary>
        /// <param name="nextTypeTpAdd"></param>
        /// <returns></returns>
        string SuggestNextChildName(Models.SolutionItemType nextTypeTpAdd);

        /// <summary>
        /// Adds a child item of type <see cref="IBaseItem"/> to this parent
        /// which can also be typed with <see cref="IBaseItem"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IBaseItem AddChild(IBaseItem item);

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionLib.Models.SolutionItemType"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IBaseItem AddChild(string displayName, Models.SolutionItemType type);

        /// <summary>
        /// Removes a child item from the collection of children in this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveChild(IBaseItem item);

        /// <summary>
        /// Renames a child item int the collection of children in this item.
        /// A re-sort and IsItemSelected should be applied after the rename such that
        /// the renamed it should re-appear at the correct position in the sorted
        /// list of items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        void RenameChild(IBaseItem item, string newName);

        /// <summary>
        /// Removes all children (if any) below this item.
        /// </summary>
        void RemoveAllChild();

        /// <summary>
        /// Sorts all items for display in a sorted fashion.
        /// </summary>
        void SortChildren();

        /// <summary>
        /// Adds another folder (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IBaseItem AddFolder(string displayName);

        /// <summary>
        /// Adds another project (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IBaseItem AddProject(string displayName);

        /// <summary>
        /// Adds another file (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IBaseItem AddFile(string displayName);
        #endregion methods
    }
}
