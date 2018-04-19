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
using WarehouseDeal.Data.Repositories;
using Prism.Mvvm;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    public class CategoryHierarchyViewModel : ViewModel
    {
        #region Constructor
        public CategoryHierarchyViewModel (CategoryHierarchyViewModel parent, 
                                           Category category, 
                                           ObservableCollection<CategoryHierarchyViewModel> childrenCategoryHierarchy, 
                                           IEnumerable<Complexity> complexities = null)
        {
            Parent = parent;                                                    // Родитель
            Category = category;                                                // Категория
            ChilderenCategoryHierarchyViewModels = childrenCategoryHierarchy;   // Дочерние view models

            // TODO: Заполнение сложностей
            if (complexities != null) {

                foreach (var complexity in complexities) {

                    // Есть ли такая установленная сложность у категории
                    CategoryComplexity categoryComplexity = Category.CategoryComplexity.FirstOrDefault (cc => cc.ComplexityId == complexity.Id);

                    if (categoryComplexity == null) {
                        CategoryComplexityViewModelList.Add(new CategoryComplexityViewModel(complexity));
                    }
                    else {
                        CategoryComplexityViewModelList.Add(new CategoryComplexityViewModel(categoryComplexity));
                    }
                }
            }
        }
        #endregion Constructor

        #region Properties
        public Category Category { get; }                       // Underlying Category
        public CategoryHierarchyViewModel Parent { get; set; }  // Родительская модель категории
        public ObservableCollection<CategoryHierarchyViewModel> ChilderenCategoryHierarchyViewModels { get; internal set; }
        public ObservableCollection<CategoryComplexityViewModel> CategoryComplexityViewModelList { get; set; } = new ObservableCollection<CategoryComplexityViewModel>();
        public string Id => Category?.Id;
        public string Name => Category?.Name;
        public bool IsInDeal
        {
            get => Category.IsInDeal;
            set {
                Category.IsInDeal = value;
                RaisePropertyChanged ();
            }
        }
        public bool IsPiecesInDeal
        {
            get => Category.IsPiecesInDeal;
            set {
                Category.IsPiecesInDeal = value;
                RaisePropertyChanged ();
            }
        }
        #endregion Properties

        #region Static Methods
        /// <summary>
        /// Добавление категорий в сделку
        /// </summary>
        /// <param name="categoryHierarchy"></param>
        /// <param name="complexities">Енумератор всех сложностей</param>
        /// <param name="categoryComplexityRepository"></param>
        public static void SetInDeal (CategoryHierarchyViewModel categoryHierarchy, 
                                      IEnumerable<Complexity> complexities, 
                                      IRepository<CategoryComplexity, Tuple<Category, Complexity>> categoryComplexityRepository)
        {
            // Если категория в базе данных не существует, либо категория уже учавствует в сделке, то выйти
            if (categoryHierarchy?.Id == null || categoryHierarchy.IsInDeal) return;

            // Определяем список категорий, не учавствующие в сделке, начиная с исходной в сторону родителей
            ICollection<CategoryHierarchyViewModel> parents = new List<CategoryHierarchyViewModel> {categoryHierarchy};
            CategoryHierarchyViewModel parent = categoryHierarchy.Parent;

            while (parent != null && !parent.IsInDeal && parent.Id != null) {

                parents.Add (parent);
                parent = parent.Parent;
            }

            if (parent == null) {

                parent = parents.Last();
            }

            // Исходные значения сложностей для категорий
            var importComplexities = parent.CategoryComplexityViewModelList.ToArray();

            foreach (CategoryHierarchyViewModel itemCategoryHierarchyViewModel in parents.Reverse()) {

                // Если Категория не является источником значений сложностей
                if (importComplexities[0] != itemCategoryHierarchyViewModel.CategoryComplexityViewModelList[0]) {

                    for (int i = 0; i < itemCategoryHierarchyViewModel.CategoryComplexityViewModelList.Count; ++i) {

                        var itemCategoryComplexityViewModel = itemCategoryHierarchyViewModel.CategoryComplexityViewModelList[i];    // Ссылка на сложность категории

                        // Если сложность не установлена для категории (в бд нет записи):
                        if (itemCategoryComplexityViewModel.IsFake) {

                            double value = 0d;

                            if (!importComplexities[i].IsFake)
                                value = importComplexities[i].Value;

                            var categoryComplexity = new CategoryComplexity()
                            {
                                Category = itemCategoryHierarchyViewModel.Category,
                                Complexity = itemCategoryComplexityViewModel.Complexity,
                                Value = value
                            };
                            categoryComplexityRepository.AddNew (categoryComplexity);
                            itemCategoryComplexityViewModel = new CategoryComplexityViewModel (categoryComplexity);
                        }
                    }
                }
                // Если модель категории является источником значений для сложностей:
                else {

                    // Список нулевых сложностей:
                    var createdComplexities = itemCategoryHierarchyViewModel.CategoryComplexityViewModelList.Where (cc => cc.IsFake);

                    // Подменяем нулевые сложности новыми, добавленными в базу данных для текущей категории:
                    foreach (var itemCategoryComplexityViewModel in createdComplexities) {
                        
                        var newCategoryComplexity = new CategoryComplexity() {Category = itemCategoryHierarchyViewModel.Category, Complexity = itemCategoryComplexityViewModel.Complexity};
                        categoryComplexityRepository.AddNew (newCategoryComplexity);
                        itemCategoryComplexityViewModel.CategoryComplexity = newCategoryComplexity;
                    }
                }

                // Смещаем указатель исходных значений для устанавлеваемых на следующей итерации сложностей следующей категории
                importComplexities = itemCategoryHierarchyViewModel.CategoryComplexityViewModelList.ToArray();

                // Установка флага участника сделки
                itemCategoryHierarchyViewModel.IsInDeal = true;
            }

        }

        /// <summary>
        /// Исключение категорий из сделки
        /// </summary>
        /// <param name="categoryHierarchy"></param>
        public static void UnsetInDeal (CategoryHierarchyViewModel categoryHierarchy)
        {
            if (categoryHierarchy?.Id == null || !categoryHierarchy.IsInDeal) return;

            // Установить поле IsInDeal для переданной категории и всех её детей
            categoryHierarchy.IsInDeal = false;

            if(categoryHierarchy.ChilderenCategoryHierarchyViewModels == null) return;

            foreach (var categoryHierarchyViewModel in categoryHierarchy.ChilderenCategoryHierarchyViewModels) {

                UnsetInDeal (categoryHierarchyViewModel);    
            }
        }
        #endregion Static Methods
    }
}
