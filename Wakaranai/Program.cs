using System;

namespace Wakaranai
{
    class Program
    {
        static void Main()
        {
            string[] arr = new string[100];
            arr[0] = "hello";
            arr[1] = "world";
            arr[2] = "!";
            arr[9] = "mda";

            StringList sl = new(arr);
            Console.WriteLine("Array is created like this:"); sl.Print();
            Console.WriteLine("\nNew elmenet \"({0})\" is added on {1} pos.", "no", sl.Insert("no")); sl.Print();
            Console.WriteLine("\nElement \"({0})\" is deleted.", "world"); sl.Delete((uint)sl.Search("world")); sl.Print();
            Console.WriteLine("\nUpdating element on pos {0} with word: \"(sus)\".", 1); sl.Update(1, "sus"); sl.Print();
            Console.WriteLine("\nFinding elemnt on pos {0}. This word is: {1}", 4, sl.GetAt(4));
        }
    }

}
