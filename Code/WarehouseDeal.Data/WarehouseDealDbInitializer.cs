using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    using Repositories;

    class WarehouseDealDbInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed (DataContext context)
        {
            //TODO:
            IEnumerable<Complexity> defaultComplexities = new List<Complexity>
            {
                new Complexity { Title = "Сложность поиска", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность подбора", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность упаковки", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность расстановки", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность подсчёта", MinComplexity = 0.0, MaxComplexity = 99.9 }
            };

            context.ComplexitySet.AddRange (defaultComplexities);

            CategoryRepository.LoadCategoriesFromFile ("TestCategories.csv", context);

            base.Seed (context);
        }
    }
}
