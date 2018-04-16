using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Mvvm;
using WarehouseDeal.Data;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    public class CategoryComplexityViewModel : BindableBase
    {
        private readonly CategoryComplexity _categoryComplexity;
        private readonly Complexity _complexity;

        public CategoryComplexityViewModel (CategoryComplexity categoryComplexity)
        {
            _categoryComplexity = categoryComplexity;
            _complexity = _categoryComplexity.Complexity;
        }

        public Complexity Complexity  => _complexity;

        public string Title
        {
            get => _complexity.Title;
            set {
                _complexity.Title = value;
                RaisePropertyChanged();
            }
        }

        public double MinComplexity
        {
            get => _complexity.MinComplexity;
            set {
                _complexity.MinComplexity = value;
                RaisePropertyChanged();
            }
        }

        public double MaxComplexity
        {
            get => _complexity.MaxComplexity;
            set {
                _complexity.MaxComplexity = value;
                RaisePropertyChanged ();
            }
        }

        public double Value
        {
            get => _categoryComplexity.Value;
            set {
                _categoryComplexity.Value = value;
                RaisePropertyChanged();
            }
        }
    }
}
