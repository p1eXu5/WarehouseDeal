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

        public static IEnumerable<string[]> GetStringsArrayEnumeratorFromCsvFile (string fileName)
        {
            if (!File.Exists (fileName) || !fileName.EndsWith (ConstCsvFileExtension))
                throw new ArgumentException ($"File does't exist or file have wrong extension: {fileName}", nameof(fileName));

            using (StreamReader sr = new StreamReader (fileName)) {

                while (!sr.EndOfStream) {
                    yield return sr.ReadLine ().Split (';');
                }
            }
        }
    }
}
