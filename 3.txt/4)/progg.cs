class Program
    {

        public static int UsageParams(params int[] nums)
        {
            int sum = 0;
            foreach (var item in nums)
                sum += item;

            return sum;
        }

        static void Main(string[] args)
        {          
            Console.WriteLine(UsageParams(1, 2, 3, 4)); //выведет 10
        }       
    }
