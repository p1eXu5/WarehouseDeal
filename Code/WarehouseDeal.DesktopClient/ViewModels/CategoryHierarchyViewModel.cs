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
        public CategoryHierarchyViewModel (CategoryHierarchyViewModel parent, Category category, ObservableCollection<CategoryHierarchyViewModel> categories)
        {
            Parent = parent;
            Category = category;
            Categories = categories;

            if (Category.CategoryComplexity != null)
                foreach (var categoryComplexity in Category.CategoryComplexity) {
                    
                    CategoryComplexityViewModelList.Add (new CategoryComplexityViewModel (categoryComplexity));
                }
        }

        public Category Category { get; }
        public CategoryHierarchyViewModel Parent { get; set; }
        public ObservableCollection<CategoryHierarchyViewModel> Categories { get; internal set; }
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

        
        public static void SetInDeal (CategoryHierarchyViewModel categoryHierarchy, 
                                      IRepository<Complexity,int> complexityRepository, 
                                      IRepository<CategoryComplexity, Tuple<Category, Complexity>> categoryComplexityRepository)
        {
            if (categoryHierarchy?.Id == null || categoryHierarchy.IsInDeal) return;

            // Определяем родителей, не учавствующих в сделке
            ICollection<CategoryHierarchyViewModel> parents = new List<CategoryHierarchyViewModel> {categoryHierarchy};
            CategoryHierarchyViewModel parent = categoryHierarchy.Parent;

            while (parent != null && !parent.IsInDeal && parent.Id != null) {

                parents.Add (parent);
                parent = parent.Parent;
            }

            // Получаем множество всех имеющихся в сделке сложностей, относящихся к категориям
            ISet<Complexity> complexityGlobalSet = new HashSet<Complexity> (complexityRepository.GetAll());

            // Получаем неустановленные сложности выбранной категории

            foreach (CategoryHierarchyViewModel categoryHierarchyViewModel in parents) {
                
                ISet<Complexity> complexityAddingSet = new HashSet<Complexity>(complexityGlobalSet);
                complexityAddingSet.ExceptWith (categoryHierarchyViewModel.CategoryComplexityViewModelList.Select (c => c.Complexity));

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
    }
}
