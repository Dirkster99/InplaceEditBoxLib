namespace SolutionLib.Interfaces
{
    using SolutionLib.Models;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// A Solution root is the class that hosts all other solution related items.
    /// Even the SolutionRootItem that is part of the displayed collection is hosted in
    /// the collection below.
    /// </summary>
    public interface ISolution : IViewModelBase
    {
        #region properties
        /// <summary>
        /// Gets the root of the treeview. That is, there is only
        /// 1 item in the ObservableCollection and that item is the root.
        /// 
        /// The Children property of that one <see cref="IItemChildren"/>
        /// represents the rest of the tree.
        /// </summary>
        IEnumerable<IItem> Root { get; }

        /// <summary>
        /// Gets a command that Renames the item that is represented by this viewmodel.
        /// 
        /// This command should be called directly by the implementing view
        /// since the new name of the item is delivered as string with the
        /// item itself as second parameter via bound via RenameCommandParameter
        /// dependency property.
        /// </summary>
        ICommand RenameCommand { get; }

        /// <summary>
        /// Gets the currently selected from the collection of tree items.
        /// </summary>
        IItem SelectedItem { get; }

        /// <summary>
        /// Gets a command that changes the currently <see cref="SelectedItem"/>
        /// to the item that is supplied as <see cref="IItem"/> parameter
        /// of this command.
        /// </summary>
        ICommand SelectionChangedCommand { get; }

        /// <summary>
        /// Gets the file filter that is applied when the user opens a save/load
        /// dialog view to save/load the solution's treeview content.
        /// </summary>
        string SolutionFileFilter { get; }

        /// <summary>
        /// Gets the default file filter that is applied when the user opens a save/load
        /// dialog view to save/load the solution's treeview content for the first time.
        /// </summary>
        string SolutionFileFilterDefault { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Returns the first visible item in the treeview (if any) or null.
        /// 
        /// This method is a convinience wrapper that unwinds the <see cref="Root"/> property
        /// since the viewmodel does support only ONE root at all times.
        /// </summary>
        /// <returns></returns>
        IItemChildren GetRootItem();

        /// <summary>
        /// Adds a solution root into the collection of solution items.
        /// 
        /// Be careful here (!) since the current root item (if any) is discarded
        /// along with all its children since the viewmodel does support only ONE root
        /// at all times.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IItemChildren AddSolutionRootItem(string displayName);

        /// <summary>
        /// Adds another child item below the root item in the collection.
        /// This will throw an Exception if parent is null.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        IItem AddRootChild(string itemName,
                                SolutionItemType itemType);

        /// <summary>
        /// Adds another file (child) item below the parent item.
        /// This will throw an Exception if parent is null.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parent"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        IItem AddChild(string itemName,
                            SolutionItemType itemType,
                            IItemChildren parent);

        /// <summary>
        /// Resets all viewmodel items to initial states of construction time.
        /// </summary>
        void ResetToDefaults();
        #endregion methods
    }
}
