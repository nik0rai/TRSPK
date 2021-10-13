using System;
using System.Globalization;
using System.Text;
using System.Threading;

public class CultureExample
{
   public static void Main()
   {
      Console.OutputEncoding = Encoding.UTF8; //делаем поддержку кодировки

      CultureInfo culture; //enum для культуры
      if (Thread.CurrentThread.CurrentCulture.Name == "ru-RU")
         culture = CultureInfo.CreateSpecificCulture("en-US");
      else
         culture = CultureInfo.CreateSpecificCulture("ru-Ru");

      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;

      
      RandomCur(); //создаем и показываем три рандомных числа с валютой на текущем треде
      Thread.Sleep(1000);

      Thread thread1 = new Thread(new ThreadStart(CultureExample.RandomCur));
      thread1.Start();
   }

   private static void RandomCur()
   {
      Console.WriteLine("\nCurrent Culture: {0}", Thread.CurrentThread.CurrentCulture);
      Console.WriteLine("Current UI Culture: {0}", Thread.CurrentThread.CurrentUICulture);

      Console.Write("Random Currencies: ");
      Random rand = new Random();
      for (int val = 0; val < 3; val++)
         Console.Write("     {0:C2}     ", rand.NextDouble()); //точность и указывать, чтобы выводился формат валюты, принадлежащий стране

      Console.WriteLine();
   }
}
//Вывод будет такой:
//Current Culture: ru-RU
//Current UI Culture: ru-RU
//Random Currencies:      0,40 ₽          0,95 ₽          0,90 ₽
//
//Current Culture: en-US
//Current UI Culture: en-US
//Random Currencies:      $0.49          $0.11          $0.52
