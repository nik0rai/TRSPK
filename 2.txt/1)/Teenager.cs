using System;
using System.Diagnostics.CodeAnalysis;

namespace Zadanie2
{
    class Teenager : Man //наследуем класс Man
    {
        public string School { get; set; }

        public override void Print() => Console.WriteLine("{0}: {1}, {2}, Place of study: {3} ", nameof(Teenager), Name, Age, School);
    }
}
