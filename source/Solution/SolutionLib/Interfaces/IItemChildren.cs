namespace SolutionLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements a base class for a collection specific item type that can have children
    /// and does provide functions to manipulate the collection of children
    /// (remove, rename, add child etc.)
    /// </summary>
    public interface IItemChildren : IItem
    {
        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        IEnumerable<IItem> Children { get; }

        #region methods
        /// <summary>
        /// Finds a child item by the given key or returns null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        IItem FindChild(string displyName);

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
        /// Adds a child item of type <see cref="IItem"/> to this parent
        /// which can also be typed with <see cref="IItem"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IItem AddChild(IItem item);

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionLib.Models.SolutionItemType"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IItem AddChild(string displayName, Models.SolutionItemType type);

        /// <summary>
        /// Removes a child item from the collection of children in this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveChild(IItem item);

        /// <summary>
        /// Renames a child item int the collection of children in this item.
        /// A re-sort and IsItemSelected should be applied after the rename such that
        /// the renamed it should re-appear at the correct position in the sorted
        /// list of items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        void RenameChild(IItem item, string newName);

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
        IItem AddFolder(string displayName);

        /// <summary>
        /// Adds another project (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem AddProject(string displayName);

        /// <summary>
        /// Adds another file (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItem AddFile(string displayName);
        #endregion methods
    }
}
