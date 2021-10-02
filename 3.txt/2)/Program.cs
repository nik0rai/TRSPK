using System;
using System.ComponentModel.DataAnnotations;
//Добавить в классы свойства Имя, Возраст, Школа, Место работы, и реализовать их параллельно методам изменения полей
namespace Man
{
    class Man
    {
        protected uint age;
        protected string name;
        public string Name
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    name = value;
                }
            }
            get => name;
        }
    
        public uint Age { get; set; }

        public void changeName (string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(); //Исключение: поле не может быть пустым
            Name = name;            
        }

        public virtual void changeAge (uint age) =>  Age = age;

        public override string ToString() => (nameof(Man) + ' ' + Name + ' ' + Age); //Пара virtual-override для использования метода в других классах с учетом изменений
    };
    class Teenager : Man // наследник класса Man 
    {
        public string School { get; set; }
        new public uint Age
        {
            set
            {
                if (value < 13 || value > 19)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    age = value;
                }
            }
            get { return age; }
        }

        public override void changeAge(uint age)
        {
            Age = age;
            if (age < 13 || age > 19) throw new ArgumentOutOfRangeException(); //Исключение: выход за диапазон допустимых значений
        }

        public override string ToString() => (nameof(Teenager) + ' ' + Name + ' ' + Age + " Place of study: " + School); //Пара virtual-override для использования метода в других классах с учетом изменений

    };
    class Worker : Man
    {
        public string Workplace { get; set; }

        new public uint Age
        {
            set
            {
                if (value < 16 || value > 70)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    age = value;
                }
            }
            get { return age; }
        }

        public override string ToString() => (nameof(Worker) + ' ' + Name + ' ' + Age + " Place of work: " + Workplace); //Пара virtual-override для использования метода в других классах с учетом изменений
    }; 
}
