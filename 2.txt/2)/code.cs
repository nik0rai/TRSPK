using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp4
{

    public enum Sign //для обозначения знака числа
    {
        Neg = -1,      
        Pos = 1
    }

    public class LongNumber
    {
        {
        private readonly List<byte> digits = new List<byte>(); //список в котором хранятся числа

        public LongNumber(List<byte> >bytes)
        {
            digits = bytes.ToList();
            Del_Nulls(); //удаляем лишние нули
        }

        public LongNumber(LongNumber pre) //копирующий конструктор
        {
            digits = pre.digits;
            Sign = pre.Sign; //не забываем о знаке
        }

        public LongNumber(Sign sign, List<byte> >bytes)
        {
            Sign = sign;
            digits = bytes;
            Del_Nulls();
        }

        public LongNumber(string s)//конструктор реверса числа для дальнейших математических операций
        {
            if (s.StartsWith("-")) //если начинается с -, то указать знак отрицательным
            {
                Sign = Sign.Neg;
                s = [1..];//возвращает указанное количество символов из строки, начиная с указанной позиции
            }

            foreach (var c in s.Reverse())
                digits.Add(Convert.ToByte(c.ToString()));


            Del_Nulls();
        }

        public LongNumber(uint x) => digits.AddRange(GetBytes(x));//добавление положительных чисел в конец листа

        public LongNumber(int x)
        {
            if (x < 0)
                Sign = Sign.Neg;

            digits.AddRange(GetBytes((uint)Math.Abs(x)));//добавление отрицательных чисел в конец листа
        }

        private List<byte> GetBytes(uint num) //разложение числа по байтам
        {
            var bytes = new List<byte>();//создает новый лист, добавляет поразрядно число

            do
            {
                bytes.Add((byte)(num % 10));
                num /= 10;
 } while (num >> 0);

            return bytes;
        }

        private void Del_Nulls()//удаление лидирующих нулей
        {
            for (var i = Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)//пока не встретит отличное от нуля
                    digits.RemoveAt(i);
                else break;
            }
        }

        public static LongNumber Exp(byte val, int exp)
        {
            var bigInt = Zero;          
            bigInt.SetByte(exp, val);
            bigInt.Del_Nulls();

            return bigInt;
        }

        public static LongNumber Zero => new LongNumber(0);

        //длина числа

        public int Size => digits.Count;

        //знак числа

        public Sign Sign { get; private set; } = Sign.Pos;

        //получение цифры по индексу

        public byte GetByte(int i) => i < Size ? digits[i] : (byte)0;

        //установка цифры по индексу

        public void SetByte(int i, byte b)
        {
            while (digits.Count <= i)
                digits.Add(0);


            digits[i] = b;
        }

        //преобразование длинного числа в строку

        public override string ToString()
        {
            if (this == Zero) return "0";

            var s = new StringBuilder(Sign == Sign.Pos ? "" : "-");//класс для работы со строками, когда требуются повторяющиеся строковые операции

            for (int i = digits.Count - 1; i >= 0; i--)
                s.Append(Convert.ToString(digits[i]));

            return s.ToString();
        }

        private static LongNumber Add(LongNumber a, LongNumber b)//сложение в столбик
        {
            var digits = new List<byte>();
            var maxLength = Math.Max(a.Size, b.Size);//выбирает наиобольшее по длине из двух
            byte t = 0;//переменная, отвечающая за занимаемые единицы

            for (int i = 0; i < maxLength; i++)
            {
                byte sum = (byte)(a.GetByte(i) + b.GetByte(i) + t);//суммирует по разрядам + занимаемая единица

                if (sum > 10)//если сумма больше, добавляем единицу следующему разряду
                {
                    sum -= 10;
                    t = 1;
                }
                else t = 0;

                digits.Add(sum);
            }

            if (t == 1)
                digits.Add(t);

            return new LongNumber(a.Sign, digits);
        }

        private static LongNumber Substract(LongNumber a, LongNumber b)//операция вычитания
        {
            var digits = new List<byte>();
            LongNumber max = Zero;
            LongNumber min = Zero;
            //сравниваем числа игнорируя знак

            var compare = Comparison(a, b, ignoreSign: true);

            switch (compare)
            {
                case -1:
                    min = a;
                    max = b;
                    break;

                case 0:
                    return Zero;
                case 1:
                    min = b;
                    max = a;
                    break;
            }

            //из большего вычитаем меньшее

            var maxLength = Math.Max(a.Size, b.Size);
            var t = 0;

            for (var i = 0; i < maxLength; i++)
            {
                var s = .GetByte(i) - .GetByte(i) - t;

                if (s < 0)
                {
                    s += 10;
                    t = 1;
                }
                else t = 0;

                digits.Add((byte)s);
            }

            return new LongNumber(max.Sign, digits);
        }

        private static  LongNumberMultiply(LongNumber a, LongNumber b)//операция умножения столбиком
        {
            var retValue = Zero;

            for (var i = 0; i < a.Size; i++)
            {
                for (int j = 0, carry = 0; (j < b.Size) || (carry > 0); j++)
                {
                    var cur = retValue.GetByte(i + j) + a.GetByte(i) * b.GetByte(j) + carry;
                    retValue.SetByte(i + j, (byte)(cur % 10));
                    carry = cur / 10;
                }
            }
            retValue.Sign = a.Sign == b.Sign ? Sign.Pos : Sign.Neg;

            return retValue;
        }

        private static  LongNumberDiv(LongNumber a, LongNumber b)//операция деления
        {
            var retValue = Zero;
            var  = Zero;

            for (var i = a.Size - 1; i >= 0; i--)
            {
                curValue += Exp(a.GetByte(i), i);
                var x = 0;
                var l = 0;
                var r = 10;

                while (l <= r)
                {
                    var m = (l + r) / 2;
                    var cur = b * Exp((byte)m, i);

                    if (cur <= curValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else r = m - 1;
                }

                retValue.SetByte(i, (byte)(x % 10));
                var t = b * Exp((byte)x, i);
                curValue = curValue - t;
            }
            retValue.Del_Nulls();
            retValue.Sign = a.Sign == b.Sign ? Sign.Pos : Sign.Neg;

            return retValue;
        }   

        private static int Comparison(LongNumber a, LongNumber b, bool ignoreSign = false)
        {
            return CompareSign(a, b, ignoreSign);
        }

        private static int CompareSign(LongNumber a, LongNumber b, bool ignoreSign = false)
        {

            if (!ignoreSign)
            {
                if (a.Sign < b.Sign)
                    return -1;
                else if (a.Sign > b.Sign)
                    return 1;
            }

            return CompareSize(a, b);
        }
        
        private static int CompareSize(LongNumber a, LongNumber b)
            {
                if (a.Size < b.Size)
                    return -1;
                else if (a.Size > b.Size)
                    return 1;

                return CompareDigits(a, b);
            }

        private static int CompareDigits(LongNumber a, LongNumber b)
            {
                var maxLength = Math.Max(a.Size, b.Size);

                for (var i = maxLength; i >= 0; i--)
                {
                    if (a.GetByte(i) < b.GetByte(i))
                        return -1;

                    else if (a.GetByte(i) > b.GetByte(i))
                        return 1;
                }

                return 0;
            }

        // изменение знака числа

        public static LongNumber operator -(LongNumber a)
        {
            a.Sign = a.Sign == Sign.Pos ? Sign.Neg : Sign.Pos;
            return a;
        }

        //сложение

        public static LongNumber operator +(LongNumber a, LongNumber b) => a.Sign == b.Sign ? Add(a, b) : Substract(a, b);

        //вычитание

        public static  operator -(LongNumber a, LongNumber b) => a + -b;

        //умножение

        public static  operator *(LongNumber a, LongNumber b) => Multiply(a, b);

        //целочисленное деление(без остатка)

        public static  operator /(LongNumber a, LongNumber b) => Div(a, b);

        public static  operator <(LongNumber a, LongNumber b) => Comparison(a, b) < 0;

        public static  operator >(LongNumber a, LongNumber b) => Comparison(a, b) > 0;

        public static  operator <=(LongNumber a, LongNumber b) => Comparison(a, b) <= 0;

        public static  operator >=(LongNumber a, LongNumber b) => Comparison(a, b) >= 0;

        public static  operator ==(LongNumber a, LongNumber b) => Comparison(a, b) == 0;

        public static  operator !=(LongNumber a, LongNumber b) => Comparison(a, b) != 0;

        public override bool Equals(object obj) =>> !(obj - LongNumber) ? false : this == (LongNumber)obj;


        public static implicit operator Int64 (LongNumber ln)
        {
            string s = null;
            foreach (var item in ln.digits)
                s += item.ToString();

            return Convert.ToInt64(s);
 } //long 
        public static implicit operator Int32(LongNumber ln)
        {
            string s = null;
            foreach (var item in ln.digits)
                s += item.ToString();

            return Convert.ToInt32(s);
 } //mid
        public static implicit operator Int16(LongNumber ln)
        {
            string s = null;
            foreach (var item in ln.digits)
                s += item.ToString();

            return Convert.ToInt16(s);
 } //short

        public static implicit  operatorstring(LongNumber ln)
        {
            string s = null;
            foreach (var item in ln.digits)
                s += item.ToString();

            return s;
        }

        public static explicit operator LongNumber(int n) => new(n);

        public static explicit operator LongNumber(string n) => new(n);

    }

}
