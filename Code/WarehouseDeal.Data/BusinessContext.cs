using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    public sealed class BusinessContext : IDisposable
    {
        private readonly DataContext _context;
        public DataContext DataContext => _context;

        private bool _disposed;

        public BusinessContext ()
        {
            try {
                this._context = new DataContext();
                _context.Database.Initialize (false);
            }
            catch (Exception ex) {

                Debug.WriteLine ("Инициализация не выполнена. Ошибка: ");
                Debug.WriteLine (ex.Message);
            }
        }

        #region Read Data
        //-----------------------------------------------------------------------------------------------

        public IEnumerable<Category> GetAllCategories ()
        {
            return _context.CategorySet.Select (s => s).ToArray();
        }

        public Category GetRootCategory()
        {
            return _context.CategorySet.First (p => p.CategoryParent == null);
        }

        public IEnumerable<Category> GetAllRootCategiries ()
        {
            return _context.CategorySet.Where (p => p.CategoryParent == null).ToArray ();
        }

        public IEnumerable<Category> GetChildrenCategories (Category rootCategory)
        {
            return _context.CategorySet.Where (p => p.CategoryParent.Id == rootCategory.Id).ToArray();
        }

        //-----------------------------------------------------------------------------------------------
        #endregion Read Data


        #region Create Data
        //-----------------------------------------------------------------------------------------------

        public Category AddNewCategory (string id, string name)
        {
            Check.IsCategoryIdCorrect (id);
            Check.IsCategoryNameCorrect (name);

            Category category = new Category
            {
                Id = id,
                Name = name,
                CategoryParent = null
            };

            category = _context.CategorySet.Add (category);
            _context.SaveChanges();

            return category;
        }

        public void AddNewCategory (Category category)
        {
            Check.IsCategoryIdCorrect (category.Id);
            Check.IsCategoryNameCorrect (category.Name);

            Category entity = new Category { Id = category.Id, Name = category.Name };

            _context.CategorySet.Add (entity);
            _context.SaveChanges ();
        }

        //-----------------------------------------------------------------------------------------------
        #endregion Create Data


        #region Update Data
        //-----------------------------------------------------------------------------------------------
        public void AddParentCategory (string idChild, string idParent)
        {
            Category category = _context.CategorySet.Find (idChild);

            if (category == null)
                throw new ArgumentException ();

            category.CategoryParent = _context.CategorySet.Find (idParent);

            _context.SaveChanges();
        }

        public void AddParentCategory (Category child, Category parent)
        {
            Category category = _context.CategorySet.Find (child.Id);

            if (category == null)
                throw new ArgumentException();

            category.CategoryParent = _context.CategorySet.Find (parent.Id);

            _context.SaveChanges ();
        }

        //-----------------------------------------------------------------------------------------------
        #endregion Update Data


        #region Check Class
        static class Check
        {
            public static void IsCategoryIdCorrect (string id)
            {
                if (id == null) 
                    throw new ArgumentNullException();

                if (id.Trim().Length != 7)
                    throw new ArgumentException();
            }

            public static void IsCategoryNameCorrect (string name)
            {
                if (name == null)
                    throw new ArgumentNullException ();

                if (name.Trim ().Length == 0)
                    throw new ArgumentException ();
            }
        }
        #endregion Check Class


        #region IDisposable Members
        public void Dispose ()
        {
            Dispose(true);

            GC.SuppressFinalize (this);

        }

        private void Dispose (bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _context?.Dispose();

            _disposed = true;
        }
        #endregion IDisposable Members
    }
}
