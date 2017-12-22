namespace SolutionLib
{
    using SolutionLib.Interfaces;
    using SolutionLib.ViewModels.Browser;

    /// <summary>
    /// Contains methods for generating library objects that are exposed via interface only.
    /// </summary>
    public sealed class Factory
    {
        private Factory()
        {
        }

        /// <summary>
        /// Gets the root of a solution tree viewmodel object. Use the functions
        /// available below <see cref="ISolution"/> to manipulate amd mange the
        /// items in that tree.
        /// </summary>
        /// <returns></returns>
        public static ISolution RootViewModel()
        {
            return new SolutionViewModel();
        }
    }
}
