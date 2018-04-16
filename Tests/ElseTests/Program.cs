using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElseTests
{

    class Program
    {
        static void Main (string[] args)
        {
            string[] strings1 = "param11;param12;param13".Split(';');
            string[] strings2 = "param11;param12;param13".Split(';');

            if (String.Join(";", strings1).Equals(String.Join (";", strings2)))
                Console.WriteLine("Equal");
            else
                Console.WriteLine ("Not Equal");

            string kod = "AM18162";
            string rus = "АМ18162";
            string eng = "AM18162";

            Console.WriteLine (kod.Equals (rus));
            Console.WriteLine (kod.Equals (eng));

            List<int> ints = new List<int>();

            foreach (int i in ints) {

                Console.WriteLine (i);
            }

            Console.WriteLine ("End");

            var a = new Collection<string>();
            IEnumerable<string> ie = a;
            var b = new ObservableCollection<string>();
            ie = b;

        }
    }
}
