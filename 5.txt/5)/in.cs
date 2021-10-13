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
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                object.ReferenceEquals(a, b);
            }

            stopwatch.Stop();
            string.Intern(a);
            string.Intern(b);
            var stopwatch1 = Stopwatch.StartNew();
            for (int i = 0; i < Math.Max(a.Length, b.Length); i++)
            {
                object.ReferenceEquals(a, b);
            }
            stopwatch1.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds + " - " + object.ReferenceEquals(a, b));
            Console.WriteLine(stopwatch1.Elapsed.TotalMilliseconds + " - " + object.ReferenceEquals(a, b));
        }
    }
}
