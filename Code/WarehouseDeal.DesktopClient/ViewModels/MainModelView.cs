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
        private CategoryViewModel _selectedCategory;

        public MainModelView ()
        {
            Categories = new ObservableCollection<CategoryViewModel>(_categoryContext.Categories.Select (c => new CategoryViewModel (c)));
            CategoriesHierarchy = LoadHierarchy();
            RaisePropertyChanged ("CategoriesHierarchy");

            SetSelectedItemCommand = new DelegateCommand<CategoryHierarchy> (SetSelectedCategory);
            ImportCategoriesCommand = new DelegateCommand (ImportFileCategory);

            IsTreeView = true;
        }

        public ObservableCollection<CategoryViewModel> Categories { get; }
        public CategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                RaisePropertyChanged ();
            }
        }
        public ObservableCollection<CategoryHierarchy> CategoriesHierarchy { get; }
        public DelegateCommand<CategoryHierarchy> SetSelectedItemCommand { get; }
        public DelegateCommand ImportCategoriesCommand { get; }
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

        private ObservableCollection<CategoryHierarchy> LoadHierarchy()
        {
            var hierarchy = new ObservableCollection<CategoryHierarchy>()
                                {
                                    new CategoryHierarchy
                                    {
                                        Category = new CategoryViewModel (new Category {Name = "Категория"}),
                                        Categories = LoadRootHierarchy()
                                    }
                                };

            return hierarchy;
        }

        private ObservableCollection<CategoryHierarchy> LoadRootHierarchy ()
        {
            ObservableCollection<CategoryHierarchy> hierarchyRootCategories = new ObservableCollection<CategoryHierarchy> ();
            foreach (Category category in _categoryContext.GetAllRootCategiries())
                hierarchyRootCategories.Add (new CategoryHierarchy
                                                {
                                                    Category = new CategoryViewModel (category),
                                                    Categories = LoadChildrenCategories (category)
                                                });

            return hierarchyRootCategories.Count > 0 ? hierarchyRootCategories : null;
        }

        private ObservableCollection<CategoryHierarchy> LoadChildrenCategories (Category category)
        {
            ObservableCollection<CategoryHierarchy> hierarchyCategories = new ObservableCollection<CategoryHierarchy> ();

            foreach (Category childCategiry in _categoryContext.GetChildrenCategories (category)) {
                hierarchyCategories.Add (new CategoryHierarchy
                                            {
                                                Category = new CategoryViewModel (childCategiry),
                                                Categories = LoadChildrenCategories (childCategiry)
                                            });
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

        private void SetSelectedCategory (CategoryHierarchy item)
        {
            SelectedCategory = item.Category;

        }

        private void ClearCategoriesLists ()
        {
            _categoryContext.DeleteAllCategories ();
        }


        public class CategoryHierarchy
        {
            public CategoryViewModel Category { get; set; }
            public ObservableCollection<CategoryHierarchy> Categories { get; set; }
        }      
    }

    public class CategoryViewModel : ViewModel
    {
        public Category Category { get; }

        public CategoryViewModel (Category category)
        {
            Category = category;
            IsInDeal = category.SearchComplexity != null;
        }

        public string Id => Category.Id;
        public string Name => Category.Name;
        private bool _isInDeal;
        public bool IsInDeal { get => _isInDeal;
            set {
                _isInDeal = value;
                RaisePropertyChanged ();
            }
        }
    }
}
