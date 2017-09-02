namespace SolutionLib.Interfaces
{
    using SolutionLib.Models;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// A Solution root is the class that hosts all other solution related items.
    /// Even the SolutionRootItem that is part of the displayed collection is hosted in
    /// the collection below.
    /// </summary>
    public interface ISolution : INotifyPropertyChanged
    {
        #region properties
        /// <summary>
        /// Gets the root of the treeview. That is, there is only
        /// 1 item in the ObservableCollection and that item is the root.
        /// 
        /// The Children property of that one <see cref="ISolutionItem"/>
        /// represents the rest of the tree.
        /// </summary>
        ObservableCollection<ISolutionBaseItem> Root { get; }

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
        ISolutionBaseItem SelectedItem { get; }

        /// <summary>
        /// Gets a command that changes the currently <see cref="SelectedItem"/>
        /// to the item that is supplied as <see cref="ISolutionBaseItem"/> parameter
        /// of this command.
        /// </summary>
        ICommand SelectionChangedCommand { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Adds a solution root into the collection of solution items.
        /// 
        /// Be careful here (!) since the current root item (if any) is discarded
        /// along with all its children since the viewmodel does support only ONE root
        /// at all times.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem AddSolutionRootItem(string displayName);

        /// <summary>
        /// Adds another child item below the root item in the collection.
        /// This will throw an Exception if parent is null.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        ISolutionBaseItem AddRootChild(string itemName,
                                        SolutionItemType itemType);

        /// <summary>
        /// Adds another file (child) item below the parent item.
        /// This will throw an Exception if parent is null.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parent"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        ISolutionBaseItem AddChild(string itemName,
                                    SolutionItemType itemType,
                                    ISolutionBaseItem parent);
        #endregion methods
    }
}
