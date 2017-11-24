namespace SolutionLib.Interfaces
{
    using InplaceEditBoxLib.Events;
    using InplaceEditBoxLib.Interfaces;
    using SolutionLib.Models;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Defines properties and members of all objects displayed in a solution.
    /// </summary>
    public interface IItem : IEditBox, IViewModelBase, IParent
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
        /// Sets the Parent property object
        /// where this object is the child in the treeview.
        /// </summary>
        void SetParent(IItem parent);

        /// <summary>
        /// Sets the ID of an item in the collection.
        /// </summary>
        /// <param name="itemId"></param>
        void SetId(long itemId);

        /// <summary>
        /// Gets the ID of an item in the collection.
        /// </summary>
        long GetId();

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
        #endregion methods
    }
}
