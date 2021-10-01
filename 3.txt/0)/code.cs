
    class Program
    {
        static int Permiter(int[,] arr, int howMany)
        {
            int p = howMany * 4; // 4 значит p клетки 1 на 1
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (arr[i, j] == 1 && arr[i + 1, j] == 1) p -= 2;
                    if (arr[i, j] == 1 && arr[i, j + 1] == 1) p -= 2;
                }
            return p;
        }

        static void Main(string[] args)
        {
            int[,] arr = new int[8, 8];
            int size = Convert.ToInt32(Console.ReadLine());

            for (int flag = 0; flag < size; flag++)
            {
                uint i = Convert.ToUInt32(Console.ReadLine());
                uint j = Convert.ToUInt32(Console.ReadLine());
                arr[i, j] = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine(value: Permiter(arr, size));
        }       
    }

