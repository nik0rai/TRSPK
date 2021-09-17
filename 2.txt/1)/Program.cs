using System;
using System.Collections.Generic;

namespace Zadanie2
{
    class Program
    {
        static void Main(string[] args)
        {
            Man person1 = new() { Name="Al", Age = 16 }; //нельзя поставить null или не объявлять
            person1.Print();

            person1 = new Teenager() { School = "Liceum #1547", Age = person1.Age }; //имя передалось, а вот возраст нет? Почему так? Пришлось его передать отдельно.
            person1.Print();
        }
    }

}
