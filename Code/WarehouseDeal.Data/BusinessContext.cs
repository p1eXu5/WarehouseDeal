using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    public class BusinessContext : IDisposable
    {
        private readonly DataContext context;
        private bool disposed;

        public BusinessContext ()
        {
            this.context = new DataContext();
        }

        public ICollection<Category> GetCategoriesList()
        {
            return context.CategorySet.OrderBy (p => p.Id).ToArray();
        }
        #region IDisposable Members
        public void Dispose ()
        {
            Dispose(true);

            GC.SuppressFinalize (this);

        }

        private void Dispose (bool disposing)
        {
            if (!disposing || disposed)
                return;

            context?.Dispose();

            disposed = true;
        }
        #endregion IDisposable Members
    }
}
