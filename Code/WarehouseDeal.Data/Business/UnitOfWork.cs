using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data.Business
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataContext _context = new DataContext();

        private CategoryRepository _categoryRepository;

        public CategoryRepository CategoryRepository
        {
            get {

                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);

                return _categoryRepository;
            }
        }

        public DataContext DataContext => _context;

        #region IDisposable

        private bool _disposed;
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize (this);
        }

        private void Dispose (bool disposing)
        {
            if(_disposed || !disposing)
                return;

            _context?.Dispose();
            _disposed = true;
        }

        #endregion
    }
}
