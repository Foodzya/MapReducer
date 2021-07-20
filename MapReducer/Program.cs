using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReducer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> testList = new List<int> { 5, 6, 5, 6, 1230, 50, 100, 50, -5, 50, 99 };
            List<string> listOfNames = new List<string> { "Alex", "Peter", "Andrew", "Axel", "Boris", "Ashton", "John", "David", "Bob", "Jack", "Owshen", "Hobs" };

            int[] vs = new int[10];

            listOfNames.MapReduce(s => s + " ", s => s.First(), (s, m) => s + m);
            testList.MapReduce(s => s, s => s, (s, m) => s + m);

            Console.ReadKey();
        }
    }
}
