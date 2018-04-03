using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WarehouseDeal.BaseClasses;
using WarehouseDeal.Data;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    public class CategoriesModelView : ViewModel
    {
        protected readonly BusinessContext _context;
        private Category _selectedCategory;
        private bool _isTreeView = true;    // Declarate which control is visible now
        private string _viewContent;        // Content for toggle button

        #region constructors
        public CategoriesModelView() : this(new BusinessContext()) { }

        public CategoriesModelView (BusinessContext context)
        {
            _context = context;
            Categories = new ObservableCollection<Category>();
            CategoriesHierarchy = new ObservableCollection<CategoryHierarchy>();
            SetCategoriesLists ();
            IsTreeView = true;
        }
        #endregion constructors

        #region Properties
        public ICollection<Category> Categories { get; private set; }
        public ObservableCollection<CategoryHierarchy> CategoriesHierarchy { get; private set; }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                NotifyPropertyChanged ();
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
                NotifyPropertyChanged ();
            }
        }
        public string ViewContent
        {
            get => _viewContent;
            set {
                _viewContent = value;
                NotifyPropertyChanged ();
            }
        }
        #endregion Properties

        #region Commands
        public ICommand ImportCommand => new ActionCommand (a => ImportFileCategory());
        public ICommand ClearCommand => new ActionCommand (a => ClearCategoriesLists ());
        public ICommand GetCategoryListCommand => new ActionCommand (a => SetCategoriesLists());
        #endregion Commands

        #region Methods
        public void SetCategoriesLists()
        {
            GetCategoryHierarchy();
            GetCategoryList();
        }

        private void GetCategoryList()
        {
            Categories.Clear();

            foreach (Category category in _context.GetAllCategories()) {
                
                Categories.Add (category);
            }
        }

        private void GetCategoryHierarchy()
        {
            CategoriesHierarchy.Clear();

            foreach (Category category in _context.GetAllRootCategiries()) {

                CategoriesHierarchy.Add (new CategoryHierarchy {Category = category, Categories = LoadChildrenCategories (category)});
            }
        }

        private ObservableCollection<CategoryHierarchy> LoadChildrenCategories (Category category)
        {
            ObservableCollection<CategoryHierarchy> hierarchyCategories = new ObservableCollection<CategoryHierarchy>();

            foreach (Category childCategiry in _context.GetChildrenCategories (category)) {
                
                hierarchyCategories.Add (new CategoryHierarchy { Category = childCategiry, Categories = LoadChildrenCategories (childCategiry) });
            }

            return hierarchyCategories.Count > 0 ? hierarchyCategories: null;
        }


        public void ImportFileCategory ()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {

                string fileName = ofd.FileName;
                _context.LoadCategoriesFromFile(fileName);
            }

            SetCategoriesLists();
        }

        public void ClearCategoriesLists()
        {
            _context.DeleteAllCategories();
            Categories.Clear();
            CategoriesHierarchy.Clear();
        }
        #endregion Methods


        

        public class CategoryHierarchy
        {
            public Category Category { get; set; }
            public ObservableCollection<CategoryHierarchy> Categories { get; set; }
        }

        private int ind = 0;
        public CategoryHierarchy TestCategoryHierarchy => new CategoryHierarchy
                                                            {
                                                                Category = new Category {Name = $"Категория {ind++}"},
                                                                Categories = new ObservableCollection<CategoryHierarchy>
                                                                {
                                                                    new CategoryHierarchy {Category = new Category {Name = "Первая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категория"}},
                                                                    new CategoryHierarchy
                                                                    {
                                                                        Category = new Category {Name = "Вторая категория"},
                                                                        Categories = new ObservableCollection<CategoryHierarchy>
                                                                        {
                                                                            new CategoryHierarchy {Category = new Category {Name = "Вложенная категория"}}
                                                                        }
                                                                    }
                                                                }
                                                            };
        
    }
}
