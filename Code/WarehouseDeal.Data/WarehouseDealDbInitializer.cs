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
            // Заполнение базы данных начальными значениями Сложностей и тестовыми Категорий:
            IEnumerable<Complexity> defaultComplexities = new List<Complexity>
            {
                new Complexity { Title = "Сложность поиска", Abbreviation = "Сл_пск",MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность подбора", Abbreviation = "Сл_пдб", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность упаковки", Abbreviation = "Сл_уп", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность расстановки", Abbreviation = "Сл_рст", MinComplexity = 0.0, MaxComplexity = 99.9 },
                new Complexity { Title = "Сложность подсчёта", Abbreviation = "Сл_псч", MinComplexity = 0.0, MaxComplexity = 99.9 }
            };

            context.ComplexitySet.AddRange (defaultComplexities);

            CategoryRepository.LoadCategoriesFromFile ("TestCategories.csv", context);

            base.Seed (context);
        }
    }
}
