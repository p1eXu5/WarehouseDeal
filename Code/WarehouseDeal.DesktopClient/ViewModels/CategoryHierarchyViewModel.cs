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
            IsInDeal = category.SearchComplexity != null;
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

        public static void SetInDealSelectedCategory (CategoryHierarchyViewModel category)
        {
            ICollection<Category> children = new List<Category> { category.Category };
            Category parent = category.Category.CategoryParent;

            while (parent != null && parent.SearchComplexity == null) {
                children.Add (parent);
                parent = parent.CategoryParent;
            }

            if (parent != null) {

                if (parent.SearchComplexity != null) {

                    // Заполняем сложности всех детей до целевого значениями предка
                    foreach (Category child in children) {

                        child.SearchComplexity = parent.SearchComplexity;
                        child.PickingComplexity = parent.PickingComplexity;
                        child.RankingComplexity = parent.RankingComplexity;
                        child.CountingComplexity = parent.CountingComplexity;
                        child.IsPiecesInDeal = parent.IsPiecesInDeal;

                        return;
                    }
                }

                category.IsPiecesInDeal = parent.IsPiecesInDeal;
            }

            if (category.SearchComplexity != null) return;

            category.SearchComplexity = 0;
            category.PickingComplexity = 0;
            category.RankingComplexity = 0;
            category.CountingComplexity = 0;
        }

        public void UnsetInDealSelectedCategory ()
        {

        }
    }
}
