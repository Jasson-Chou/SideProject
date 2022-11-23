using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_Generic
{
    class Program
    {
        //SortedDictionary and SortedSet Test
        static void Main(string[] args)
        {
            SortedDictionary<int, string> dic1 = new SortedDictionary<int, string>();
            dic1.Add(0, "Zero");
            dic1.Add(2, "Zero");
            dic1.Add(1, "Zero");
            dic1.Add(3, "Zero");
            //排序Dictionary
            //0, Zero | 1, Zero | 2, Zero | 3, Zero |
            foreach (var item in dic1)
                Console.Write($"{item.Key}, {item.Value} | ");
            //throw ArgumentException
            //dic1.Add(0, "Zero");

            Console.WriteLine();

            //排序且不重複
            //1 | 3 | 4 | 5 | 6 | 8 |
            SortedSet<int> list1 = new SortedSet<int>() { 1, 3, 5, 4, 8, 6, 4 };
            foreach (var item in list1)
                Console.Write($"{item} | ");


            Console.WriteLine();

            //Z | G | F | E1 | C1 | B1 | A1 |
            SortedSet<string> list2 = new SortedSet<string>(new Compare()) { "A1", "B1", "C1", "E1", "Z", "F", "G" };
            foreach (var item in list2)
                Console.Write($"{item} | ");

            Console.ReadKey();

            myClassGeneric<TestClass> f = new myClassGeneric<TestClass>();
        }
    }

    class Compare : IComparer<string>
    {
        int IComparer<string>.Compare(string x, string y)
        {
            //return x[0] - y[0]; //A-Z
            return y[0] - x[0]; //Z - A
        }
    }

    class myClassGeneric<T> where T: class//, new()
    {
        public myClassGeneric()
        {

        }
    }

    class myStructGeneric<T> where T: struct
    {
        public myStructGeneric()
        {

        }
    }

    class TestClass
    {
        //public TestClass() { }

        private TestClass() { }
    }
}
