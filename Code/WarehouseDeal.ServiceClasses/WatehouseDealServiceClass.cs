using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.ServiceClasses
{
    public static class WatehouseDealServiceClass
    {
        const string ConstCsvFileExtension = ".csv";

        /// <summary>
        /// Возвращает последовательность строк из csv-файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>IEnumerable&lt;string[]&gt;</returns>
        public static IEnumerable<string[]> GetStringsArrayEnumeratorFromCsvFile (string fileName)
        {
            if (!File.Exists (fileName) || !fileName.EndsWith (ConstCsvFileExtension))
                throw new ArgumentException ($"File does't exist or file have wrong extension: {fileName}", nameof(fileName));

            using (StreamReader sr = new StreamReader (fileName, Encoding.GetEncoding ("Windows-1251"))) {

                while (!sr.EndOfStream) {
                    yield return sr.ReadLine ().Split (';');
                }
            }
        }
    }
}
