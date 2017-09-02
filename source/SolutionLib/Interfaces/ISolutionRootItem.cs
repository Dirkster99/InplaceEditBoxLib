namespace SolutionLib.Interfaces
{
    /// <summary>
    /// Implements an interface for a viewmodel class of the first visible item in the treeview.
    /// Normally, there is only one root in any given tree - so this class implements
    /// that one item visually representing that root (eg.: Computer item in Windows Explorer).
    /// </summary>
    public interface ISolutionRootItem : ISolutionItem
    {
    }
}
