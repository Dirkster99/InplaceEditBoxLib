namespace InPlaceEditBoxDemo.ViewModels
{
    using SolutionLib.Interfaces;
    using SolutionLib.Models;
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class provides methods to convert from a Solution:
    /// 1) model to a viewmodel
    /// 2) viewmodel to a model
    /// </summary>
    internal class ViewModelModelConverter
    {
        /// <summary>
        /// Method implements Level-Order traversal via <see cref="TreeLib"/> nuget
        /// package to convert a <see cref="ISolution"/> viewmodel into a
        /// <see cref="SolutionModel"/> model.
        /// </summary>
        /// <param name="solutionRoot"></param>
        /// <returns></returns>
        public ISolutionModel ToModel(ISolution solutionRoot)
        {
            IItem treeRootVM = solutionRoot.GetRootItem();
            long itemId = 0;

            var items = TreeLib.BreadthFirst.Traverse.LevelOrder(treeRootVM
                        , (i) =>
                        {
                            var it = i as IItemChildren;

                            if (it != null)
                                return it.Children;

                            // Emulate an emtpy list if items have no children
                            return new List<IItemChildren>();
                        });

            var dstIdItems = new Dictionary<long, IItemChildrenModel>();
            var solutionModel = SolutionModelsLib.Factory.CreateSolutionModel();

            foreach (var item in items.Select(i => i.Node))
            {
                item.SetId(itemId++);

                if (item.Parent == null)
                {
                    solutionModel.AddSolutionRootItem(item.DisplayName, item.GetId());
                    dstIdItems.Add(solutionModel.Root.Id, solutionModel.Root);
                }
                else
                {
                    IItemChildrenModel modelParentItem;
                    IItemModel modelNewChild;
                    dstIdItems.TryGetValue(item.Parent.GetId(), out modelParentItem);

                    modelNewChild = ConvertToModel(solutionModel, modelParentItem, item);

                    modelNewChild.Id = item.GetId();

                    // Store only items that can have children for later lock-up
                    if (modelNewChild is IItemChildrenModel)
                        dstIdItems.Add(modelNewChild.Id, modelNewChild as IItemChildrenModel);
                }
            }

            return solutionModel;
        }

        /// <summary>
        /// Method implements Level-Order traversal via <see cref="TreeLib"/> nuget
        /// package to convert the <see cref="SolutionModel"/> model in <paramref name="model"/>
        /// into a <see cref="ISolution"/> viewmodel in <paramref name="solutionRoot"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="solutionRoot"></param>
        /// <returns></returns>
        public ISolution ToViewModel(ISolutionModel model
                                    ,ISolution solutionRoot)
        {
            solutionRoot.ResetToDefaults(); // Reset current viewmodel to construction time defaults

            ISolutionRootItemModel treeRootModel = model.Root;
            long itemId = 0;

            var items = TreeLib.BreadthFirst.Traverse.LevelOrder<IItemModel>(treeRootModel
                        , (i) =>
                        {
                            var it = i as IItemChildrenModel;

                            if (it != null)
                                return it.Children;

                            // Emulate an emtpy list if items have no children
                            return new List<IItemChildrenModel>();
                        });

            var dstIdItems = new Dictionary<long, IItemChildren>();

            foreach (var item in items.Select(i => i.Node))
            {
                item.Id = itemId++;

                if (item.Parent == null)
                {
                    var rootItem = solutionRoot.AddSolutionRootItem(item.DisplayName);
                    rootItem.SetId(item.Id);

                    dstIdItems.Add(rootItem.GetId(), rootItem);
                }
                else
                {
                    IItemChildren modelParentItem;
                    IItem modelNewChild;
                    dstIdItems.TryGetValue(item.Parent.Id, out modelParentItem);

                    modelNewChild = ConvertToViewModel(solutionRoot, modelParentItem, item);

                    modelNewChild.SetId(item.Id);

                    // Store only items that can have children for later lock-up
                    if (modelNewChild is IItemChildren)
                        dstIdItems.Add(modelNewChild.GetId(), modelNewChild as IItemChildren);
                }
            }

            return solutionRoot;
        }

        /// <summary>
        /// Converts a viewmodel item into a model item and
        /// inserts it into the solution model structure.
        /// </summary>
        /// <param name="solutionModel"></param>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private IItemModel ConvertToModel(
              ISolutionModel solutionModel
            , IItemChildrenModel parent
            , IItem item)
        {
            IItemModel modelNewChild = null;
            switch (item.ItemType)
            {
                case SolutionItemType.File:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionModelItemType.File, parent);
                    break;

                case SolutionItemType.Folder:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionModelItemType.Folder, parent);
                    break;
                case SolutionItemType.Project:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionModelItemType.Project, parent);
                    break;

                case SolutionItemType.SolutionRootItem:
                default:
                    throw new NotSupportedException();
            }

            return modelNewChild;
        }

        /// <summary>
        /// Converts a model item into a viewmodel item and
        /// inserts it into the solution viewmodel structure.
        /// </summary>
        /// <param name="solutionModel"></param>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private IItem ConvertToViewModel(
              ISolution solutionModel
            , IItemChildren parent
            , IItemModel item)
        {
            IItem modelNewChild = null;
            switch (item.ItemType)
            {
                case SolutionModelItemType.File:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionItemType.File, parent);
                    break;

                case SolutionModelItemType.Folder:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionItemType.Folder, parent);
                    break;
                case SolutionModelItemType.Project:
                    modelNewChild = solutionModel.AddChild(item.DisplayName, SolutionItemType.Project, parent);
                    break;

                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new NotSupportedException();
            }

            return modelNewChild;
        }
    }
}
