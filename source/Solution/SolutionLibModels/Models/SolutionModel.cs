namespace SolutionModelsLib.Models
{
    using SolutionModelsLib.Enums;
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models.Base;
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// A Solution root is the class that hosts all other solution related items.
    /// Even the SolutionRootItem that is part of the displayed collection is hosted in
    /// the collection below.
    /// </summary>
    internal class SolutionModel : ISolutionModel
    {
        #region constructors
        /// <summary>
        /// Paremeterless standard constructor.
        /// </summary>
        public SolutionModel()
        {
            Version = 1;
            MinorVersion = 0;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the version of this model.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the minor version of this model.
        /// </summary>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// Gets the root of the treeview. That is, there is only
        /// 1 item in the ObservableCollection and that item is the root.
        /// 
        /// The Children property of that one <see cref="SolutionRootItemModel"/>
        /// represent the rest of the tree.
        /// </summary>
        public ISolutionRootItemModel Root { get; set; }
        #endregion properties

        #region methods

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IItemModel AddStaticChild(string displayName
                                 , SolutionModelItemType type
                                 , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            IItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Project:
                    newItem = new ProjectItemModel(parent, displayName);
                    break;

                // This should be created via AddSolutionRootItem() method
                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new ArgumentException(type.ToString());
            }

            parent.AddChild(newItem);

            return newItem;
        }

        /// <summary>
        /// Creates a new solution root item from the given parameters
        /// (replacing the current root item if there is any)
        /// and returns its interface.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IItemChildrenModel AddSolutionRootItem(string displayName, long id = -1)
        {
            Root = new SolutionRootItemModel(displayName) { Id = id };

            return Root;
        }

        /// <summary>
        /// Adds a solution root item (replacing the current root item if there is any)
        /// and returns its interface.
        /// </summary>
        /// <param name="rootItem"></param>
        /// <returns></returns>
        public IItemChildrenModel AddSolutionRootItem(ISolutionRootItemModel rootItem)
        {
            Root = rootItem;

            return Root;
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItemModel AddChild(string displayName
                                 , SolutionModelItemType type
                                 , IItemChildrenModel parent)
        {
            return AddItem(displayName, type, parent);
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// 
        /// This wrapper uses a long input for conversion when reading from file.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IItemModel AddChild(string displayName
                                   , long longType
                                   , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            SolutionModelItemType type = (SolutionModelItemType)longType;

            return AddChild(displayName, type, parent);
        }

        /// <summary>
        /// Adds a child item with the given type
        /// (<see cref="SolutionItemType.SolutionRootItem"/> cannot be added here).
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IItemModel AddItem(string displayName
                                         , SolutionModelItemType type
                                         , IItemChildrenModel parent)
        {
            if (parent.FindChild(displayName) != null)
                throw new ArgumentException("Item '" + displayName + "' already exists.");

            IItemModel newItem = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    newItem = new FileItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Folder:
                    newItem = new FolderItemModel(parent, displayName);
                    break;
                case SolutionModelItemType.Project:
                    newItem = new ProjectItemModel(parent, displayName);
                    break;

                // This should be created via AddSolutionRootItem() method
                case SolutionModelItemType.SolutionRootItem:
                default:
                    throw new ArgumentException(type.ToString());
            }

            parent.AddChild(newItem);

            return newItem;
        }

        #region IXmlSerializable interface
        /// <summary>
        /// Implements the GetSchema() method of the <seealso cref="IXmlSerializable"/> interface
        /// and returns null (or Nothing in Visual Basic).
        /// </summary>
        /// <returns></returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Implements the ReadXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            int i = 0;

            int.TryParse(reader.GetAttribute("Version") , out i);
            Version = i;

            int.TryParse(reader.GetAttribute("MinorVersion"), out i);
            MinorVersion = i;

            reader.ReadStartElement();  // Consume SolutionModel Tag

            reader.MoveToContent();

            // Invoke ReadXml in SolutionRootItemModel to finish this off
            var rootItem = ReadItem(SolutionModelItemType.SolutionRootItem, reader) as ISolutionRootItemModel;
            this.AddSolutionRootItem(rootItem);
        }

        /// <summary>
        /// Implements the WriteXml() method of the <seealso cref="IXmlSerializable"/> interface.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Version", Version.ToString());
            writer.WriteAttributeString("MinorVersion", MinorVersion.ToString());

            // RootItems are written here...
            if (Root != null)
            {
                var rootSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                rootSer.WriteObject(writer, Root);
            }
        }
        #endregion IXmlSerializable interface

        /// <summary>
        /// Reads the indicated item <paramref name="type"/> from the Xml reader's
        /// data stream, advances the Xml reader, and returns the new item.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static IItemModel ReadItem(SolutionModelItemType type
                                          , XmlReader reader)
        {
            IItemModel newItem = null;
            DataContractSerializer itemSer = null;

            switch (type)
            {
                case SolutionModelItemType.File:
                    itemSer = new DataContractSerializer(typeof(FileItemModel));
                    newItem = (FileItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.Folder:
                    itemSer = new DataContractSerializer(typeof(FolderItemModel));
                    newItem = (FolderItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.Project:
                    itemSer = new DataContractSerializer(typeof(ProjectItemModel));
                    newItem = (ProjectItemModel)itemSer.ReadObject(reader);
                    break;

                case SolutionModelItemType.SolutionRootItem:
                    itemSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                    newItem = (SolutionRootItemModel)itemSer.ReadObject(reader);
                    break;

                default:
                    throw new ArgumentException(type.ToString());
            }

            return newItem;
        }

        /// <summary>
        /// Creates a type dependent serializer object that matches the given
        /// <paramref name="item"/> and writes its Xml content into the
        /// Xml <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        internal static void SerializeItem(XmlWriter writer, IItemModel item)
        {
            DataContractSerializer itemSer = null;
            switch (item.ItemType)
            {
                case SolutionModelItemType.SolutionRootItem:
                    itemSer = new DataContractSerializer(typeof(SolutionRootItemModel));
                    break;
                case SolutionModelItemType.File:
                    itemSer = new DataContractSerializer(typeof(FileItemModel));
                    break;
                case SolutionModelItemType.Folder:
                    itemSer = new DataContractSerializer(typeof(FolderItemModel));
                    break;
                case SolutionModelItemType.Project:
                    itemSer = new DataContractSerializer(typeof(ProjectItemModel));
                    break;
                default:
                    throw new System.NotImplementedException(item.ItemType.ToString());
            }

            itemSer.WriteObject(writer, item);
        }

        /// <summary>
        /// Gets the name of the corresponding XML for each type of model
        /// based on an enumerated value.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetXmlName(SolutionModelItemType type)
        {
            switch (type)
            {
                case SolutionModelItemType.SolutionRootItem:
                    return "RootItem";
                case SolutionModelItemType.File:
                    return "File";
                case SolutionModelItemType.Folder:
                    return "Folder";
                case SolutionModelItemType.Project:
                    return "Project";
                default:
                    throw new System.NotImplementedException(type.ToString());
            }
        }

        /// <summary>
        /// An items collection can hold any item derived from <see cref="ItemModel"/>.
        /// It is therefore, the job of this method to determine the correct class and
        /// get its <see cref="IXmlSerializable.ReadXml(XmlReader)"/> method to work.
        /// 
        /// Background Info on ReadXml():
        /// https://docs.microsoft.com/de-de/dotnet/api/system.xml.serialization.ixmlserializable.readxml?view=netframework-4.5.2#System_Xml_Serialization_IXmlSerializable_ReadXml_System_Xml_XmlReader_
        /// 
        /// When this method is called, the reader is positioned on the start tag that wraps
        /// the information for your type. That is, directly on the start tag that indicates
        /// the beginning of a serialized object. When this method returns, it must have read
        /// the entire element from beginning to end, including all of its contents. Unlike the
        /// WriteXml method, the framework does not handle the wrapper element automatically.
        /// Your implementation must do so. Failing to observe these positioning rules may cause
        /// code to generate unexpected runtime exceptions or corrupt data.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parent"></param>
        internal static void ReadItemsCollection(XmlReader reader,
                                                 ItemChildrenModel parent)
        {
            try
            {
                SolutionModelItemType[] childType =  // Elements that can occur in an Items collection
                {
                    SolutionModelItemType.Project
                    ,SolutionModelItemType.Folder
                    ,SolutionModelItemType.File
                };

                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                reader.ReadStartElement("Items");

                reader.MoveToContent();
                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                if (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                    {
                        bool bProcessedElement = false;

                        // Reading a Project, Folder, or File item and adding it to the collection
                        foreach (var item in childType)
                        {
                            if (SolutionModel.GetXmlName(item) == reader.LocalName)
                            {
                                IItemModel newItem = SolutionModel.ReadItem(item, reader);

                                newItem.Parent = parent;
                                parent.AddChild(newItem);

                                bProcessedElement = true;
                                break;
                            }
                        }

                        if (bProcessedElement == false)
                            throw new System.NotSupportedException(reader.LocalName);

                        while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                            reader.Read();
                    }

                    reader.ReadEndElement();
                }

                while (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                    reader.Read();

                reader.ReadEndElement();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion methods
    }
}
