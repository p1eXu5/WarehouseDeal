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


        public CategoryComplexityViewModel (CategoryComplexity complexity)
        {
                _categoryComplexity = complexity;
        }

        public string Title
        {
            get => _categoryComplexity.Complexity.Title;
            set {
                _categoryComplexity.Complexity.Title = value;
                RaisePropertyChanged();
            }
        }

        public double MinComplexity
        {
            get => _categoryComplexity.Complexity.MinComplexity;
            set {
                _categoryComplexity.Complexity.MinComplexity = value;
                RaisePropertyChanged();
            }
        }

        public double MaxComplexity
        {
            get => _categoryComplexity.Complexity.MaxComplexity;
            set {
                _categoryComplexity.Complexity.MaxComplexity = value;
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
