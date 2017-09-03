namespace SolutionLib.Interfaces
{
    using InplaceEditBoxLib.Events;
    using InplaceEditBoxLib.Interfaces;
    using SolutionLib.Models;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Defines properties and members of all objects displayed in a solution.
    /// </summary>
    public interface ISolutionBaseItem : IEditBox, INotifyPropertyChanged
    {
        #region properties
        /// <summary>
        /// Gets a unique technical name to id the item
        /// and manage items in colleciton.
        /// </summary>
        SolutionItemType ItemType { get; }

        /// <summary>
        /// Gets a name for display in UI.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets a description of the item - for usage in tool tip etc..
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        IEnumerable<ISolutionBaseItem> Children { get; }

        /// <summary>
        /// Gets/sets whether this treeview item is expanded or not.
        /// </summary>
        bool IsItemExpanded { get; set; }

        /// <summary>
        /// Gets/sets whether this treeview item is selected or not.
        /// </summary>
        bool IsItemSelected { get; set; }

        /// <summary>
        /// Gets whether the <see cref="DisplayName"/> of this treeview item
        /// can be edit by the user or not.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the parent object where this object is the child in the treeview.
        /// </summary>
        ISolutionBaseItem Parent { get; }

        /// <summary>
        /// Finds a child item by the given key or returns null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        ISolutionBaseItem FindChild(string displyName);

        /// <summary>
        /// Gets/sets a string that determines the order in which items are displayed.
        /// </summary>
        string SortKey { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Sets the value of the <seealso cref="DisplayName"/> property.
        /// </summary>
        /// <param name="displayName"></param>
        void SetDisplayName(string displayName);

        /// <summary>
        /// Sets the value of the <seealso cref="Description"/> property.
        /// </summary>
        /// <param name="description"></param>
        void SetDescription(string description);

        /// <summary>
        /// Sets the value of the <seealso cref="IsReadOnly"/> property.
        /// </summary>
        /// <param name="value"></param>
        void SetIsReadOnly(bool value);

        /// <summary>
        /// Sets the <see cref="Parent"/> property object
        /// where this object is the child in the treeview.
        /// </summary>
        void SetParent(ISolutionBaseItem parent);

        /// <summary>
        /// Adds a child item of type <see cref="ISolutionBaseItem"/> to this parent
        /// which can also be typed with <see cref="ISolutionBaseItem"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ISolutionBaseItem AddChild(ISolutionBaseItem item);

        /// <summary>
        /// Removes a child item from the collection of children in this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveChild(ISolutionBaseItem item);

        /// <summary>
        /// Renames a child item int the collection of children in this item.
        /// A re-sort and IsItemSelected should be applied after the rename such that
        /// the renamed it should re-appear at the correct position in the sorted
        /// list of items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        void RenameChild(ISolutionBaseItem item, string newName);

        #region IEditBox Members
        /// <summary>
        /// Call this method to request of start editing mode for renaming this item.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns true if event was successfully send (listener is attached), otherwise false</returns>
        bool RequestEditMode(RequestEditEvent request);

        /// <summary>
        /// Shows a pop-notification message with the given title and text.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>true if the event was succesfully fired.</returns>
        bool ShowNotification(string title, string message,
                                     BitmapImage imageIcon = null);
        #endregion IEditBox Members

        /// <summary>
        /// Sorts all items for display in a sorted fashion.
        /// </summary>
        void SortChildren();
        #endregion methods
    }
}
