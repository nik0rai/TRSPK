using System;

namespace TrainCsharp
{
    [Flags]
    public enum Group
    {
        Arina = 1 << 0,
        Marina = 1 << 1,
        Nikolay = 1 << 2,
        MarkWolf = 1 << 3,
        Anna = 1 << 4,
        ArinaMarina = 5
    }
    class Program
    {
        static void Main(string[] args)
        {
            var enums = (Group.Arina | Group.Marina | Group.Nikolay |
                         Group.MarkWolf | Group.Anna).ToString();
            Console.WriteLine(enums);

            Console.WriteLine(Enum.Parse(typeof(Group), "31").ToString());      //31 = 2^5 - 2^0

            Console.WriteLine(Enum.Parse(typeof(Group), "1"));
            Console.WriteLine(Enum.Parse(typeof(Group), "Arina"));
            Console.WriteLine(Enum.Parse(typeof(Group), "Arina,Nikolay"));
            Console.WriteLine(Enum.Parse(typeof(Group), "5"));

        }
    }
}
