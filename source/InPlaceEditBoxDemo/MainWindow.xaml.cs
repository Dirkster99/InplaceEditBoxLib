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

            Loaded += MainWindow_LoadedAsync;
        }

        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_LoadedAsync;

            var appVM = new AppViewModel();
            this.DataContext = appVM;

            appVM.ResetDefaults();
            await appVM.LoadSampleDataAsync();
        }
    }
}
