namespace SolutionLib.Interfaces
{
    using System.ComponentModel;

    /// <summary>
    /// This interface provides a base interface for all items in the solution,
    /// including the root item, all items below it, and the solution root.
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
    }
}
