using System;

namespace zadanie5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите символ: ");
            char ch = Console.ReadKey().KeyChar;
            if (Char.IsLetter(ch))
            { Console.WriteLine("Введена буква."); }
            else if (Char.IsDigit(ch))
            { Console.WriteLine("Введена цифра."); }
            else Console.WriteLine("Введен символ.");
        }
    }
}
