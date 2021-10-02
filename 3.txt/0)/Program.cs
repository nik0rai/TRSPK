using System;
using System.Linq;
using System.IO;

namespace zadanie_3
{
    class Program
    {
        private const string Path1 = @"C:\Users\User\Documents\INPUT.txt";
        private const string Path2 = @"C:\Users\User\Documents\OUTPUT.txt";
        static void Main()
        {
            uint N = 0, k = 0;//кол-во выпиленных, переменная для записи смежных клеток
            int[,] chess = new int[8, 8]; //двумерный массив для шахматной доски
            try//блок выполняется, пока нет исключения или успешно не завершится
            {
                using StreamReader sr = new StreamReader(Path1);//передается путь файла, из которого идет запись
                string line = sr.ReadLine();//считываем первую строчку файла INPUT
                N = Convert.ToUInt32(line);
                line = sr.ReadLine();//переходим ко второй, чтобы сразу записать ее
                while (line != null)
                {
                    int[] mas = line.Split().Select(int.Parse).ToArray();//Split для разделения, Parse преобразует str в int, Select переводит каждый элемент в новую форму
                    chess[mas[0], mas[1]] = 1; //клетки фигуры отмечаем единицей
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);//вызывает сообщение о найденном исключении
            }
            for (int i = 0; i < 7; i++) //проходим по горизонтали
            {
                for (int j = 0; j < 6; j++)
                {
                    if ((chess[i, j] == 1) & (chess[i, j + 1] == 1))//ищем есть ли совпадающие координаты, чтобы узнать, если ли смежные стороны
                    {
                        k++;
                    }

                }
            }
            for (int i = 0; i < 7; i++) //проходим по вертикали
            {
                for (int j = 0; j < 6; j++)
                {
                    if ((chess[i, j] == 1) & (chess[i + 1, j] == 1))
                    {
                        k++;
                    }

                }
            }
            try
            {
                StreamWriter sw = new StreamWriter(Path2, false);//передается путь к файлу для записи, false - перезапись файла при добавлении новых данных
                sw.Write(4 * N - 2 * k); //периметр каждой клетки = 4, если 2 стороны смежны, вычитается 2
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
