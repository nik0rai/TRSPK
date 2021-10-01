class ArrEl<TKey, TValue>
    {
        private readonly List<Item<TKey, TValue>> _items = new();
        public int Count => _items.Count; //кол-во элементов
        public IReadOnlyList<TKey> Keys => (IReadOnlyList<TKey>)_items.Select(i => i.Key).ToList(); //коллекция ключей

        public void Add(Item<TKey, TValue> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_items.Any(i => i.Key.Equals(item.Key))) throw new ArgumentException("Element is already exist.", nameof(item));
            
            _items.Add(item); //добавляем данные
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (value == null) throw new ArgumentNullException(nameof(value));

            if (_items.Any(i => i.Key.Equals(key))) throw new ArgumentException("Element is already exist.", nameof(key));

            //cоздаем новый элемент хранимых данных
            var item = new Item<TKey, TValue>()
            {
                Key = key,
                Value = value
            };
            
            _items.Add(item); //добавляем данные 
        }

        public void Delete(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            //элемент данных из коллекции по ключу
            var item = _items.SingleOrDefault(i => i.Key.Equals(key));

            //если нашли по ключу, то удаляем
            if (item != null) _items.Remove(item);
        }

        public TValue Get(TKey key)
        {           
            if (key == null) throw new ArgumentNullException(nameof(key));

            var item = _items.SingleOrDefault(i => i.Key.Equals(key)) ??
                throw new ArgumentException("Element is already exist.", nameof(key));

            return item.Value;
        }
        
        public TKey Get(TValue val)
        {
            if (val == null) throw new ArgumentNullException(nameof(val));

            var item = _items.SingleOrDefault(i => i.Value.Equals(val)) ??
                throw new ArgumentException("Element is already exist.", nameof(val));

            return item.Key;
        }
    }
