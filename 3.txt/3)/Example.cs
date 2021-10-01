 class Program
    {     
        static void Main(string[] args)
        {
            var mapper = new ArrEl<int, string>();
            mapper.Add(0, "Hello");
            mapper.Add(new Item<int, string>(1, "world"));

            foreach (var item in mapper.Keys)
                Console.WriteLine(mapper.Get(item));
        }       
    }
