    class Program
    {  
        public static void Swap(ref int x, ref int y) =>  (x, y) = (y, x);         
        
        static void Main()
        {    
            int x = 1;
            int y = 0;
            Console.WriteLine($"{ x}, { y}"); // 1 0
            Swap(ref x, ref y);
            Console.WriteLine($"{ x}, { y}"); // 0 1

        }
    }
}
