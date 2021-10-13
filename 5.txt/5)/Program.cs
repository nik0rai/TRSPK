
using System;
using System.Diagnostics;
class HelloWorld
{
    static void Main()
    {

        Stopwatch stopwatch = new();
        bool check = true;
        int a = 8;
        int i;

        stopwatch.Start();
        for (i = 0; i < 10; i++)
        {
            if (a == 8)
            {
                check = true;
                i++;
            }
        }
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0}ms", stopwatch.Elapsed.TotalMilliseconds);

        stopwatch.Start();
            if (a == 8) check = true;
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0}ms", stopwatch.Elapsed.TotalMilliseconds);


    }
}