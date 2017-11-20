namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseItemChildrenModel : BaseItemModel, IBaseItemChildrenModel
    {
        private readonly Dictionary<string, IBaseItemModel> _Children = new Dictionary<string, IBaseItemModel>();

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        protected BaseItemChildrenModel(IBaseItemModel parent
                                        , string displayName
                                        , SolutionModelItemType itemType)
            : base(parent, displayName, itemType)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected BaseItemChildrenModel(SolutionModelItemType itemType)
            : base(itemType)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected BaseItemChildrenModel()
            : base()
        {
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        public IList<IBaseItemModel> Children
        {
            get
            {
                return _Children.Values.ToList();
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Finds a child item based on the given key in <paramref name="displayName"/>.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public IBaseItemModel FindChild(string displayName)
        {
            IBaseItemModel rs = null;

            _Children.TryGetValue(displayName, out rs);

            return rs;
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IBaseItemModel AddChild(IBaseItemChildrenModel parent
                                     , string displayName
                                     , SolutionModelItemType type)
        {
            if (FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            BaseItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.SolutionRootItem:
                    newItem = new SolutionRootItemModel(displayName);
                    break;
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent as IBaseItemModel, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent as IBaseItemModel, displayName);
                    break;
                case SolutionModelItemType.Project:
                    break;

                default:
                    throw new ArgumentException(type.ToString());
            }

            _Children.Add(displayName, newItem);

            return newItem;
        }
        #endregion methods
    }
}
