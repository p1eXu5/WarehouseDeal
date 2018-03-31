﻿using System;
using System.Collections.Generic;
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
        }
    }
}
