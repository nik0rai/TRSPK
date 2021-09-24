using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp4
{

    public enum Sign
    {
        Neg = -1,      
        Pos = 1
    }

    public class LongNumber
    {
        private readonly List<byte> digits = new List<byte>();

        public LongNumber(List<byte> bytes)
        {
            digits = bytes.ToList();
            Del_Nulls();
        }

        public LongNumber(LongNumber pre)
        {
            digits = pre.digits;
            Sign = pre.Sign;
        }

        public LongNumber(Sign sign, List<byte> bytes)
        {
            Sign = sign;
            digits = bytes;
            Del_Nulls();
        }

        public LongNumber(string s)
        {
            if (s.StartsWith("-"))
            {
                Sign = Sign.Neg;
                s = s.Substring(1);
            }

            foreach (var c in s.Reverse())
                digits.Add(Convert.ToByte(c.ToString()));


            Del_Nulls();
        }

        public LongNumber(uint x) => digits.AddRange(GetBytes(x));

        public LongNumber(int x)
        {
            if (x < 0)
                Sign = Sign.Neg;

            digits.AddRange(GetBytes((uint)Math.Abs(x)));
        }

        private List<byte> GetBytes(uint num)
        {
            var bytes = new List<byte>();

            do
            {
                bytes.Add((byte)(num % 10));
                num /= 10;
            } while (num > 0);

            return bytes;
        }

        private void Del_Nulls()
        {
            for (var i = digits.Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)
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

        public static LongNumber One => new LongNumber(1);

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

            var s = new StringBuilder(Sign == Sign.Pos ? "" : "-");

            for (int i = digits.Count - 1; i >= 0; i--)
                s.Append(Convert.ToString(digits[i]));

            return s.ToString();
        }

        private static LongNumber Add(LongNumber a, LongNumber b)
        {
            var digits = new List<byte>();
            var maxLength = Math.Max(a.Size, b.Size);
            byte t = 0;

            for (int i = 0; i < maxLength; i++)
            {
                byte sum = (byte)(a.GetByte(i) + b.GetByte(i) + t);

                if (sum > 10)
                {
                    sum -= 10;
                    t = 1;
                }
                else t = 0;

                digits.Add(sum);
            }

            if (t > 0)
                digits.Add(t);

            return new LongNumber(a.Sign, digits);
        }

        private static LongNumber Substract(LongNumber a, LongNumber b)
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
                var s = max.GetByte(i) - min.GetByte(i) - t;

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

        private static LongNumber Multiply(LongNumber a, LongNumber b)
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

        private static LongNumber Div(LongNumber a, LongNumber b)
        {
            var retValue = Zero;
            var curValue = Zero;

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

        private static LongNumber Mod(LongNumber a, LongNumber b)
        {
            var retValue = Zero;
            for (var i = a.Size - 1; i >= 0; i--)
            {
                retValue += Exp(a.GetByte(i), i);
                var x = 0;
                var l = 0;
                var r = 10;

                while (l <= r)
                {
                    var m = (l + r) >> 1;
                    var cur = b * Exp((byte)m, i);

                    if (cur <= retValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else r = m - 1;
                }

                retValue -= b * Exp((byte)x, i);
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

        public static LongNumber operator +(LongNumber a, LongNumber b) => a.Sign == b.Sign

        ? Add(a, b)

        : Substract(a, b);

        //вычитание

        public static LongNumber operator -(LongNumber a, LongNumber b) => a + -b;

        //умножение

        public static LongNumber operator *(LongNumber a, LongNumber b) => Multiply(a, b);

        //целочисленное деление(без остатка)

        public static LongNumber operator /(LongNumber a, LongNumber b) => Div(a, b);

        //остаток от деления

        public static LongNumber operator %(LongNumber a, LongNumber b) => Mod(a, b);

        public static bool operator <(LongNumber a, LongNumber b) => Comparison(a, b) < 0;

        public static bool operator >(LongNumber a, LongNumber b) => Comparison(a, b) > 0;

        public static bool operator <=(LongNumber a, LongNumber b) => Comparison(a, b) <= 0;

        public static bool operator >=(LongNumber a, LongNumber b) => Comparison(a, b) >= 0;

        public static bool operator ==(LongNumber a, LongNumber b) => Comparison(a, b) == 0;

        public static bool operator !=(LongNumber a, LongNumber b) => Comparison(a, b) != 0;

        public override bool Equals(object obj) => !(obj is LongNumber) ? false : this == (LongNumber)obj;


        public static implicit operator Int64 (LongNumber ln)
        {
            string s = null;
            foreach (var item in ln.digits)
                s += item.ToString();

            return Convert.ToInt64(s);
        }  //long 
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

        public static implicit operator string(LongNumber ln)
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
