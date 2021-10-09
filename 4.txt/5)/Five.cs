using System;
using System.IO;

namespace ConsoleApp6
{
    public delegate void KeyPressEventDelegate();

    class KeyLogger
    {
        public event KeyPressEventDelegate ThreeKeyPressed;
        public event KeyPressEventDelegate FiveKeyPressed;
        public event KeyPressEventDelegate DigitKeyPressed;
        public event KeyPressEventDelegate AnyKeyPressed;

        public void ThreeKeyPressedInvoke() => ThreeKeyPressed?.Invoke();
        public void FiveKeyPressedInvoke() => FiveKeyPressed?.Invoke();
        public void DigitKeyPressedInvoke() => DigitKeyPressed?.Invoke();
        public void AnyKeyPressedInvoke() => AnyKeyPressed?.Invoke();
    }
    class Subscribers
    {
        private static ConsoleKey key;

        public static void ThreeKeyPressed()
        {
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D3)
                    Console.WriteLine("3");
            }
        }

        public static void FiveKeyPressed()
        {
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D5)
                    Console.WriteLine("5");
            }
        }

        public static void DigitPressed()
        {
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (char.IsDigit((char)key))
                    Console.WriteLine((char)key);
            }
        }

        public static void AnyKeyPressed()
        {
            string path = @"log.txt"; //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DescktopDirectory),"log.txt");
            using StreamWriter txt = File.CreateText(path);
            txt.Dispose();
            if (File.Exists(path))
            {
                while (true)
                {
                    key = Console.ReadKey(true).Key;
                    Console.WriteLine((char)key);
                    File.AppendAllText(path, Convert.ToString(key));
                }
            }
        }
    }
    class Programm
    {
        static void Main()
        {
            KeyLogger keyLog = new();

            keyLog.ThreeKeyPressed += Subscribers.ThreeKeyPressed;
            keyLog.FiveKeyPressed += Subscribers.FiveKeyPressed;
            keyLog.DigitKeyPressed += Subscribers.DigitPressed;
            keyLog.AnyKeyPressed += Subscribers.AnyKeyPressed;
            keyLog.AnyKeyPressedInvoke();
        }
    }
}
