namespace SolutionModelsLib
{
    using SolutionModelsLib.Interfaces;
    using SolutionModelsLib.Models;

    /// <summary>
    /// Class creates model objects for the outside world.
    /// </summary>
    public sealed class Factory
    {
        private Factory()
        {
        }

        /// <summary>
        /// Create a solution root data model object and return it.
        /// </summary>
        /// <returns></returns>
        public static ISolutionModel CreateSolutionModel()
        {
            return new SolutionModel();
        }
    }
}
