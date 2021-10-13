using System;
using System.Diagnostics;

namespace Intern
{
    static class Programm
    {
        static void Main(string[] args)
        {
            var a = "aasd fghjk";
            var b = "qawe rty";

            bool equals = false;
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i <Math.Max(a.Length,b.Length) ; i++)
            {
                if (a == b) equals=true;
            }
            stopwatch.Stop();
            string.Intern("aasdf ghjk");
            string.Intern("qawe rty");
            equals = false;
            var stopwatch1 = Stopwatch.StartNew();
            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                if (a == b) equals = true;
            }
            stopwatch1.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds + " - " + equals);
            Console.WriteLine(stopwatch1.Elapsed.TotalMilliseconds + " - " + equals);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds/stopwatch1.Elapsed.TotalMilliseconds);
        }
    }
}
