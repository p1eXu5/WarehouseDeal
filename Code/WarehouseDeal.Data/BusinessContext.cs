using System;
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
            return context.CategorySet.First (p => p.CategoryParent == null);
        }

        public IEnumerable<Category> GetChildCategiries (Category rootCategory)
        {
            return context.CategorySet.Where (p => p.CategoryParent.Id == rootCategory.Id).ToArray();
        }

        public Category AddNewCategory (string id, string name)
        {
            Category category = new Category
            {
                Id = id,
                Name = name,
                CategoryParent = null
            };

            category = context.CategorySet.Add (category);
            context.SaveChanges();

            return category;
        }

        public void AddNewCategory (Category category)
        {
            context.CategorySet.Add (category);
            context.SaveChanges ();
        }

        public void AddParentCategory (string idChild, string idParent)
        {
            Category category = context.CategorySet.Find (idChild);
            if (category != null)
                category.CategoryParent = context.CategorySet.Find (idParent);
            context.SaveChanges();
        }

        public void AddParentCategory (Category child, Category parent)
        {
            Category category = context.CategorySet.Find (child.Id);
            if (category != null)
                category.CategoryParent = context.CategorySet.Find (parent.Id);
            context.SaveChanges ();
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
