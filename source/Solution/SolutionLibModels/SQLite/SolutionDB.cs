namespace SolutionModelsLib.SQLite
{
    using SolutionModelsLib.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using SolutionModelsLib.Interfaces;

    /// <summary>
    /// Implements methods and properties necessary to load and save
    /// tree view based data from or to a Sqlite based dataase file.
    /// </summary>
    public class SolutionDB : SQLiteDatabase
    {
        #region MyRegion
        /// <summary>
        /// Class constructor
        /// </summary>
        public SolutionDB()
            : base()
        {
        }
        #endregion

        #region Methods
        #region Write database model
        /// <summary>
        /// Recreates all tables in a database (tables are empty after calling this)
        /// </summary>
        /// <param name="db"></param>
        public void ReCreateDBTables(SQLiteDatabase db = null)
        {
            if (db == null)
                db = this;

            // This table stores the SolutionItemType enumeration for this version of the model 
            string createQuery =
                @"CREATE TABLE IF NOT EXISTS
                    [itemtype] (
                    [id]           INTEGER      NOT NULL PRIMARY KEY,
                    [name]         VARCHAR(256) NOT NULL
                    )";

            // recreate above table if it does not exist, yet
            using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
            {
                cmd.CommandText = createQuery;
                cmd.ExecuteNonQuery();
            }

            // Usage of Foreign Key References is enabled by default
            // Enable its enforcement via connection string "foreign keys=true"
            // This will result in an error thrown when a reference is in-consistent
            // AN ERROR OCURRED: constraint failed
            // FOREIGN KEY constraint failed
            createQuery =
                @"CREATE TABLE IF NOT EXISTS
                    [solution] (
                    [id]           INTEGER      NOT NULL PRIMARY KEY,
                    [parent]       INTEGER      NOT NULL,
                    [level]        INTEGER      NOT NULL,
                    [name]         VARCHAR(256) NOT NULL,
                    [itemtypeid]   INTEGER      NOT NULL,
                    FOREIGN KEY (itemtypeid) REFERENCES itemtype(id)
                    )";

            // recreate above table if it does not exist, yet
            using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
            {
                cmd.CommandText = createQuery;
                cmd.ExecuteNonQuery();
            }

            var cmdDeleteTable = new SQLiteCommand("delete from solution", db.Connection);
            cmdDeleteTable.ExecuteNonQuery();

            cmdDeleteTable = new SQLiteCommand("delete from itemtype", db.Connection);
            cmdDeleteTable.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete all entries in itemtype fact table and
        /// Inserts all current ItemType values into the enumerating fact table.
        /// </summary>
        /// <param name="db"></param>
        public int InsertItemTypeEnumeration(
             string[] names
           , Array values
           , SQLiteDatabase db = null)
        {
            if (db == null)
                db = this;

            int result = 0;

            // Insert into Database
            string query = "INSERT INTO itemtype ([id],[name])VALUES(@id, @name)";

            // Write data out to database
            using (SQLiteCommand cmd = new SQLiteCommand(query, db.Connection))
            {
                // https://www.jokecamp.com/blog/make-your-sqlite-bulk-inserts-very-fast-in-c/
                using (var transaction = cmd.Connection.BeginTransaction())
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@id", values.GetValue(i));
                        cmd.Parameters.AddWithValue("@name", names[i]);

                        result += cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts the data structure items (root item, projects, folders, files) of the solution
        /// into the SQLite database table 'solution'.
        /// </summary>
        /// <param name="solutionRoot"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int InsertSolutionData(ISolutionModel solutionRoot
                                    , SQLiteDatabase db = null)
        {
            int recordCount = 0;

            if (db == null)
                db = this;

            // Insert into Database
            string query = "INSERT INTO solution ([id],[parent],[level],[name],[itemtypeid])VALUES(@id, @parent, @level, @name, @itemtypeid)";

            // Write data out to database
            using (SQLiteCommand cmd = new SQLiteCommand(query, db.Connection))
            {
                // https://www.jokecamp.com/blog/make-your-sqlite-bulk-inserts-very-fast-in-c/
                using (var transaction = cmd.Connection.BeginTransaction())
                {
                    recordCount = WriteToFile(solutionRoot, cmd);

                    transaction.Commit();
                }
            }

            return recordCount;
        }

        /// <summary>
        /// Implements a LevelOrder traversal algorithm to write the tree model data
        /// into a SQLite database file.
        /// </summary>
        /// <param name="solutionRoot"></param>
        private int WriteToFile(ISolutionModel solutionRoot
                              , SQLiteCommand cmd)
        {
            int result = 0;
            int iKey = 0;

            var items = TreeLib.BreadthFirst.Traverse.LevelOrder<IItemModel>(solutionRoot.Root
                        , (i) =>
                        {
                            var it = i as IItemChildrenModel;

                            if (it != null)
                                return it.Children;

                            // Emulate an emtpy list if items have no children
                            return new List<IItemChildrenModel>();
                        });

            foreach (var item in items)
            {
                int iLevel = item.Level;
                IItemModel current = item.Node;
                current.Id = iKey++;

                long parentId = (current.Parent == null ? -1 : current.Parent.Id);

                if (cmd != null)
                {
                    cmd.Parameters.AddWithValue("@id", current.Id);
                    cmd.Parameters.AddWithValue("@parent", parentId);
                    cmd.Parameters.AddWithValue("@level", iLevel);
                    cmd.Parameters.AddWithValue("@name", current.DisplayName);
                    cmd.Parameters.AddWithValue("@itemtypeid", (int)(current.ItemType));

                    result += cmd.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine(string.Format("{0,4} - {1} ({2})"
                        , iLevel, current.GetStackPath(), current.ItemType.ToString()));
                }
            }

            return result;
        }
        #endregion  Write database model

        #region Read from database model
        /// <summary>
        /// Reads the enumeration items from the 'itemtype' table and returns their
        /// names and values in a dictionary (or null if data was not available).
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public Dictionary<long, string> ReadItemTypeEnum(SQLiteDatabase db = null)
        {
            if (db == null)
                db = this;

            Dictionary<long, string> mapKeyToItem = new Dictionary<long, string>();

            var query = "SELECT [id],[name] FROM itemtype ORDER BY id";
            using (SQLiteCommand cmd = new SQLiteCommand(query, db.Connection))
            {
                using (SQLiteDataReader selectResult = cmd.ExecuteReader())
                {
                    if (selectResult.HasRows == false)
                        return null;

                    while (selectResult.Read() == true)
                    {
                        long iKey = (long)selectResult["id"];
                        string name = selectResult["name"].ToString();

                        mapKeyToItem.Add(iKey, name);
                    }
                }
            }

            return mapKeyToItem;
        }

        /// <summary>
        /// Read the Tree Model data structure from a SQLite database file back into memory.
        /// </summary>
        /// <param name="solutionRoot"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int ReadSolutionData(ISolutionModel solutionRoot
                                  , SQLiteDatabase db = null)
        {
            if (db == null)
                db = this;

            int recordCount = 0;

            var query = "SELECT * FROM solution ORDER BY level, id";
            using (SQLiteCommand cmd = new SQLiteCommand(query, db.Connection))
            {
                using (SQLiteDataReader selectResult = cmd.ExecuteReader())
                {
                    Dictionary<long, IItemChildrenModel> mapKeyToItem = new Dictionary<long, IItemChildrenModel>();

                    if (selectResult.HasRows == true)
                    {
                        if (selectResult.Read() == true)
                        {
                            var root = solutionRoot.AddSolutionRootItem(selectResult["name"].ToString());

                            // .GetInt32(0) gets the Id of Root entry
                            mapKeyToItem.Add(selectResult.GetInt32(0), root);
                            recordCount++;
                        }

                        while (selectResult.Read() == true)
                        {
                            int iParentKey = selectResult.GetInt32(1); // Get parent key from next item
                            IItemChildrenModel parent = null;

                            if (mapKeyToItem.TryGetValue(iParentKey, out parent) == true)
                            {
                                var itemTypeId = (long)selectResult["itemtypeid"];

                                var item = solutionRoot.AddChild(selectResult["name"].ToString()
                                                               , itemTypeId, parent);

                                IItemChildrenModel parentItem = null;
                                if ((parentItem = item as IItemChildrenModel) != null)
                                {
                                    mapKeyToItem.Add(selectResult.GetInt32(0), parentItem);
                                }
                                recordCount++;
                            }
                            else
                            {
                                throw new Exception("Data written is corrupted.");
                            }
                        }
                    }
                }
            }

            return recordCount;
        }
        #endregion Read from database model
        #endregion Methods
    }
}
