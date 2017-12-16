namespace SolutionModelsLib.Models.Base
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;

    public abstract class ItemChildrenModel : ItemModel, IItemChildrenModel
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
        #endregion methods
    }
}
