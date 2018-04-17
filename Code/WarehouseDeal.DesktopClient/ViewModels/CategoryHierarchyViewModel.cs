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
        public CategoryHierarchyViewModel (CategoryHierarchyViewModel parent, 
                                           Category category, 
                                           ObservableCollection<CategoryHierarchyViewModel> childrenCategoryHierarchy, 
                                           IEnumerable<Complexity> complexities = null)
        {
            Parent = parent;                                                    // Родитель
            Category = category;                                                // Категория
            ChilderenCategoryHierarchyViewModels = childrenCategoryHierarchy;   // Дочерние view models

            // TODO: Заполнение сложностей
            if (Category.CategoryComplexity != null)
                foreach (var categoryComplexity in Category.CategoryComplexity) {
                    
                    CategoryComplexityViewModelList.Add (new CategoryComplexityViewModel (categoryComplexity));
                }
        }

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

        /// <summary>
        /// Добавление категорий в сделку
        /// </summary>
        /// <param name="categoryHierarchy"></param>
        /// <param name="complexityRepository"></param>
        /// <param name="categoryComplexityRepository"></param>
        public static void SetInDeal (CategoryHierarchyViewModel categoryHierarchy, 
                                      IRepository<Complexity,int> complexityRepository, 
                                      IRepository<CategoryComplexity, Tuple<Category, Complexity>> categoryComplexityRepository)
        {
            if (categoryHierarchy?.Id == null || categoryHierarchy.IsInDeal) return;

            // Определяем категории, не учавствующие в сделке, начиная с исходной в сторону родителей
            ICollection<CategoryHierarchyViewModel> parents = new List<CategoryHierarchyViewModel> {categoryHierarchy};
            CategoryHierarchyViewModel parent = categoryHierarchy.Parent;

            while (parent != null && !parent.IsInDeal && parent.Id != null) {

                parents.Add (parent);
                parent = parent.Parent;
            }

            // Получаем список всех имеющихся в сделке сложностей, относящихся к категориям
            IEnumerable<Complexity> complexityGlobalSet = complexityRepository.GetAll().ToList();

            foreach (CategoryHierarchyViewModel categoryHierarchyViewModel in parents) {
                
                ISet<Complexity> complexityAddingSet = new HashSet<Complexity>(complexityGlobalSet);    // Обновляем множество всех категорий из списка
                complexityAddingSet.ExceptWith (categoryHierarchyViewModel.CategoryComplexityViewModelList.Select (c => c.Complexity));     // Получаем неустановленные сложности выбранной категории

                foreach (var complexity in complexityAddingSet) {

                    CategoryComplexity newCategoryComplexity = new CategoryComplexity { Category = categoryHierarchyViewModel.Category, Complexity = complexity }; // Экземпляр сущности БД

                    var parentCategoryComplexityViewModel = parent?.CategoryComplexityViewModelList.FirstOrDefault (cc => cc.Complexity == complexity);

                    if (parentCategoryComplexityViewModel != null)
                        newCategoryComplexity.Value = parentCategoryComplexityViewModel.Value;

                    categoryComplexityRepository.AddNew (newCategoryComplexity);                                                                                // Вносим экземпляр в таблицу
                    categoryHierarchyViewModel.CategoryComplexityViewModelList.Add (new CategoryComplexityViewModel(newCategoryComplexity));                       // Вносим экземпляр во ViewModel
                }

                categoryHierarchyViewModel.IsInDeal = true;
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
    }
}
