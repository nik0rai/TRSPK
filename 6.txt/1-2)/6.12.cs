using System;
using System.Reflection;

namespace ConsoleApplication1
{
    class Program
    {
        public struct LimitList
        {
            public int MinValue;
            public int MaxValue;
            public bool ZeroEnabled;
        }
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class NameAttribute : Attribute
        {
            public string Description;
            public NameAttribute() { }
            public NameAttribute(string str)
            {
                Description = str;
            }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
        public class ValidateInt32Attribute : Attribute
        {
            public int MinValue { get; set; }
            public int MaxValue { get; set; }
            public bool ZeroEnabled { get; set; }
            public ValidateInt32Attribute(int MinValue, int MaxValue, bool ZeroEnabled)
            {
                this.MinValue = MinValue;
                this.MaxValue = MaxValue;
                this.ZeroEnabled = ZeroEnabled;
            }
        }
        public class InvalidValueException : Exception
        {
            public string FieldName;
            public int currentfieldvalue;
            public LimitList list;
            public InvalidValueException(string FieldName, int currentfieldvalue, LimitList l)
            {
                this.FieldName = FieldName;
                this.currentfieldvalue = currentfieldvalue;
                this.list.MaxValue = l.MaxValue;
                this.list.MinValue = l.MinValue;
                this.list.ZeroEnabled = l.ZeroEnabled;
            }
        }
        public class FirstClass
        {
            [ValidateInt32Attribute(-1, 2, true)]
            [NameAttribute("1")]
            public int first { get; set; }

            [ValidateInt32Attribute(3, 4, true)]
            [NameAttribute("2")]
            public int second { get; set; }

            [ValidateInt32Attribute(1, 3, true)]
            [NameAttribute("3")]
            public int thirth { get; set; }

            [ValidateInt32Attribute(-1, 3, false)]
            [NameAttribute("4")]
            public int fourth { get; set; }

            [ValidateInt32Attribute(-1, 6, true)]
            [NameAttribute("5")]
            public int fiveths { get; set; }

            [ValidateInt32Attribute(-5, 6, true)]
            [NameAttribute("6")]
            public int sixths { get; set; }

            [ValidateInt32Attribute(-1, 3, true)]
            [NameAttribute("7")]
            public int sevenths { get; set; }

            [ValidateInt32Attribute(-2, 8, true)]
            [NameAttribute("8")]
            public int eights { get; set; }

            [ValidateInt32Attribute(-1, 3, true)]
            [NameAttribute("9")]
            public int nineths { get; set; }

            [ValidateInt32Attribute(-4, 3, true)]
            [NameAttribute("10")]
            public int tenths { get; set; }
        }
        public static class Int32Validate
        {
            public static bool Validate<FirstClass>(FirstClass obj)
            {
                foreach (PropertyInfo prop in typeof(FirstClass).GetProperties())
                {
                    if (prop.PropertyType == typeof(Int32))
                    {
                        ValidateInt32Attribute validAt = (ValidateInt32Attribute)prop.GetCustomAttribute(typeof(ValidateInt32Attribute));
                        if (validAt != null)
                        {
                            if (((Int32)prop.GetValue(obj, null) > validAt.MaxValue) || ((Int32)prop.GetValue(obj, null) < validAt.MinValue) || ((Int32)prop.GetValue(obj, null) == 0 && validAt.ZeroEnabled == false))
                            {
                                LimitList list;
                                list.MaxValue = validAt.MaxValue;
                                list.MinValue = validAt.MinValue;
                                list.ZeroEnabled = validAt.ZeroEnabled;
                                throw new InvalidValueException(prop.Name, (Int32)prop.GetValue(obj, null), list);
                            }
                        }
                    }
                }
                return true;
            }
            static void Main()
            {
                PropertyInfo[] obj1 = typeof(FirstClass).GetProperties();
                foreach (PropertyInfo pi in obj1)
                {
                    NameAttribute nw = new();
                    nw = (NameAttribute)pi.GetCustomAttribute(typeof(NameAttribute));
                    Console.WriteLine("{0} - {1}", pi.ToString(), nw.Description);
                }
                FirstClass fc = new()
                {
                    first = 1,
                    second = 1,
                    thirth = 10
                };

                try
                {
                    Int32Validate.Validate<FirstClass>(fc);
                }
                catch (InvalidValueException ob)
                {
                    Console.WriteLine("Ошибка! Имя поля: {0}, Текущее значение: {1}, Максимальное значение: {2}, Минимальное значение: {3}", ob.FieldName, ob.currentfieldvalue, ob.list.MaxValue, ob.list.MinValue);
                }
            }
        }
    }
}
