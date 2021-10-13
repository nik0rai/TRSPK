using System;
using System.Diagnostics;
using System.Text;
class HelloWorld
{
    static void Main()
    {

        Console.WriteLine("text: hello world");

        Stopwatch stopwatch = new();

        string s1 = "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohello";
        string s2 = "worldworldworldworldworldworldworldworldworldworldworldworldworldworldworldworldworldworldworld";

        StringBuilder sb = new();

        Console.WriteLine("String: ");
        stopwatch.Start();
        _ = s1 + ", " + s2;
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0}ms", stopwatch.Elapsed.TotalMilliseconds);

        Console.WriteLine("StringBuilder: ");
        stopwatch.Start();
        sb.AppendLine(s1 + ", " + s2);
        stopwatch.Stop();
        Console.WriteLine("Elapsed time: {0}ms", stopwatch.Elapsed.TotalMilliseconds);


    }
}