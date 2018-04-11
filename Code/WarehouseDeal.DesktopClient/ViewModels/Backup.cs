//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WarehouseDeal.DesktopClient.ViewModels
//{
//    public class MainModelView : ViewModel
//    {
//        protected readonly CategoryRepository _context;
//        private SelectedCategory _selectedSelectedCategory;
//        private bool _isTreeView = true;    // Declarate which control is visible now
//        private string _viewContent;        // Content for toggle button

//        #region constructors
//        public MainModelView () : this (new CategoryRepository ()) { }

//        public MainModelView (CategoryRepository context)
//        {
//            _context = context;
//            Categories = new ObservableCollection<SelectedCategory> ();
//            CategoriesHierarchy = new ObservableCollection<CategoryHierarchy> ();
//            SetCategoriesLists ();
//            IsTreeView = true;
//        }
//        #endregion constructors

//        #region Properties
//        public ICollection<SelectedCategory> Categories { get; private set; }
//        public ObservableCollection<CategoryHierarchy> CategoriesHierarchy { get; private set; }
//        public SelectedCategory SelectedSelectedCategory
//        {
//            get => _selectedSelectedCategory;
//            set
//            {
//                _selectedSelectedCategory = value;
//                NotifyPropertyChanged ();
//            }
//        }

//        public string ViewContent
//        {
//            get => _viewContent;
//            set
//            {
//                _viewContent = value;
//                NotifyPropertyChanged ();
//            }
//        }
//        #endregion Properties

//        #region Commands
//        public ICommand ImportCategoriesCommand => new ActionCommand (a => ImportFileCategory ());
//        public ICommand ClearCommand => new ActionCommand (a => ClearCategoriesLists ());
//        public ICommand GetCategoryListCommand => new ActionCommand (a => SetCategoriesLists ());
//        #endregion Commands

//        #region Methods
//        public void SetCategoriesLists ()
//        {
//            GetCategoryHierarchy ();
//            GetCategoryList ();
//        }

//        private void GetCategoryList ()
//        {
//            Categories.Clear ();

//            foreach (SelectedCategory category in _context.GetAllCategories ()) {

//                Categories.Add (category);
//            }
//        }

//        private void GetCategoryHierarchy ()
//        {
//            CategoriesHierarchy.Clear ();

//            foreach (SelectedCategory category in _context.GetAllRootCategiries ()) {

//                CategoriesHierarchy.Add (new CategoryHierarchy { SelectedCategory = category, Categories = LoadChildrenCategories (category) });
//            }
//        }

//        private ObservableCollection<CategoryHierarchy> LoadChildrenCategories (SelectedCategory category)
//        {
//            ObservableCollection<CategoryHierarchy> hierarchyCategories = new ObservableCollection<CategoryHierarchy> ();

//            foreach (SelectedCategory childCategiry in _context.GetChildrenCategories (category)) {

//                hierarchyCategories.Add (new CategoryHierarchy { SelectedCategory = childCategiry, Categories = LoadChildrenCategories (childCategiry) });
//            }

//            return hierarchyCategories.Count > 0 ? hierarchyCategories : null;
//        }


//        public void ImportFileCategory ()
//        {
//            OpenFileDialog ofd = new OpenFileDialog ();
//            ofd.InitialDirectory = Directory.GetCurrentDirectory ();
//            ofd.Filter = "csv files (*.csv)|*.csv";
//            ofd.RestoreDirectory = true;

//            if (ofd.ShowDialog () == true) {

//                string fileName = ofd.FileName;
//                _context.LoadCategoriesFromFile (fileName);
//            }

//            SetCategoriesLists ();
//        }

//        public void ClearCategoriesLists ()
//        {
//            _context.DeleteAllCategories ();
//            Categories.Clear ();
//            CategoriesHierarchy.Clear ();
//        }
//        #endregion Methods




//        public class CategoryHierarchy
//        {
//            public SelectedCategory SelectedCategory { get; }
//            public ReadOnlyObservableCollection<CategoryHierarchy> Categories { get; }
//        }

//        private int ind = 0;
//        public CategoryHierarchy TestCategoryHierarchy => new CategoryHierarchy
//        {
//            SelectedCategory = new SelectedCategory { Name = $"Категория {ind++}" },
//            Categories = new ObservableCollection<CategoryHierarchy>
//                                                                {
//                                                                    new CategoryHierarchy {SelectedCategory = new SelectedCategory {Name = "Первая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категорияПервая категория"}},
//                                                                    new CategoryHierarchy
//                                                                    {
//                                                                        SelectedCategory = new SelectedCategory {Name = "Вторая категория"},
//                                                                        Categories = new ObservableCollection<CategoryHierarchy>
//                                                                        {
//                                                                            new CategoryHierarchy {SelectedCategory = new SelectedCategory {Name = "Вложенная категория"}}
//                                                                        }
//                                                                    }
//                                                                }
//        };

//    }
//}
