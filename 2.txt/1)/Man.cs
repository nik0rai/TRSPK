using System;
using System.Diagnostics.CodeAnalysis;

namespace Zadanie2
{
    class Man
    {
        public uint Age { get; set; }       

        private static string _name = null; //хелпер у которого сначала null (обход Stack Overflow)
        [DisallowNull]
        public string Name
        {
            get => _name ?? throw new ArgumentNullException(); //если не объявлять, то может привести к Stack Overflow
            set => _name = value ?? throw new ArgumentNullException();
        }
        
        public virtual void Print() => Console.WriteLine("{0}: {1}, {2} ", nameof(Man), Name, Age); //Человек: Имя, Возраст ?то ли я сделал?
    }
}
