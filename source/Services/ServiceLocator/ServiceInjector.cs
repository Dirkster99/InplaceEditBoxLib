namespace ServiceLocator
{
    using ExplorerLib;
    using ServiceLocator;

    /// <summary>
    /// Creates and initializes all services.
    /// </summary>
    public static class ServiceInjector
    {
        /// <summary>
        /// Loads service objects into the ServiceContainer on startup.
        /// </summary>
        /// <returns>Returns the current <seealso cref="ServiceContainer"/> instance
        /// to let caller work with service container items right after creation.</returns>
        public static ServiceContainer InjectServices()
        {
            ServiceContainer.Instance.AddService<IExplorer>(new Explorer());

            return ServiceContainer.Instance;
        }
    }
}
