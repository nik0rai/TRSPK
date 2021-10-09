using System;
using System.Diagnostics;


namespace ConsoleApp9
{
    class NumberArray
    {
        public int[] Arr { get; set; }

        public void ReadArray()
        {
            Console.WriteLine("Введите элементы:");
            for (int i = 0; i < Arr.Length; i++)
            {
                Console.Write("intArray[{0}] = ", i); 
                Arr[i] = Convert.ToInt32(Console.ReadLine());
            }
        }
        public void FillArrayRandom()
        {
            Random rand = new ();

            for (int i = 0; i < Arr.Length; i++)
                Arr[i] = rand.Next(100);    
        }

        public int GetUnit(uint index) => (index < Arr.Length) ? Arr[index] : throw new IndexOutOfRangeException(nameof(index));
        public object SetUnit(int[] arr, uint index) => (index < arr.Length) ? arr[index] : throw new ArgumentOutOfRangeException(nameof(index));
        public void CopyArray(out NumberArray to)
        {
            to = new();
            to.Arr = Arr;
            Array.Clear(Arr, 0, Arr.Length);
            
        }
        public void CopyArray(out int[] to)
        {
            to = Arr;
            Array.Clear(Arr, 0, Arr.Length);

        }

    }


    class Sorting
    {
        private static void Swap(ref int x, ref int y) => (x, y) = (y, x);
        public static int[] InsertionSort(NumberArray numbers)
        {
            int x, j;

            for (int i = 0; i < numbers.Arr.Length; i++)
            {
                x = numbers.Arr[i];
                j = i;
                while (j > 0 && numbers.Arr[j - 1] > x)
                {
                    Swap(ref numbers.Arr[j], ref numbers.Arr[j - 1]);
                    j -= 1;
                }
                numbers.Arr[j] = x;
            }
            return numbers.Arr;
        }


        public static int Partition(int[] numbers, int left, int right)
        {
            var p = left - 1;
            for (var i = left; i < right; i++)
            {
                if (numbers[i] < numbers[right])
                {
                    p++;
                    Swap(ref numbers[p], ref numbers[i]);
                }
            }
            p++;
            Swap(ref numbers[p], ref numbers[right]);
            return p;
        }
        public static int[] QuickSort(int[] numbers, int left, int right)
        {
            if (left >= right)
                return numbers;
            
            var pIndex = Partition(numbers, left, right);
            QuickSort(numbers, left, pIndex - 1);
            QuickSort(numbers, pIndex + 1, right);
            return numbers;         
        }
        public static int[] QuickSort(NumberArray numbers) => QuickSort(numbers.Arr, 0, numbers.Arr.Length - 1);
        

        public delegate int[] SortDelegate(NumberArray Array);
        internal class Program
        {
            static void Main()
            {
                Console.Write("Enter size of array: ");
                int len = Convert.ToInt32(Console.ReadLine());

                NumberArray num = new ();
                num.Arr = new int[len];
                num.FillArrayRandom();

                foreach (var item in num.Arr)
                    Console.WriteLine(item);


                SortDelegate _quickSort = new (QuickSort);
                SortDelegate _insertionSort = new (InsertionSort);
                Stopwatch stopwatch = new ();

                Console.WriteLine("Select sorting alorithm:\n 1. Quick sort\n 2. Insertion sort");
                char selection = Console.ReadKey().KeyChar;
                switch (selection)
                {
                    case '1':
                        stopwatch.Start();
                        _quickSort(num);
                        stopwatch.Stop();
                        break;
                    case '2':
                        stopwatch.Start();
                        _insertionSort(num);
                        stopwatch.Stop();
                        break;
                    default:
                        Console.WriteLine("Command error, not existing command!");
                        break;
                }

                Console.WriteLine("Sorted array: ");
                foreach (var item in num.Arr)
                    Console.WriteLine(item);                
       
                Console.WriteLine("Elapsed time: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
