using System;

namespace Zadanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = new string[100];
            arr[0] = "Hello";
            arr[1] = "World";
            arr[2] = "Nice";
            arr[99] = "Yo";
            StringList stringList = new(arr);

            stringList.Print();

            stringList.Delete(1);
            stringList.Print();

            stringList.Update(4, "hi");           
            stringList.Print();

            Console.WriteLine("GetAt(5): {0}", stringList.GetAt(5));

            Console.WriteLine("Insert() at pos: {0}", stringList.Insert("Dazo"));

            stringList.Print();

            Console.WriteLine("Found at pos: {0}", stringList.Search("Hello"));
        }
    }
}
