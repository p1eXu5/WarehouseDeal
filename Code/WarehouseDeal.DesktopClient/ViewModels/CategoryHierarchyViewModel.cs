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
    public class CategoryHierarchyViewModel : ViewModel
    {
        public Category Category { get; }
        public ObservableCollection<CategoryHierarchyViewModel> Categories { get; }

        public CategoryHierarchyViewModel (Category category, ObservableCollection<CategoryHierarchyViewModel> categories, ObservableCollection<CategoryComplexityViewModel> categoryComplexityList)
        {
            Category = category;
            Categories = categories;

            CategoryComplexityList = categoryComplexityList;
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
        public ObservableCollection<CategoryComplexityViewModel> CategoryComplexityList { get; set; }
    }
}
