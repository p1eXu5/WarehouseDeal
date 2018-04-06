using System.IO;
using System.Windows;

namespace DirectoryView
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string folder = @"..\..\";
            DirectoryRecord root = new DirectoryRecord(new DirectoryInfo(folder));
            treeGrid.DataContext = new DirectoryRecord[] { root };
        }
    }
}
