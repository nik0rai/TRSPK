using System;
using System.Diagnostics;


namespace TRSPK
{
    class NumberArray
    {
        public int[] Array
        {
            get;
            set;
        }
        public int n
        {
            get;
            set;
        }
        public void intArray(int N)
        {
            n = N;
            Array = new int[n];
        }
        public int this[int index]
        {
            get { return Array[index]; }
            set { }
        }
        public void ReadArray()
        {
            Console.WriteLine("Введите элементы:");
            for (int i = 0; i < Array.Length; i++)
            {
                Console.Write("intArray[{0}] = ", i); Array[i] = Convert.ToInt32(Console.ReadLine());
            }
        }
        public void ReadArrayRandom()
        {
            Random rand = new Random();
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = rand.Next(100);
            }
        }
        public void GetUnit(int[] arr)
        {
            Console.WriteLine("Введите номер элемента: ");
            int Index = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Значение данного элемента: " + arr[Index]);
        }
        public void SetUnit(int[] arr)
        {
            Console.WriteLine("Введите номер элемента: ");
            int Index = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Введите новое значение для элемента");
            arr[Index] = Convert.ToInt32(Console.ReadLine());
        }
        public void CopyArray(int[] arr, NumberArray Array)
        {
            Array.Array = arr;
        }
    }


    class Sorting
    {
        static void Swap(ref int i, ref int j)
        {
            var temp = i;
            i = j;
            j = temp;
        }


        static int[] InsertionSort(NumberArray numbers) //вставки
        {
            int x;
            int j;
            for (int i = 1; i < numbers.n; i++)
            {
                x = numbers[i];
                j = i;
                while (j > 0 && numbers[j - 1] > x)
                {
                    Swap(ref numbers.Array[j], ref numbers.Array[j - 1]);
                    j -= 1;
                }
                numbers[j] = x;
            }
            return numbers.Array;
        }


        static int Partition(int[] numbers, int minIndex, int maxIndex)
        {
            var p = minIndex - 1;
            for (var i = minIndex; i < maxIndex; i++)
            {
                if (numbers[i] < numbers[maxIndex])
                {
                    p++;
                    Swap(ref numbers[p], ref numbers[i]);
                }
            }
            p++;
            Swap(ref numbers[p], ref numbers[maxIndex]);
            return p;
        }


        //быстрая сортировка
        static int[] QuickSort(int[] numbers, int minIndex, int maxIndex)
        {
            {
                if (minIndex >= maxIndex)
                    return numbers;

                var pIndex = Partition(numbers, minIndex, maxIndex);
                QuickSort(numbers, minIndex, pIndex - 1);
                QuickSort(numbers, pIndex + 1, maxIndex);
                return numbers;
            }
        }


        static int[] QuickSort(NumberArray numbers)
        {
            return QuickSort(numbers.Array, 0, numbers.n - 1);
        }


        public delegate int[] SortDelegate(NumberArray Array);
        class Program
        {
            static void Main()
            {
                Console.Write("N = ");
                var len = Convert.ToInt32(Console.ReadLine());
                NumberArray num = new NumberArray();
                num.intArray(len);
                num.ReadArrayRandom();
                for (int i = 0; i < num.n; i++)
                {
                    Console.WriteLine(num[i]);
                }
                SortDelegate funct1 = new SortDelegate(QuickSort);
                SortDelegate funct2 = new SortDelegate(InsertionSort);
                Stopwatch stopwatch = new Stopwatch();
                Console.WriteLine("Выберите вариант сортировки:\n 1. Быстрая сортировка\n 2. Сортировка вставками");
                string selection = Console.ReadLine();
                switch (selection)
                {
                    case "1":
                        stopwatch.Start();
                        funct1(num);
                        stopwatch.Stop();
                        break;
                    case "2":
                        stopwatch.Start();
                        funct2(num);
                        stopwatch.Stop();
                        break;
                    default:
                        Console.WriteLine("Такой команды не существует");
                        break;
                }

                Console.WriteLine("Упорядоченный массив: ");
                for (int i = 0; i < num.n; i++)
                {
                    Console.WriteLine(num[i]);
                }
                Console.WriteLine("Time: " + stopwatch.ElapsedTicks);
            }
        }
    }
}

