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
        private readonly CategoryContext _categoryContext = new CategoryContext();
        private ObservableCollection<CategoryHierarchyViewModel> _categoriesHierarchy;
        //private readonly ComplexityComtext _complexityContext = new ComplexityComtext();
        private bool _isTreeView;
        private bool _isSelectedCategoryNotNull;
        private string _viewContent;
        private CategoryHierarchyViewModel _selectedCategory;
        private List<Complexity> _complexityList;

        public MainModelView()
        {
            _categoriesHierarchy = LoadHierarchy();
            CategoriesHierarchy = new ReadOnlyObservableCollection<CategoryHierarchyViewModel>(_categoriesHierarchy);

            //_complexityContext = LoadComplexities();

            SetSelectedItemCommand = new DelegateCommand<CategoryHierarchyViewModel> (SetSelectedCategory);
            ImportCategoriesCommand = new DelegateCommand (ImportFileCategory);
            SetInDealSelectedCategoryCommand = new DelegateCommand (SetInDealSelectedCategory);
            UnsetInDealSelectedCategoryCommand = new DelegateCommand (UnsetInDealSelectedCategory);

            IsTreeView = true;
            IsSelectedCategoryNotNull = SelectedCategory != null;
        }


        #region Properties

        public ReadOnlyObservableCollection<CategoryHierarchyViewModel> CategoriesHierarchy { get; }
        public CategoryHierarchyViewModel SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                RaisePropertyChanged();
                IsSelectedCategoryNotNull = SelectedCategory != null;
            }
        }
        public bool IsTreeView
        {
            get => _isTreeView;
            set {
                _isTreeView = value;
                if (_isTreeView)
                    ViewContent = "Представление: Иерархия";
                else
                    ViewContent = "Представление: Таблица";
                RaisePropertyChanged();
            }
        }
        public string ViewContent
        {
            get => _viewContent;
            set {
                _viewContent = value;
                RaisePropertyChanged();
            }
        }
        public string Name { get; set; }
        public bool IsSelectedCategoryNotNull
        {
            get => _isSelectedCategoryNotNull;
            set {
                _isSelectedCategoryNotNull = value;
                RaisePropertyChanged ();
            }
        }

        #endregion

        #region Commands
        public DelegateCommand<CategoryHierarchyViewModel> SetSelectedItemCommand { get; }
        public DelegateCommand ImportCategoriesCommand { get; }
        public DelegateCommand SetInDealSelectedCategoryCommand { get; }
        public DelegateCommand UnsetInDealSelectedCategoryCommand { get; }
        #endregion

        // Заполнение иерархии категорий TreeView
        private ObservableCollection<CategoryHierarchyViewModel> LoadHierarchy()
        {
            // Корневая категория TreeView - "Категория"
            var hierarchy = new ObservableCollection<CategoryHierarchyViewModel>()
            {
                new CategoryHierarchyViewModel (new Category {Name = "Категория"}, LoadRootHierarchy())
            };

            return hierarchy;
        }
        private ObservableCollection<CategoryHierarchyViewModel> LoadRootHierarchy()
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyRootCategories = new ObservableCollection<CategoryHierarchyViewModel>();
            IEnumerable<Category> categories = _categoryContext.GetAllRootCategiries();

            foreach (Category category in categories)
                hierarchyRootCategories.Add (new CategoryHierarchyViewModel (category, LoadChildrenCategories (category)));

            return hierarchyRootCategories.Count > 0 ? hierarchyRootCategories : null;
        }
        private ObservableCollection<CategoryHierarchyViewModel> LoadChildrenCategories (Category category)
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyCategories =
                new ObservableCollection<CategoryHierarchyViewModel>();
            IEnumerable<Category> categories = _categoryContext.GetChildrenCategories (category);

            foreach (Category childCategiry in categories) {

                hierarchyCategories.Add (new CategoryHierarchyViewModel (childCategiry,
                    LoadChildrenCategories (childCategiry)));
            }

            return hierarchyCategories.Count > 0 ? hierarchyCategories : null;
        }

        private void ImportFileCategory()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {

                string fileName = ofd.FileName;
                _categoryContext.LoadCategoriesFromFile (fileName);
            }
        }
        private void SetSelectedCategory (CategoryHierarchyViewModel item)
        {
            SelectedCategory = item;
        }
        private void ClearCategoriesLists()
        {
            _categoryContext.DeleteAllCategories();
        }
        private void SetInDealSelectedCategory()
        {
            var category = SelectedCategory;
            ICollection<Category> children = new List<Category> {category.Category};
            Category parent = category.Category.CategoryParent;

            while (parent != null && parent.SearchComplexity == null) {
                children.Add (parent);
                parent = parent.CategoryParent;
            }

            if (parent != null) {

                if (parent.SearchComplexity != null) {

                    // Заполняем сложности всех детей до целевого значениями предка
                    foreach (Category child in children) {

                        child.SearchComplexity = parent.SearchComplexity;
                        child.PickingComplexity = parent.PickingComplexity;
                        child.RankingComplexity = parent.RankingComplexity;
                        child.CountingComplexity = parent.CountingComplexity;
                        child.IsPiecesInDeal = parent.IsPiecesInDeal;

                        return;
                    }
                }

                category.IsPiecesInDeal = parent.IsPiecesInDeal;
            }

            if (category.SearchComplexity != null) return;

            category.SearchComplexity = 0;
            category.PickingComplexity = 0;
            category.RankingComplexity = 0;
            category.CountingComplexity = 0;
        }
        private void UnsetInDealSelectedCategory()
        {
            var category = SelectedCategory;

        }

        public struct Complexity
        {
            public string Title { get; }
            public double MinComplexity { get; }
            public double MaxComplexity { get; }
        }
    }
}
