namespace SolutionModelsLib.SQLite
{
    /// <summary>
    /// Models the Pragma Journal_Mode enumeration
    /// - (<see cref="SQLiteDatabase.JournalMode(JournalMode)"/> and <see cref="SQLiteDatabase.JournalMode"/>).
    /// 
    /// See details described below and:
    /// https://sqlite.org/pragma.html#pragma_journal_mode
    /// https://sqlite.org/wal.html
    /// </summary>
    public enum JournalMode
    {
        /// <summary>
        /// The DELETE journaling mode is the normal behavior. In the DELETE mode, the rollback journal is
        /// deleted at the conclusion of each transaction. Indeed, the delete operation is the action that
        /// causes the transaction to commit. (See the document titled Atomic Commit In SQLite for
        /// additional detail.)
        /// </summary>
        DELETE = 0,

        /// <summary>
        /// The TRUNCATE journaling mode commits transactions by truncating the rollback journal
        /// to zero-length instead of deleting it. On many systems, truncating a file is much faster
        /// than deleting the file since the containing directory does not need to be changed.
        /// </summary>
        TRUNCATE = 1,

        /// <summary>
        /// The PERSIST journaling mode prevents the rollback journal from being deleted at the end
        /// of each transaction. Instead, the header of the journal is overwritten with zeros. This
        /// will prevent other database connections from rolling the journal back. The PERSIST journaling
        /// mode is useful as an optimization on platforms where deleting or truncating a file is much
        /// more expensive than overwriting the first block of a file with zeros. See also: PRAGMA
        /// journal_size_limit and SQLITE_DEFAULT_JOURNAL_SIZE_LIMIT.
        /// </summary>
        PERSIST = 2,

        /// <summary>
        /// The MEMORY journaling mode stores the rollback journal in volatile RAM. This saves disk I/O
        /// but at the expense of database safety and integrity. If the application using SQLite crashes
        /// in the middle of a transaction when the MEMORY journaling mode is set, then the database file
        /// will very likely go corrupt.
        /// </summary>
        MEMORY = 3,

        /// <summary>
        /// Indicates the WAL Journal Mode
        /// WAL mode is persisted as documented here: https://sqlite.org/wal.html
        /// </summary>
        WAL = 4,

        /// <summary>
        /// The OFF journaling mode disables the rollback journal completely. No rollback journal is ever
        /// created and hence there is never a rollback journal to delete. The OFF journaling mode disables
        /// the atomic commit and rollback capabilities of SQLite. The ROLLBACK command no longer works; it
        /// behaves in an undefined way. Applications must avoid using the ROLLBACK command when the journal
        /// mode is OFF. If the application crashes in the middle of a transaction when the OFF journaling
        /// mode is set, then the database file will very likely go corrupt.
        /// </summary>
        OFF = 5
    }
}
