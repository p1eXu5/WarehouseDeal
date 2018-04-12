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
        private bool _isInDeal;

        public CategoryHierarchyViewModel (Category category, ObservableCollection<CategoryHierarchyViewModel> categories)
        {
            Category = category;
            Categories = categories;
        }

        public string Id => Category?.Id;
        public string Name => Category?.Name;
        public double? SearchComplexity
        {
            get => Category.SearchComplexity;
            set {
                Category.SearchComplexity = value;
                RaisePropertyChanged ();
            }
        }

        public static string SearchComplexityString { get; } = "Сложность поиска";
        public double? PickingComplexity
        {
            get => Category.PickingComplexity;
            set {
                Category.PickingComplexity = value;
                RaisePropertyChanged ();
            }
        }
        public double? PackagingComplexity
        {
            get => Category.PackagingComplexity;
            set {
                Category.PackagingComplexity = value;
                RaisePropertyChanged ();
            }
        }
        public double? RankingComplexity
        {
            get => Category.RankingComplexity;
            set {
                Category.RankingComplexity = value;
                RaisePropertyChanged ();
            }
        }
        public double? CountingComplexity
        {
            get => Category.CountingComplexity;
            set {
                Category.CountingComplexity = value;
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
        public bool IsInDeal
        {
            get => _isInDeal;
            set {
                _isInDeal = value;
                RaisePropertyChanged ();
            }
        }
    }
}
