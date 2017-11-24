namespace SolutionLib.Views
{
    using SolutionLib.Interfaces;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Implements DataTemplate Selector for TreeViewItems. This is required
    /// since file entries CANNOT have children and all other types of items
    /// can have child entries below themselves.
    /// 
    /// Usage:
    /// - Instantiate the Template selector in ResourceDictionary
    /// - Instantiate:
    ///   - DataTemplate (for files) and
    ///   - HierarchicalDataTemplate (items with children)
    ///   in ResourceDictionary
    ///   
    /// - Assign each template on the <see cref="FileTemplate"/> and
    ///   <see cref="ChildrenItemTemplate"/>properties below.
    ///   
    /// - Assign the <see cref="TreeViewItemSelector"/> to the TreeView
    ///   ItemTemplateSelector="{StaticResource TreeItemSelector}"
    /// </summary>
    public class TreeViewItemSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets/sets the property that holds the Template for items that cannot
        /// have children (files) in a TreeView.
        /// </summary>
        public DataTemplate FileTemplate { get; set; }

        /// <summary>
        /// Gets/sets the property that holds the Template for items that can
        /// have children (folder, project) in a TreeView.
        /// </summary>
        public DataTemplate ChildrenItemTemplate { get; set; }

        /// <summary>
        /// Overrides the statndard method that is invoked when the framework queries
        /// for the correct Template to be used for a given ViewModel object.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IFile)
                return FileTemplate;

            return ChildrenItemTemplate;
        }
    }
}
