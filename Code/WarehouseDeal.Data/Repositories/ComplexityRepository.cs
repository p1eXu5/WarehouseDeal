using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data.Repositories
{
    public class ComplexityRepository : IRepository<Complexity,int>
    {
        private readonly DataContext _context;

        public ComplexityRepository (DataContext context)
        {
            this._context = context;
        }

        public void Load() => _context.ComplexitySet.Load();

        public IEnumerable<Complexity> GetAll() => _context.ComplexitySet;
        public Complexity Get (int id) => _context.ComplexitySet.Find (id);

        public void AddNew (Complexity complexity)
        {
            if (_context.ComplexitySet.Find (complexity.Id) == null) {

                if (string.IsNullOrWhiteSpace (complexity.Title)) {

                    int maxId = _context.ComplexitySet.Max (i => i.Id);
                    complexity.Title = $"Неизвестная сложность {++maxId}";
                }

                complexity.Id = default(int);
                if (complexity.CategoryComplexity != null) complexity.CategoryComplexity = null;
                if (complexity.Operations != null) complexity.Operations = null;

                _context.ComplexitySet.Add (complexity);
            }
            else {
               Update (complexity);
            }
        }

        public void Update (Complexity item)
        {
            _context.Entry (item).State = EntityState.Modified;
        }

        public void Delete (int item)
        {
            var complexity = _context.ComplexitySet.Find (item);
            if (complexity != null)
                _context.ComplexitySet.Remove (complexity);
        }
    }
}
