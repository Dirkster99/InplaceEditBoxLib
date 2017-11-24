namespace SolutionLib.ViewModels.Browser.Base
{
    using InplaceEditBoxLib.Events;
    using SolutionLib.ViewModels.Base;
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using UserNotification.Events;

    /// <summary>
    /// Implements base functions for all treeview items that are NOT concerned
    /// about managing Children collections. The functionality in this base class
    /// is focussed on the item itself (not on it's children). The design aims
    /// at implementing items, such as files, that may not even have
    /// children themselves.
    /// </summary>
    internal abstract class ItemViewModel : BaseViewModel, IItem
    {
        #region fields
        private readonly SolutionItemType _ItemType;

        private string _DisplayName;
        private string _Description;
        private bool _IsItemExpanded;
        private bool _IsItemSelected;

        private IItem _Parent = null;

        private bool _IsReadOnly;
        private long _ItemId = -1;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemViewModel(IItem parent, SolutionItemType itemType)
            : this()
        {
            SetParent(parent);
            _ItemType = itemType;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemViewModel()
        {
            _IsItemExpanded = false;
            _IsItemSelected = false;

            _IsReadOnly = false;
        }
        #endregion constructors

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
        public IItem Parent { get { return _Parent; } }

        /// <summary>
        /// Gets/sets a string that determines the order in which items are displayed.
        /// </summary>
        public string SortKey { get; set; }
        #endregion properties

        #region methods
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
        /// Sets the <see cref="Parent"/> property object
        /// where this object is the child in the treeview.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IItem parent)
        {
            _Parent = parent;
            NotifyPropertyChanged(() => Parent);
        }

        /// <summary>
        /// Sets the ID of an item in the collection.
        /// </summary>
        /// <param name="itemId"></param>
        void IItem.SetId(long itemId)
        {
            _ItemId = itemId;
        }

        /// <summary>
        /// Gets the ID of an item in the collection.
        /// </summary>
        long IItem.GetId()
        {
            return _ItemId;
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
        #endregion methods
    }
}
