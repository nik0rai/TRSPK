using System;
using System.Collections.Generic;

namespace Zadanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            Node root = new("root");
            Node child1 = new("child1");
            child1.AddChild("grandchild1");
            child1.AddChild("grandchild2");

            Node child2 = new("child2");
            child2.AddChild("grandchild3");

            root.AddChild(child1);
            root.AddChild(child2);

            root.GetChildren()[0].GetChildren()[1].AddChild("grandgrandchild1(connected to 2nd)");


            List<Node> ch = new();
            ch.Add(new Node("Child4"));
            ch.Add(new Node("Child5"));
            ch.Add(new Node("Child6"));
            root.AddChildren(ch);

            //strings: ├ │ ─
            root.Print("", true);
        }
    }

}
