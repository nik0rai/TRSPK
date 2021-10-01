internal class Item<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Item(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            Key = key;
            Value = value;
        }

        public Item() //без него не работает
        {
        }

        public override string ToString() => Value.ToString();       
    }
