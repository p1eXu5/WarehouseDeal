using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WarehouseDeal.BaseClasses;
using WarehouseDeal.Data;

namespace WarehouseDeal.DesktopClient.ViewModels
{
    using static WarehouseDeal.ServiceClasses.WatehouseDealServiceClass;

    public class CategoriesModelView : ViewModel
    {
        private readonly BusinessContext _context;
        private Category _selectedCategory;

        public ActionCommand ImportCommand => new ActionCommand (a => ImportFileCategory());

        public CategoriesModelView() : this(new BusinessContext()) { }

        public CategoriesModelView (BusinessContext context)
        {
            _context = context;
            //Categories = new ObservableCollection<Category>();
            GetCategoriesList();
            IsTreeView = true;
      
        }


        //public ICollection<Category> Categories { get; private set; }
        public ObservableCollection<CategoriesList> Categories { get; private set; }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set {
                _selectedCategory = value;
                NotifyPropertyChanged ();
            }
        }

        private bool _isTreeView = true;
        public bool IsTreeView
        {
            get => _isTreeView;
            set {
                _isTreeView = value;
                if (_isTreeView)
                    ViewContent = "Представление: Иерархия";
                else
                    ViewContent = "Представление: Таблица";
                NotifyPropertyChanged ();
            }
        }

        private string _viewContent;
        public string ViewContent
        {
            get => _viewContent;
            set {
                _viewContent = value;
                NotifyPropertyChanged ();
            }
        }

        private void GetCategoriesList()
        {
            Categories = new ObservableCollection<CategoriesList>
            {
                new CategoriesList
                {
                    Category = new Category {Name = "Категория"},
                    Categories = new ObservableCollection<CategoriesList>
                    {
                        new CategoriesList {Category = new Category {Name = "Первая категория"}},
                        new CategoriesList
                        {
                            Category = new Category {Name = "Вторая категория"},
                            Categories = new ObservableCollection<CategoriesList>
                            {
                                new CategoriesList {Category = new Category {Name = "Вложенная категория"}}
                            }
                        }
                    }
                }
            };

            //foreach (Category category in _context.GetCategoriesList()) {

            //    Categories.Add (category);
            //}
        }


        public void ImportFileCategory ()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "csv files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {

                string fileName = ofd.FileName;

                IEnumerable<string[]> lines = GetStringsArrayEnumeratorFromCsvFile (fileName);

                foreach (var line in lines) {
                    
                    if (!IsCategoryString(line)) continue;

                    _context.AddNewCategory(line[0], line[1]);
                }
            }
        }

        public bool IsCategoryString (string[] line)
        {
            return !(line == null || line.Length != 4 || 
                     String.IsNullOrEmpty (line[0]) || line[0].Length != 7 ||
                     String.IsNullOrWhiteSpace (line[1]) ||
                     (!string.IsNullOrEmpty (line[3]) && line[3].Length != 7) ||
                     (!line[2].Equals ("Да") && !line[2].Equals ("Нет")));
        }

        public class CategoriesList
        {
            public Category Category { get; set; }
            public ObservableCollection<CategoriesList> Categories { get; set; }
        }
    }
}
