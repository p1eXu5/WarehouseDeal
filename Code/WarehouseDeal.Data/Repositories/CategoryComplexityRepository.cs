using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data.Repositories
{
    public class CategoryComplexityRepository : IRepository<CategoryComplexity, Tuple<Category,Complexity>>
    {
        private readonly DataContext _context;

        public CategoryComplexityRepository (DataContext context)
        {
            if (context != null)
                _context = context;
        }

        public IEnumerable<CategoryComplexity> GetAll() => _context.CategoryComplexitySet;

        public CategoryComplexity Get (Tuple<Category, Complexity> id) => _context.CategoryComplexitySet.Find (new object[] {id.Item1.Id, id.Item2});

        public void AddNew (CategoryComplexity item)
        {
            if (item != null)
                _context.CategoryComplexitySet.Add (item);

        }

        public void Update (CategoryComplexity item)
        {
            _context.Entry (item).State = EntityState.Modified;
        }

        public void Delete (Tuple<Category, Complexity> item)
        {
            CategoryComplexity categoryComplexity = _context.CategoryComplexitySet.Find (new object[] { item.Item1.Id, item.Item2 });

            if (categoryComplexity != null)
                _context.CategoryComplexitySet.Remove (categoryComplexity);
        }


    }
}
