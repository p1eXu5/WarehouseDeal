using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data.Business
{
    public class ComplexityRepository : IRepository<Complexity, int>
    {
        private readonly DataContext _context;

        public ComplexityRepository (DataContext context)
        {
            if (context != null)
                _context = context;
        }

        public void AddNew (Complexity item)
        {
            if (item != null)
                _context.ComplexitySet.Add (item);

        }

        public void Delete (int item)
        {
            throw new NotImplementedException ();
        }

        public Complexity Get (int id)
        {
            throw new NotImplementedException ();
        }

        public IEnumerable<Complexity> GetAll ()
        {
            throw new NotImplementedException ();
        }

        public void Update (Complexity item)
        {
            throw new NotImplementedException ();
        }
    }
}
