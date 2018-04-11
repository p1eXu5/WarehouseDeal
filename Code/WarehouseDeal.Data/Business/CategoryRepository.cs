namespace WarehouseDeal.Data.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using static WarehouseDeal.ServiceClasses.WatehouseDealServiceClass;
    using static FileOfCategoriesColumns;



    /// <summary>
    /// Контекст категорий
    /// </summary>
    public class CategoryRepository : IRepository<Category,string>
    {
        private readonly DataContext _context;

        public CategoryRepository (DataContext context)
        {
            _context = context;
        }

        #region Read Data
        //-----------------------------------------------------------------------------------------------
        public IEnumerable<Category> GetAll () => _context.CategorySet;
        public Category Get (string id) => _context.CategorySet.Find (id);
        public Category GetRootCategory() => _context.CategorySet.FirstOrDefault (p => p.CategoryParent == null);
        public IEnumerable<Category> GetAllRootCategiries () => _context.CategorySet.Where (p => p.CategoryParent == null);
        public IEnumerable<Category> GetChildrenCategories (Category rootCategory) => _context.CategorySet.Where (p => p.CategoryParent.Id == rootCategory.Id);
        //-----------------------------------------------------------------------------------------------
        #endregion Read Data


        #region Create Data
        //-----------------------------------------------------------------------------------------------
        public Category AddNew (string id, string name)
        {
            Check.IsCategoryIdCorrect (id);
            Check.IsCategoryNameCorrect (name);

            Category category = new Category
            {
                Id = id,
                Name = name,
                CategoryParent = null
            };

            category = _context.CategorySet.Add (category);
            _context.SaveChanges();

            return category;
        }
        public void AddNew (Category category)
        {
            Check.IsCategoryIdCorrect (category.Id);
            Check.IsCategoryNameCorrect (category.Name);
            category.CategoryParent = null;
            category.Dept = null;
            category.Product = null;

            _context.CategorySet.Add (category);
            _context.SaveChanges ();
        }

        /// <summary>
        /// Загрузка в БД записей из csv-файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public int LoadCategoriesFromFile (string fileName)
        {
            ICollection<string[]> lines = GetStringsArrayEnumeratorFromCsvFile (fileName).ToList ();
            ICollection<string[]> removableLines = new List<string[]> ();

            foreach (var line in lines) {
                if (!Check.IsCategoryString (line)) {
                    removableLines.Add (line);
                    continue;
                }

                AddNew (line[(int)Id], line[(int)Name]);
            }

            foreach (string[] line in removableLines) {
                lines.Remove (line);
            }

            foreach (var line in lines) {
                if (!String.IsNullOrWhiteSpace (line[(int)Parent]))
                    AddParentCategory (line[(int)Id], line[(int)Parent]);
            }

            return lines.Count;
        }
        //-----------------------------------------------------------------------------------------------
        #endregion Create Data


        #region Update Data
        //-----------------------------------------------------------------------------------------------
        /// <summary>
        /// Заполнение родителей
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idParent"></param>
        public void AddParentCategory (string idChild, string idParent)
        {
            Category category = _context.CategorySet.Find (idChild);

            if (category == null)
                throw new ArgumentException ();

            category.CategoryParent = _context.CategorySet.Find (idParent);

            _context.SaveChanges();
        }
        public void AddParentCategory (Category child, Category parent)
        {
            Category category = _context.CategorySet.Find (child.Id);

            if (category == null)
                throw new ArgumentException();

            category.CategoryParent = _context.CategorySet.Find (parent.Id);

            _context.SaveChanges ();
        }
        public void Update (Category category)
        {
            _context.Entry (category).State = EntityState.Modified;
        }
        //-----------------------------------------------------------------------------------------------
        #endregion Update Data


        #region Delete
        public void Delete (string id)
        {
            Category category = _context.CategorySet.Find (id);
            if (category != null)
                _context.CategorySet.Remove (category);
        }
        public void DeleteAllCategories()
        {
            _context.CategorySet.RemoveRange(GetAll());
            _context.SaveChanges();
        }
        #endregion


        #region Check Class
        public static class Check
        {
            public static void IsCategoryIdCorrect (string id)
            {
                if (id == null) 
                    throw new ArgumentNullException();

                if (id.Trim().Length != 7)
                    throw new ArgumentException();
            }
            public static void IsCategoryNameCorrect (string name)
            {
                if (name == null)
                    throw new ArgumentNullException ();

                if (name.Trim ().Length == 0)
                    throw new ArgumentException ();
            }
            public static bool IsCategoryString (string[] line)
            {

                return !(line == null || line.Length != 4 ||
                         String.IsNullOrEmpty (line[0]) || line[0].Length != 7 ||
                         String.IsNullOrWhiteSpace (line[1]) ||
                         (!string.IsNullOrEmpty (line[3]) && line[3].Length != 7) ||
                         (!line[2].Equals ("Да") && !line[2].Equals ("Нет")));
            }
        }
        #endregion Check Class

    }
}
