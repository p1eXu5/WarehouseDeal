﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    public sealed class BusinessContext : IDisposable
    {
        private readonly DataContext context;
        private bool disposed;

        public BusinessContext ()
        {
            this.context = new DataContext();
        }

        public Category GetRootCategory()
        {
            try {
                return context.CategorySet.First (p => p.CategoryParent == null);
            }
            catch {

            }

            return null;
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
