namespace InPlaceEditBoxDemo
{
    using InPlaceEditBoxDemo.ViewModels;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new AppViewModel();
        }
    }
}
