using System;

namespace enumchik
{
    class Program
    {
        enum Days
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите число");
            int i = Convert.ToInt32(Console.ReadLine());
            i--;
            Console.WriteLine(Enum.GetName(typeof(Days), i));
        }
    }
}
