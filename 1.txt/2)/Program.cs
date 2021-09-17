using System;

namespace ConsoleApp1
{
    class Constant
    {
        public const double pi = 3.14; // 15-16 цифр после запятой
        public const decimal pidva = 4.8976468m; // 28-29 цифр после запятой
        public const float pitri = 3.14666f; // 6-9 цифр после запятой
        public const int width = 100;
        public const Int16 number = -100;
        public const UInt16 weight = 100;
        //public const UInt16 weight = -100;
        public const Int64 bignumber = 9223372036854775800;
        //static const int smth = 55; ошибка: константа не может быть определена с модификатором static
        public const bool roof = true;
        public const string name = "Swimming pool";
        public const byte one = 255;
        public const char symbol = 'A';
        // public const int = a ошибка, константа должна быть проинициализирована и определена
    }

    class Fields
    {
        public readonly int F1 = 55;
        public static bool F2 = true;
        public volatile char F3;
        public Fields(int _f1, char _t)
        {
            F1 = _f1; // поле для чтения может быть изменено в конструкторе 
            F3 = _t;
        }
      
    }
    class Program
    {
        static void Main(string[] args)
        {
            Fields fields = new Fields(125, 'C');
            Console.WriteLine(fields.F1);
            Console.WriteLine(Fields.F2);
            Console.WriteLine(fields.F3);
            Console.WriteLine(Constant.pi);
            Console.WriteLine(Constant.pidva);
            Console.WriteLine(Constant.pitri);
        }
    }
}
