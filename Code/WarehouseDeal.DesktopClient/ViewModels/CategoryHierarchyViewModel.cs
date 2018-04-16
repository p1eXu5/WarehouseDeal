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

        public Category Category { get; }
        public ObservableCollection<CategoryHierarchyViewModel> Categories { get; }


        public CategoryHierarchyViewModel (Category category, ObservableCollection<CategoryHierarchyViewModel> categories)
        {
            Category = category;
            Categories = categories;

            if (Category.CategoryComplexity != null)
                foreach (var categoryComplexity in Category.CategoryComplexity) {
                    
                    CategoryComplexityList.Add (new CategoryComplexityViewModel (categoryComplexity));
                }
        }

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
        public ObservableCollection<CategoryComplexityViewModel> CategoryComplexityList { get; set; } = new ObservableCollection<CategoryComplexityViewModel>();

        
        public static void SetInDeal (CategoryHierarchyViewModel categoryHierarchy, IRepository<Complexity,int> complexityRepository, IRepository<CategoryComplexity, Tuple<Category, Complexity>> categoryComplexityRepository)
        {
            // Получаем множество всех имеющихся в сделке сложностей, относящихся к категориям
            ISet<Complexity> complexityAddingSet = new HashSet<Complexity> (complexityRepository.GetAll());

            // Получаем неустановленные сложности выбранной категории
            complexityAddingSet.ExceptWith (categoryHierarchy.CategoryComplexityList.Select (c => c.Complexity));

            foreach (var complexity in complexityAddingSet) {

                CategoryComplexity newCategoryComplexity = new CategoryComplexity { Category = categoryHierarchy.Category, Complexity = complexity };
                categoryComplexityRepository.AddNew (newCategoryComplexity);
                categoryHierarchy.CategoryComplexityList.Add (new CategoryComplexityViewModel(newCategoryComplexity));
            }

            categoryHierarchy.IsInDeal = true;
        }
    }
}
