namespace InPlaceEditBoxDemo
{
    using InPlaceEditBoxDemo.ViewModels;
    using ServiceLocator;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceInjector.InjectServices();      // Start-up services
            this.DataContext = new AppViewModel();
        }
    }
}
