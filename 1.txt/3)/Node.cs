using System;
using System.Collections.Generic;

namespace Zadanie1
{
    class Node
    {
        private Node parent = null;
        private string Text;
        private readonly List<Node> Children = new();

        public Node(string _Text) => Text = _Text;
        private void SetParent(Node _parent) => parent = _parent;
        public void SetText(string _Text) => Text = _Text;

        public string GetText() => Text;
        public List<Node> GetChildren() => Children;

        public void AddChild(Node child)
        {
            child.SetParent(this);
            this.Children.Add(child);
        }
        public void AddChild(string _text)
        {
            Node newChild = new Node(_text);
            this.AddChild(newChild);
        }
        public void AddChildren(List<Node> children)
        {
            foreach (Node i in children)
                i.SetParent(this);

            this.Children.AddRange(children);
        }
       
        public void Print(string pref, bool last)
        {

            Console.Write(pref);
            if (last){
                Console.Write("└─");
                pref += "  ";
            }
            else{
                Console.Write("├─");
                pref += "| ";
            }
            Console.WriteLine(Text);


            for (int i = 0; i < Children.Count; i++)
                Children[i].Print(pref, i == Children.Count - 1);
        }
    }
}
