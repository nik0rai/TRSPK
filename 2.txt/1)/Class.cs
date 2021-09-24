    class Man
    {
        public string Name;
        public uint Age;

        public Man(string name, uint age)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();           
            Name = name;
            Age = age;
        }

        public virtual string Display() => (nameof(Man) + ' ' + Name + ' ' + Age);
    };

    class Teenager : Man
    {
        public string School;

        public Teenager(string name, uint age, string school) : base(name, age)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();
            if (age < 13 || age > 19) throw new ArgumentOutOfRangeException();
            School = school;
        }

        public override string Display() => (nameof(Teenager) + ' ' + Name + ' ' + Age + " Place of study: " + School);

    };

    class Worker : Man
    {
        public string Workplace;

        public Worker(string name, uint age, string workplace) : base(name, age)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();
            if (age < 16 || age > 70) throw new ArgumentOutOfRangeException();
            Workplace = workplace;
        }

        public override string Display() => (nameof(Worker) + ' ' + Name + ' ' + Age + " Place of work: " + Workplace);
    };