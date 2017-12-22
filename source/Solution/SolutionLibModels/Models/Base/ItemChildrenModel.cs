namespace SolutionModelsLib.Models.Base
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a model with properties and members of all objects displayed in a solution -
    /// this type also provides basic functions to manage (add, remove, maintain) child items
    /// below this item.
    /// </summary>
    internal abstract class ItemChildrenModel : ItemModel, IItemChildrenModel
    {
        private readonly Dictionary<string, IItemModel> _Children = new Dictionary<string, IItemModel>();

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemChildrenModel(IItemModel parent
                                        , string displayName
                                        , SolutionModelItemType itemType)
            : base(parent, displayName, itemType)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemChildrenModel(SolutionModelItemType itemType)
            : base(itemType)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        protected ItemChildrenModel()
            : base()
        {
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets all children items of this (parent) item.
        /// </summary>
        public IList<IItemModel> Children
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
        public IItemModel FindChild(string displayName)
        {
            IItemModel rs = null;
            _Children.TryGetValue(displayName, out rs);

            return rs;
        }

        public void AddChild(IItemModel newItem)
        {
            _Children.Add(newItem.DisplayName, newItem);
        }

        internal bool RenameChild(string newDisplayName)
        {
            IItemModel rs = null;
            _Children.TryGetValue(newDisplayName, out rs);

            if (rs != null)
            {
                _Children.Remove(rs.DisplayName);
                rs.DisplayName = newDisplayName;
                AddChild(rs);
                return true;
            }

            return false;
        }
        #endregion methods
    }
}
