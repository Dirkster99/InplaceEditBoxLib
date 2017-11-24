namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;

    /// <summary>
    /// Implements an interface to a viewmodel of a folder item in
    /// a tree structured collection of items.
    /// 
    /// Types of collection items can be distinguished via the:
    /// 1) an interface
    ///    (eg. to select a template in an ItemTemplateSelector or
    ///    for usage in an HierarchicalDataTemplate,
    ///    
    /// or
    /// 
    /// 2) thrpigh enumeration in <see cref="SolutionLib.Models.SolutionItemType"/>.
    /// </summary>
    public class FolderItemModel : ItemChildrenModel, IFolderItemModel
    {
        public FolderItemModel(IItemModel parent, string displayName)
            : base(parent, displayName, Enums.SolutionModelItemType.Folder)
        {
        }
    }
}
