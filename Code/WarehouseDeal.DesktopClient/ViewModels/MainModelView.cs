using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using WarehouseDeal.BaseClasses;
using WarehouseDeal.Data;
using Prism.Mvvm;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    public class MainModelView : ViewModel
    {
        private readonly CategoryContext _categoryContext = new CategoryContext ();
        private bool _isTreeView;
        private string _viewContent;
        private CategoryHierarchyViewModel _selectedCategory;

        public MainModelView ()
        {
            CategoriesHierarchy = LoadHierarchy();
            //RaisePropertyChanged ("CategoriesHierarchy");

            _selectedCategory = null;

            SetSelectedItemCommand = new DelegateCommand<CategoryHierarchyViewModel> (SetSelectedCategory);
            ImportCategoriesCommand = new DelegateCommand (ImportFileCategory);
            SetInDealSelectedCategoryCommand = new DelegateCommand<CategoryHierarchyViewModel> (CategoryHierarchyViewModel.SetInDealSelectedCategory);

            IsTreeView = true;
        }

        public ObservableCollection<CategoryHierarchyViewModel> CategoriesHierarchy { get; }
        public CategoryHierarchyViewModel SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                RaisePropertyChanged ();
            }
        }
        public DelegateCommand<CategoryHierarchyViewModel> SetSelectedItemCommand { get; }
        public DelegateCommand ImportCategoriesCommand { get; }
        public DelegateCommand<CategoryHierarchyViewModel> SetInDealSelectedCategoryCommand { get; }
        public bool IsTreeView
        {
            get => _isTreeView;
            set
            {
                _isTreeView = value;
                if (_isTreeView)
                    ViewContent = "Представление: Иерархия";
                else
                    ViewContent = "Представление: Таблица";
                RaisePropertyChanged ();
            }
        }
        public string ViewContent
        {
            get => _viewContent;
            set
            {
                _viewContent = value;
                RaisePropertyChanged ();
            }
        }

        public string Name { get; set; }

        // Заполнение иерархии категорий TreeView
        private ObservableCollection<CategoryHierarchyViewModel> LoadHierarchy()
        {
            // Корневая категория TreeView - "Категория"
            var hierarchy = new ObservableCollection<CategoryHierarchyViewModel> ()
                                {
                                    new CategoryHierarchyViewModel (new Category {Name = "Категория"}, LoadRootHierarchy())
                                };

            return hierarchy;
        }
        private ObservableCollection<CategoryHierarchyViewModel> LoadRootHierarchy ()
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyRootCategories = new ObservableCollection<CategoryHierarchyViewModel> ();
            IEnumerable<Category> categories = _categoryContext.GetAllRootCategiries();

            foreach (Category category in categories)
                hierarchyRootCategories.Add (new CategoryHierarchyViewModel (category, LoadChildrenCategories(category)));

            return hierarchyRootCategories.Count > 0 ? hierarchyRootCategories : null;
        }
        private ObservableCollection<CategoryHierarchyViewModel> LoadChildrenCategories (Category category)
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyCategories = new ObservableCollection<CategoryHierarchyViewModel> ();
            IEnumerable<Category> categories = _categoryContext.GetChildrenCategories (category);

            foreach (Category childCategiry in categories) {

                hierarchyCategories.Add (new CategoryHierarchyViewModel (childCategiry, LoadChildrenCategories (childCategiry)));
            }

            return hierarchyCategories.Count > 0 ? hierarchyCategories : null;
        }
        private void ImportFileCategory ()
        {
            OpenFileDialog ofd = new OpenFileDialog ();
            ofd.InitialDirectory = Directory.GetCurrentDirectory ();
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog () == true) {

                string fileName = ofd.FileName;
                _categoryContext.LoadCategoriesFromFile (fileName);
            }
        }
        private void SetSelectedCategory (CategoryHierarchyViewModel item)
        {
            SelectedCategory = item;
        }
        private void ClearCategoriesLists ()
        {
            _categoryContext.DeleteAllCategories ();
        }

            
    }

    
}
