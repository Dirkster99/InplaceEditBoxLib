namespace SolutionLib.ViewModels.Browser
{
    using InplaceEditBoxLib.Events;
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using SolutionLib.ViewModels.Collections;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using UserNotification.Events;

    /// <summary>
    /// Implements the base functionality for all item collections
    /// in the treeview ItemSource.
    /// </summary>
    internal class SolutionBaseItemViewModel : Base.BaseViewModel, ISolutionBaseItem
    {
        #region fields
        private readonly SolutionItemType _ItemType;
        private readonly SortableObservableDictionaryCollection _Children;

        private string _DisplayName;
        private string _Description;
        private bool _IsItemExpanded;
        private bool _IsItemSelected;

        private ISolutionBaseItem _Parent = null;

        private bool _IsReadOnly;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        protected SolutionBaseItemViewModel(ISolutionBaseItem parent, SolutionItemType itemType)
            : this()
        {
            SetParent(parent);
            _ItemType = itemType;
        }

        #region events
        /// <summary>
        /// Expose an event that is triggered when the viewmodel tells its view:
        /// Here is another notification message please show it to the user.
        /// </summary>
        public event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;

        /// <summary>
        /// Expose an event that is triggered when the viewmodel requests its view to
        /// start the editing mode for rename this item.
        /// </summary>
        public event InplaceEditBoxLib.Events.RequestEditEventHandler RequestEdit;
        #endregion events

        /// <summary>
        /// Class constructor
        /// </summary>
        protected SolutionBaseItemViewModel()
        {
            _Children = new SortableObservableDictionaryCollection();
            _IsItemExpanded = false;
            _IsItemSelected = false;

            _IsReadOnly = false;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the enumerated type of an item in the solution.
        /// </summary>
        public SolutionItemType ItemType { get { return _ItemType; } }

        /// <summary>
        /// Gets the name of this item to be displayed in the UI.
        /// </summary>
        public string DisplayName
        {
            get { return _DisplayName; }
            private set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;
                    NotifyPropertyChanged(() => DisplayName);
                }
            }
        }

        /// <summary>
        /// Gets the description of this item to be displayed in the UI.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            private set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged(() => Description);
                }
            }
        }

        /// <summary>
        /// Gets a sorted collection of child items that can
        /// be retreived from this parent item.
        /// </summary>
        public IEnumerable<ISolutionBaseItem> Children
        {
            get
            {
                return _Children;
            }
        }

        /// <summary>
        /// Gets/sets whether this treeview item is expanded or not.
        /// </summary>
        public bool IsItemExpanded
        {
            get { return _IsItemExpanded; }
            set
            {
                if (_IsItemExpanded != value)
                {
                    _IsItemExpanded = value;
                    NotifyPropertyChanged(() => IsItemExpanded);
                }
            }
        }

        /// <summary>
        /// Gets/sets whether this treeview item is selected or not.
        /// </summary>
        public bool IsItemSelected
        {
            get { return _IsItemSelected; }
            set
            {
                if (_IsItemSelected != value)
                {
                    _IsItemSelected = value;
                    NotifyPropertyChanged(() => IsItemSelected);
                }
            }
        }

        /// <summary>
        /// Gets/sets whether the <see cref="DisplayName"/> of this treeview item
        /// can be renamed by the user or not.
        /// 
        /// This property is part of the <see cref="InplaceEditBoxLib.Interfaces.IEditBox"/>
        /// interface and should, therefore, not be renamed.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            private set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    NotifyPropertyChanged(() => IsReadOnly);
                }
            }
        }

        /// <summary>
        /// Gets the parent object where this object is the child in the treeview.
        /// </summary>
        public ISolutionBaseItem Parent { get { return _Parent; } }

        /// <summary>
        /// Gets/sets a string that determines the order in which items are displayed.
        /// </summary>
        public string SortKey { get; set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ISolutionBaseItem AddChild(string displayName, SolutionItemType type)
        {
            switch (type)
            {
                case SolutionItemType.File:
                    return AddChild(displayName, new FileViewModel(this, displayName));

                case SolutionItemType.Folder:
                    return AddChild(displayName, new FolderViewModel(this, displayName));

                case SolutionItemType.Project:
                    return AddChild(displayName, new ProjectViewModel(this, displayName));

                default:
                case SolutionItemType.SolutionRootItem:
                    throw new System.ArgumentOutOfRangeException(type.ToString());
            }
        }

        /// <summary>
        /// Sets the value of the <seealso cref="DisplayName"/> property.
        /// </summary>
        /// <param name="displayName"></param>
        public void SetDisplayName(string displayName)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Sets the value of the <seealso cref="Description"/> property.
        /// </summary>
        /// <param name="description"></param>
        public void SetDescription(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// Sets the value of the <seealso cref="IsReadOnly"/> property.
        /// </summary>
        /// <param name="value"></param>
        public void SetIsReadOnly(bool value)
        {
            IsReadOnly = value;
        }

        /// <summary>
        /// Adds a child item of type <see cref="ISolutionBaseItem"/> to this parent
        /// which can also be typed with <see cref="ISolutionBaseItem"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ISolutionBaseItem AddChild(ISolutionBaseItem item)
        {
            return AddChild(item.DisplayName, item);
        }

        public string SuggestNextChildName(SolutionItemType nextTypeTpAdd)
        {
            string suggestMask = null;

            switch (nextTypeTpAdd)
            {
                case SolutionItemType.SolutionRootItem:
                    suggestMask = "New Solution";
                    break;
                case SolutionItemType.File:
                    suggestMask = "New File";
                    break;
                case SolutionItemType.Folder:
                    suggestMask = "New Folder";
                    break;
                case SolutionItemType.Project:
                    suggestMask = "New Project";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nextTypeTpAdd.ToString());
            }

            var nextChild = _Children.TryGet(suggestMask);
            if (nextChild == null)
                return suggestMask;

            string suggestChild = null;
            for (int i = 1; i < _Children.Count + 100; i++)
            {
                suggestChild = string.Format("{0} {1}", suggestMask, i);

                nextChild = _Children.TryGet(suggestChild);
                if (nextChild == null)
                    break;
            }

            return suggestChild;
        }

        /// <summary>
        /// Removes a child item from the collection of children in this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveChild(ISolutionBaseItem item)
        {
            if (item == null)
                return false;

            item.SetParent(null);

            var itemIsSelected = item.IsItemSelected;
            var idx = _Children.IndexOf(item);
            var removedItem = _Children.RemoveItem(item);

            if (itemIsSelected == false)
                return removedItem;

            // Removed item was selected so lets try and select something nearby
            if (idx <= 0)
                this.IsItemSelected = true;
            else
                _Children[idx-1].IsItemSelected = true;

            return removedItem;
        }

        /// <summary>
        /// Removes all children (if any) below this item.
        /// </summary>
        public void RemoveAllChild()
        {
            _Children.Clear();
        }

        /// <summary>
        /// Renames a child item in the collection of children in this item.
        /// A re-sort and IsItemSelected should be applied after the rename such that
        /// the renamed item should re-appear at the correct position in the sorted
        /// list of items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public void RenameChild(ISolutionBaseItem item, string newName)
        {
            if (item == null)
                return;

            _Children.RenameItem(item, newName);
        }

        /// <summary>
        /// Finds a child item by the given key or returns null.
        /// </summary>
        /// <param name="displyName"></param>
        /// <returns></returns>
        public ISolutionBaseItem FindChild(string displyName)
        {
            return _Children.TryGet(displyName);
        }

        /// <summary>
        /// Sorts all items for display in a sorted fashion.
        /// </summary>
        public void SortChildren()
        {
            _Children.SortItems();
        }

        /// <summary>
        /// Sets the <see cref="Parent"/> property object
        /// where this object is the child in the treeview.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(ISolutionBaseItem parent)
        {
            _Parent = parent;
            NotifyPropertyChanged(() => Parent);
        }

        #region IEditBox Members
        /// <summary>
        /// Call this method to request of start editing mode for renaming this item.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns true if event was successfully send (listener is attached), otherwise false</returns>
        public bool RequestEditMode(RequestEditEvent request)
        {
            if (this.RequestEdit != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.RequestEdit(this, new RequestEdit(request));
                }));
                return true;
            }
            else
            {
                System.Console.WriteLine("CANNOT Request Edit Mode in ViewModel (No View Attached).");
            }

            return false;
        }

        /// <summary>
        /// Shows a pop-notification message with the given title and text.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="imageIcon"></param>
        /// <returns>true if the event was succesfully fired.</returns>
        public bool ShowNotification(string title, string message,
                                     BitmapImage imageIcon = null)
        {
            if (this.ShowNotificationMessage != null)
            {
                this.ShowNotificationMessage(this, new ShowNotificationEvent
                (
                  title,
                  message,
                  imageIcon
                ));

                return true;
            }

            return false;
        }
        #endregion IEditBox Members

        /// <summary>
        /// Adds a child item of type <see cref="ISolutionBaseItem"/> to this parent
        /// which can also be typed with <see cref="ISolutionBaseItem"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ISolutionBaseItem AddChild(string key, ISolutionBaseItem value)
        {
            _Children.AddItem(value);

            return value;
        }
        #endregion methods
    }
}
