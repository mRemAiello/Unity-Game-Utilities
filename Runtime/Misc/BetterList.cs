using System.Collections.Generic;

namespace GameUtils
{
    public class BetterList<T>
    {
        private readonly string _listName;
        private readonly List<T> _list = new();

        //
        public BetterList(string name)
        {
            _listName = name;
            _list = new();
        }

        public BetterList(string name, List<T> list)
        {
            _listName = name;
            _list = list;
        }

        public void InsertAt(T item, int index)
        {
            if (index < 0 || index > _list.Count)
            {
                _list.Add(item);
                return;
            }

            //
            _list.Insert(index, item);
        }

        public bool TryGetFirstOrDefault(out T item)
        {
            //
            if (_list.Count > 0)
            {
                item = _list[0];
                return true;
            }

            //
            item = default;
            return false;
        }

        public void AddRange(List<T> items)
        {
            foreach (T item in items)
            {
                if (item != null)
                {
                    _list.Add(item);
                }
            }
        }

        public List<T> ToList()
        {
            List<T> list = new();
            _list.ForEach(item => list.Add(item));
            
            //
            return list;
        }

        public override string ToString()
        {
            string str = "List Name: " + _listName + "\n";
            for(int i = 0; i < _list.Count; i++)
            {
                str += i + ": " + _list[i].ToString() + "\n";
            }
            return str;
        }

        //
        public int Count => _list.Count;
        public void Add(T item) => _list.Add(item);
        public void AddRange(BetterList<T> list) => AddRange(list.ToList());
        public void Clear() => _list.Clear();
    }
}