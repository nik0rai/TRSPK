using System;
using System.Globalization;

namespace zadanie5
{
    class Program
    {
        static void Main()
        {
            //вывести на экран все возможные варианты форматирования строк и чисел
            //форматирование валюты C
            double number = -12345.6789;
            int number2 = 123;
            double value = 1234567890;
            decimal number5 = 0.15345m;
            //форматирование целых чисел F
            string result2 = String.Format("{0:d}", number2);
            Console.WriteLine(result2);
            //форматирование дробных чисел F
            string result3 = String.Format("{0:f}", number2);
            Console.WriteLine(result3);
            string result4 = String.Format("{0:f2}", number);
            Console.WriteLine(result4);
            //Формат процентов Р
            Console.WriteLine(number5.ToString("P"));
            //экспоненциальный формат E
            Console.WriteLine(number.ToString("E", CultureInfo.InvariantCulture));
            //формат N
            Console.WriteLine(number.ToString("N", CultureInfo.InvariantCulture));
            //шестнадцатиричный формат х
            number2 = 0x2045e;
            Console.WriteLine(number2.ToString("x"));
            //числа и строки настраиваемого формата
            Console.WriteLine(value.ToString("(###) ###-####"));
            Console.WriteLine(value.ToString("#,#", CultureInfo.InvariantCulture));
            Console.WriteLine(String.Format(@"{0:##0  and \0\0}", number2));
            //считать от пользователя дату и число в трех разных форматах??
            Console.WriteLine("Enter a date: ");
            DateTime UserDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine(UserDate.ToString("g", CultureInfo.CreateSpecificCulture("en-US")));
            Console.WriteLine(UserDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US")));
            Console.WriteLine(UserDate.ToString("d"));
        }
    }
}
