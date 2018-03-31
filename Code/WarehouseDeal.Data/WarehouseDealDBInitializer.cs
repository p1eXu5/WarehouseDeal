using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    class WarehouseDealDBInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed (DataContext context)
        {
            base.Seed (context);
        }
    }
}
