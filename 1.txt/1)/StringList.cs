using System;

namespace Zadanie1
{
    class StringList
    {
        private string[] vs;
        public StringList(string[] _vs) => vs = _vs; 
        public int Insert(string s) {
            //я не понял, тут не указано, что строку надо добавлять на какую-то позицию, значит его надо добавлять в конец?
            Array.Resize(ref vs, vs.Length + 1);
            vs[vs.Length-1] = s;

            return vs.Length-1; //зач это надо 
        }
        public void Delete(int pos) {
            if (pos > -1 && pos < vs.Length)
            {                
                //System.Collections.Generic.List<string> temp = new System.Collections.Generic.List<string>(vs); //List Method
                //temp.RemoveAt(pos);
                //vs = temp.ToArray();
          
                for (int i = pos; i < vs.Length - 1; i++) //Shift Method
                    vs[i] = vs[i + 1];
                Array.Resize(ref vs, vs.Length - 1); //убираем последний т.к. мы уже переместили всё влево на 1
            }
        }
        public int Search(string s) {

            //Array.Find(vs, lambda => lambda == s); //еее находим элемент

            int pos = -1; // -1 для обозначения, что не найдено ничего
            for (int i = 0; i < vs.Length; i++)
                if (vs[i].Equals(s))
                {
                    pos = i;
                    break;
                }
         
            return pos;
        }
        //public object Update(int pos, string s) => (pos > -1 && pos < vs.Length) ? vs[pos] = s : null; //я хотел красиво, но надо использовать void
        public void Update(int pos, string s) {
            if (pos > -1 && pos < vs.Length)
                vs[pos] = s;
        }
        public string GetAt(int pos) => (pos < vs.Length && vs[pos] != null) ? vs[pos] : "\x1b[31m\x1B[4mOut of range or empty!\x1B[0m"; //сокращения (RED, UNDERLINE в самом конце RESET)
        public void Print()
        {
            for (int i = 0; i < vs.Length; i++)
                Console.Write("{0} ", vs[i]);
            Console.WriteLine(" |Size: {0}", vs.Length);
        }
    }
}
