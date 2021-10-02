using System;

namespace task_3._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество измерений в массиве: ");
            int dim = Convert.ToInt32(Console.ReadLine());  //Кол-во измерений            

            int elements = Convert.ToInt32(Math.Pow(dim, dim));
            int[] arr = new int[elements];

            int[] min_range = new int[dim];
            int[] max_range = new int[dim];

            for (int i = 0; i < dim; i++)   //Цикл для введения ограничений для каждого измерения
            {
                Console.WriteLine("Введите значение для нижней границы измерения {0}: ", i + 1);
                min_range[i] = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Введите значение для верхней границы измерения {0}: ", i + 1);
                max_range[i] = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
            }
            int[] cord = new int[dim];

            Random rand = new Random();
            for (int i = 0; i < elements; i++)
            {
                for (int j = dim - 1; j > 0; j--)
                {
                    if ((cord[j] % dim == 0) && (cord[j] != 0))
                    {
                        cord[j] = 0;
                        cord[j - 1] += 1;
                    }

                }
                arr[i] = rand.Next(min_range[cord[0]], max_range[cord[0]]);
                cord[cord.Length - 1] += 1;
            }
            int count = 0;
            for (int i = 0; i < dim*dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        Console.Write("{0} ", arr[count]);
                        count++;
                        if (count == elements) break;
                    }
                    Console.WriteLine();
                    if (count == elements) break;
                }
                Console.WriteLine();
                if (count == elements) break;
            }
        }
    }
}