using System;
using System.ComponentModel.DataAnnotations;

namespace Man
{
    class Man
    {
        public string Name;
        public uint Age;

        public Man(string name, uint age) //Положительный int
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(); //Исключение: поле не может быть пустым
            Name = name;
            Age = age;
        }

        public virtual string Display() => (nameof(Man) + ' ' + Name + ' ' + Age); //Пара virtual-override для использования метода в других классах с учетом изменений
    };
    class Teenager : Man // наследник класса Man 
    {
        public string School;

        public Teenager(string name, uint age, string school) : base(name, age) // обращаемся к классу Man: передаем имя и возраст на установку в конструктор главного класса
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(); //Исключение: поле не может быть пустым
            if (age < 13 || age > 19) throw new ArgumentOutOfRangeException(); //Исключение: выход за диапазон допустимых значений
            School = school;
        }

        public override string Display() => (nameof(Teenager) + ' ' + Name + ' ' + Age + " Place of study: " + School); //Пара virtual-override для использования метода в других классах с учетом изменений

    };
    class Worker : Man
    {
        public string Workplace;

        public Worker(string name, uint age, string workplace) : base(name, age) //обращаемся к классу Man: передаем имя и возраст на установку в конструктор главного класса
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();//Исключение: поле не может быть пустым
            if (age < 16 || age > 70) throw new ArgumentOutOfRangeException();//Исключение: выход за диапазон допустимых значений
            Workplace = workplace;
        }

        public override string Display() => (nameof(Worker) + ' ' + Name + ' ' + Age + " Place of work: " + Workplace); //Пара virtual-override для использования метода в других классах с учетом изменений
    };

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Человек является работником или школьником?");
            Console.WriteLine("1. Школьник.");
            Console.WriteLine("2. Работник.");
            Console.WriteLine("3. Ни одно из перечисленных.");
            string name;
            uint age;
            string workplace;
            string school;
            int op = Convert.ToInt32(Console.ReadLine());
            switch (op)
            {
                case 1:

                    Console.WriteLine("Введите имя");
                    name = Console.ReadLine();
                    Console.WriteLine("Введите возраст");
                    age = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine("Введите школу");
                    school = Console.ReadLine();
                    Teenager smn = new Teenager(name, age, school);
                    Console.WriteLine(smn.Display());
                    break;

                case 2:
                    Console.WriteLine("Введите имя");
                    name = Console.ReadLine();
                    Console.WriteLine("Введите возраст");
                    age = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine("Введите место работы");
                    workplace = Console.ReadLine();
                    Worker smnelse = new Worker(name, age, workplace);
                    Console.WriteLine(smnelse.Display());
                    break;

                case 3:
                    Console.WriteLine("Введите имя");
                    name = Console.ReadLine();
                    Console.WriteLine("Введите возраст");
                    age = Convert.ToUInt32(Console.ReadLine());
                    Man somebody = new Man(name, age);
                    Console.WriteLine(somebody.Display());
                    break;
            }
        }
    }
}
