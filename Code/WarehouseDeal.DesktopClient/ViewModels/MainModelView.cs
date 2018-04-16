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
using WarehouseDeal.Data.Repositories;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    public class MainModelView : ViewModel
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork ();

        private ObservableCollection<CategoryHierarchyViewModel> _categoryHierarchy;
        //private readonly ComplexityComtext _complexityContext = new ComplexityComtext();
        private bool _isTreeView;
        private bool _isSelectedCategoryNotNull;
        private string _viewContent;
        private CategoryHierarchyViewModel _selectedCategory;
        private List<Complexity> _complexityList;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainModelView()
        {
            _unitOfWork.ComplexityRepository.Load();
            _categoryHierarchy = GetCategoryHierarchy();
            CategoriesHierarchy = new ReadOnlyObservableCollection<CategoryHierarchyViewModel>(_categoryHierarchy);

            SetSelectedItemCommand = new DelegateCommand<CategoryHierarchyViewModel> (SetSelectedCategory);
            ImportCategoriesCommand = new DelegateCommand (ImportFileCategory);
            SetInDealSelectedCategoryCommand = new DelegateCommand (() =>
                                                                        {
                                                                            CategoryHierarchyViewModel.SetInDeal (SelectedCategory,
                                                                                                                  _unitOfWork.ComplexityRepository,
                                                                                                                  _unitOfWork.CategoryComplexityRepository);
                                                                        });

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
                if (value.Id != null) {
                    _selectedCategory = value;
                    RaisePropertyChanged();
                    IsSelectedCategoryNotNull = SelectedCategory != null;
                }
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
        /// <summary>
        /// Text of toggle button
        /// </summary>
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

        /// <summary>
        /// Заполнение иерархии категорий
        /// </summary>
        /// <returns>Иерархию категорий</returns>
        private ObservableCollection<CategoryHierarchyViewModel> GetCategoryHierarchy()
        {
            // Корневая категория TreeView - "Категория"
            var hierarchy = new ObservableCollection<CategoryHierarchyViewModel>()
                                {
                                    new CategoryHierarchyViewModel 
                                    (
                                        category: new Category {Name = "Категория"}, 
                                        categories: GetRootsCategories()
                                    )
                                };

            return hierarchy;
        }

        /// <summary>
        /// Получить корневые категории
        /// </summary>
        /// <returns>Наблюдаемую коллекцию корневых категорий</returns>
        private ObservableCollection<CategoryHierarchyViewModel> GetRootsCategories()
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyRootCategories = new ObservableCollection<CategoryHierarchyViewModel>();
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAllRootCategiriesIncludeCategoryComplexity().ToArray();     // ToArray() - из-за разделения DataAdapter'а

            foreach (Category category in categories) { 


                hierarchyRootCategories.Add (new CategoryHierarchyViewModel (category, GetChildrenCategories (category)));
            }

            return hierarchyRootCategories.Count > 0 ? hierarchyRootCategories : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private ObservableCollection<CategoryHierarchyViewModel> GetChildrenCategories (Category category)
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyCategories = new ObservableCollection<CategoryHierarchyViewModel>();
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetChildrenCategories (category).ToArray();

            foreach (Category childCategiry in categories) {

                hierarchyCategories.Add (new CategoryHierarchyViewModel (childCategiry, GetChildrenCategories (childCategiry)));
            }

            return hierarchyCategories.Count > 0 ? hierarchyCategories : null;
        }

        private ObservableCollection<CategoryComplexityViewModel> GetCategoryComplexityList (Category category)
        {
            var categoryComplexityList = new ObservableCollection<CategoryComplexityViewModel>();
            IEnumerable<CategoryComplexity> categoryComplexities = _unitOfWork.CategoryComplexityRepository.GetAll();

            foreach (CategoryComplexity categoryComplexity in categoryComplexities) {
                
                // TODO:


            }

            return categoryComplexityList;
        }

        private void ImportFileCategory()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {

                string fileName = ofd.FileName;
                ClearCategoriesLists();
                CategoryRepository.LoadCategoriesFromFile (fileName, _unitOfWork.DataContext);
                CategoryHierarchyViewModel rootCategory = _categoryHierarchy.First();
                rootCategory.Categories.Clear();
                rootCategory.Categories.AddRange (GetRootsCategories());
            }
        }

        private void SetSelectedCategory (CategoryHierarchyViewModel item)
        {
            SelectedCategory = item;
        }

        private void ClearCategoriesLists()
        {
            _unitOfWork.CategoryRepository.DeleteAllCategories();
        }


        private void UnsetInDealSelectedCategory()
        {
            var category = SelectedCategory;

        }

    }
}
