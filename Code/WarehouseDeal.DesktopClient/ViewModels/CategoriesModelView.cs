using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDeal.BaseClasses;
using WarehouseDeal.Data;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    class CategoriesModelView : ViewModel
    {
        private readonly BusinessContext _context;
        private Category _selectedCategory;


        public CategoriesModelView() : this(new BusinessContext()) { }

        public CategoriesModelView (BusinessContext context)
        {
            _context = context;
            Categories = new ObservableCollection<Category>();
        }


        public ICollection<Category> Categories { get; private set; }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                NotifyPropertyChanged ();
            }
        }


        private void GetCategoriesList()
        {
            Categories.Clear();

            foreach (Category category in _context.GetCategoriesList()) {
                
                Categories.Add (category);
            }
        }

    }
}
