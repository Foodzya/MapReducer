using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MapReducer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> listOfStrings = new List<string> { "Alex", "Peter", "John", "George", "Dean", "Frank", "Devour", "Jesus"};

            string someText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum ";

            string[] splittedText = someText.Split(' ');


            // splittedText.Aggregate(0, (s, m) => s + m);
            List<int> listOfInts = new List<int> { 1, 2, 3, 1, 3, 2, 3, 2, 1 };

            var result = listOfInts.MapReduce("Numbers: ", number => number, number => number, (acc, val) => 
            {
                switch (val) 
                {
                    case 1:
                    return acc + "one";
                    case 2:
                    return acc + "two";
                    case 3:
                    return acc + "three";
                } 
                return acc + val;
            });
            
            Console.WriteLine(splittedText.MapReduce("Reduced string: ", word => word, word => word, (acc, val) => acc + val));

            Console.ReadLine();

            
            
        }
    }
}
