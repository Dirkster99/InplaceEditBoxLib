namespace SolutionLib.Interfaces
{
    /// <summary>
    /// Implements a collction specific item (here solution)
    /// that contains base functionalities add with collection
    /// specific properties and functions (e.g. create folder).
    /// </summary>
    public interface ISolutionItem : ISolutionBaseItem
    {
        /// <summary>
        /// Adds another folder (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem AddFolder(string displayName);

        /// <summary>
        /// Adds another project (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem AddProject(string displayName);

        /// <summary>
        /// Adds another file (child) item in the given collection of items.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        ISolutionBaseItem AddFile(string displayName);
    }
}
