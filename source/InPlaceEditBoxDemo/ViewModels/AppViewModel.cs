namespace InPlaceEditBoxDemo.ViewModels
{
    internal class AppViewModel : Base.BaseViewModel
    {
        private readonly SolutionLib.Interfaces.ISolution _SolutionBrowser;

        public AppViewModel()
        {
            _SolutionBrowser = SolutionLib.Factory.RootViewModel();
            Demo.Create.Objects(_SolutionBrowser);
        }

        public SolutionLib.Interfaces.ISolution Solution
        {
            get { return _SolutionBrowser; }
        }
    }
}

