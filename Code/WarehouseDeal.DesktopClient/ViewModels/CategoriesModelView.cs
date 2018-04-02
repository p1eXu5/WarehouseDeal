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

    using static WarehouseDeal.ServiceClasses.WatehouseDealServiceClass;
    using static FileOfCategoriesColumns;
     
    public enum FileOfCategoriesColumns : Int32 { Id, Name, Parent = 3 }



    public class CategoriesModelView : ViewModel
    {
        private readonly BusinessContext _context;
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
        public ICommand GetCategoryListCommand => new ActionCommand (a => SetCategoriesLists());
        #endregion Commands

        #region Methods
        private void SetCategoriesLists()
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
                LoadCategoriesFromFile(fileName);
            }

            SetCategoriesLists();
        }

        private void LoadCategoriesFromFile (string fileName)
        {
            IEnumerable<string[]> lines = GetStringsArrayEnumeratorFromCsvFile (fileName).ToList();

            foreach (var line in lines) {
                if (!IsCategoryString (line)) continue;

                _context.AddNewCategory (line[(int) Id], line[(int) Name]);
            }

            foreach (var line in lines) {
                _context.AddParentCategory (line[(int) Id], line[(int) Parent]);
            }
        }
        #endregion Methods


        public bool IsCategoryString (string[] line)
        {

            return !(line == null || line.Length != 4 || 
                     String.IsNullOrEmpty (line[0]) || line[0].Length != 7 ||
                     String.IsNullOrWhiteSpace (line[1]) ||
                     (!string.IsNullOrEmpty (line[3]) && line[3].Length != 7) ||
                     (!line[2].Equals ("Да") && !line[2].Equals ("Нет")));
        }

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
