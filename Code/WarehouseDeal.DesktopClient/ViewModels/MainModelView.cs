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
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private ObservableCollection<CategoryHierarchyViewModel> _categoryHierarchy;
        private ObservableCollection<CategoryHierarchyViewModel> _categoryTable;
        private bool _isTreeView;
        private bool _isSelectedCategoryNotNull;
        private string _viewContent;
        private CategoryHierarchyViewModel _selectedCategory;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainModelView()
        {
            // Заполнение иерархии категорий:
            _categoryHierarchy = GetCategoryHierarchy();
            CategoriesHierarchy = new ReadOnlyObservableCollection<CategoryHierarchyViewModel> (_categoryHierarchy);

            // Заполнение таблицы категорий из иерархии:
            _categoryTable = new ObservableCollection<CategoryHierarchyViewModel>();

            if (_categoryHierarchy[0].ChilderenCategoryHierarchyViewModels != null)
                foreach (var categoryHierarchyViewModel in _categoryHierarchy[0].ChilderenCategoryHierarchyViewModels) {

                    _categoryTable.AddRange (GetCategoryTable (categoryHierarchyViewModel));
                }

            CategoriesTable = new ReadOnlyObservableCollection<CategoryHierarchyViewModel>(_categoryTable);

            // Определение команд:
            SetSelectedItemCommand = new DelegateCommand<CategoryHierarchyViewModel> (item => SelectedCategory = item);
            ImportCategoriesCommand = new DelegateCommand (ImportFileCategory);
            SetInDealSelectedCategoryCommand = new DelegateCommand (() => 
                                    { CategoryHierarchyViewModel.SetInDeal (SelectedCategory, Complexities, _unitOfWork.CategoryComplexityRepository); });

            UnsetInDealSelectedCategoryCommand = new DelegateCommand (() => { CategoryHierarchyViewModel.UnsetInDeal (SelectedCategory); });

            // Установка свойств:
            IsTreeView = true;
            IsSelectedCategoryNotNull = SelectedCategory != null;
        }


        #region Properties

        public Complexity[] Complexities => _unitOfWork.ComplexityRepository.GetAll().ToArray();
        public ReadOnlyObservableCollection<CategoryHierarchyViewModel> CategoriesHierarchy { get; }
        public ReadOnlyObservableCollection<CategoryHierarchyViewModel> CategoriesTable { get; }
        public CategoryHierarchyViewModel SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                RaisePropertyChanged();
                IsSelectedCategoryNotNull = value?.Id != null;
            }
        }

        public bool IsTreeView
        {
            get => _isTreeView;
            set {
                _isTreeView = value;
                RaisePropertyChanged();
                if (_isTreeView)
                    ViewContent = "Представление: Иерархия";
                else
                    ViewContent = "Представление: Таблица";
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

        public bool IsSelectedCategoryNotNull
        {
            get => _isSelectedCategoryNotNull;
            set {
                _isSelectedCategoryNotNull = value;
                RaisePropertyChanged();
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
                new CategoryHierarchyViewModel (parent: null, category: new Category {Name = "Категория"}, childrenCategoryHierarchy: GetRootsCategories())
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
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAllRootIncludeCollections().ToList(); // ToList() - из-за разделения DataAdapter'а

            foreach (Category category in categories) {

                var categoryHierarchyViewModel = new CategoryHierarchyViewModel (null, category, null, Complexities);
                categoryHierarchyViewModel.ChilderenCategoryHierarchyViewModels = GetChildrenCategories (categoryHierarchyViewModel);
                hierarchyRootCategories.Add (categoryHierarchyViewModel);
            }

            return hierarchyRootCategories.Count > 0 ? hierarchyRootCategories : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryHierarchyViewModelParent"></param>
        /// <returns></returns>
        private ObservableCollection<CategoryHierarchyViewModel> GetChildrenCategories (CategoryHierarchyViewModel categoryHierarchyViewModelParent)
        {
            ObservableCollection<CategoryHierarchyViewModel> hierarchyCategories = new ObservableCollection<CategoryHierarchyViewModel>();
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetChildrenCategories (categoryHierarchyViewModelParent.Category).ToArray();

            foreach (Category childCategiry in categories) {

                var categoryHierarchyViewModel = new CategoryHierarchyViewModel (categoryHierarchyViewModelParent, childCategiry, null, Complexities);
                categoryHierarchyViewModel.ChilderenCategoryHierarchyViewModels = GetChildrenCategories (categoryHierarchyViewModel);
                hierarchyCategories.Add (categoryHierarchyViewModel);
            }

            return hierarchyCategories.Count > 0 ? hierarchyCategories : null;
        }

        /// <summary>
        /// Заполнение таблицы категорий
        /// </summary>
        /// <param name="categoryHierarchyViewModel"></param>
        /// <returns></returns>
        private IEnumerable<CategoryHierarchyViewModel> GetCategoryTable(CategoryHierarchyViewModel categoryHierarchyViewModel)
        {
            var categoryTable = new Collection<CategoryHierarchyViewModel>() {categoryHierarchyViewModel};

            if (categoryHierarchyViewModel.ChilderenCategoryHierarchyViewModels != null) {

                foreach (var categoryHierarchy in categoryHierarchyViewModel.ChilderenCategoryHierarchyViewModels) {
                
                    categoryTable.AddRange (GetCategoryTable(categoryHierarchy));
                }
            }

            return categoryTable;
        }

        /// <summary>
        /// Импорт категорий из файла.
        /// </summary>
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
                rootCategory.ChilderenCategoryHierarchyViewModels.Clear();
                rootCategory.ChilderenCategoryHierarchyViewModels.AddRange (GetRootsCategories());
            }
        }

        private void ClearCategoriesLists()
        {
            _unitOfWork.CategoryRepository.DeleteAllCategories();
        }

    }
}
