using System;
using System.Linq;

namespace Wakaranai
{
    class StringList
    {
        private string[] vs;
        public StringList(string[] vs) => this.vs = vs.Where(val => val != null).ToArray(); //пустые значения убираем (це конструктор)

        public void Print()
        {
            for (int i = 0; i<vs.Length; i++)
                Console.Write("{0} ", vs[i]);
            Console.WriteLine();
        }

        public int Insert(string s)
        {
            vs = vs.Concat(new string[] { s }).ToArray(); //объединяем две последовательности
            return vs.Length - 1; //вернем позицию нового элемента
        }

        public void Delete(uint pos) => vs = vs.Where((val, index) => index != pos).ToArray();

        public int Search(string s) => Array.FindIndex(vs, lambda => lambda == s); //предикаты пишется через лямбда-выражение

        public void Update(uint pos, string s) => vs[pos] = s;

        public string GetAt(uint pos) => (pos < vs.Length && vs[pos] != null) ? vs[pos] : "\x1b[31m\x1B[4mOut of range or empty!\x1B[0m"; //сокращения (RED, UNDERLINE в самом конце RESET)
    }
}
