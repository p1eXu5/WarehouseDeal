using System.Windows;
using WarehouseDeal.DesktopClient.ViewModels;
using WarehouseDeal.DesktopClient.Views;

namespace WarehouseDeal.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup (StartupEventArgs e)
        {
            base.OnStartup (e);

            var viewModel = new CategoriesModelView();

            var window = new CategoriesView
            {
                DataContext = viewModel
            };

            window.ShowDialog();
        }
    }
}
