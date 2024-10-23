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

        public bool TryGet(int index, out T item)
        {
            //
            if (_list.Count > 0 && index >= 0 && index < _list.Count)
            {
                item = _list[index];
                return true;
            }

            //
            item = default;
            return false;
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

        public void RemoveAt(int index)
        {
            if (_list.Count > 0 && index >= 0 && index < _list.Count)
            {
                _list.RemoveAt(index);
            }
        }

        public int IndexOf(T itemToFind)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Equals(itemToFind))
                {
                    return i;
                }
            }

            return -1;
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
            for (int i = 0; i < _list.Count; i++)
            {
                str += i + ": " + _list[i].ToString() + "\n";
            }
            return str;
        }

        //
        public int Count => _list.Count;
        public bool IsNullOrEmpty() => _list == null || _list.IsNullOrEmpty();
        public bool Contains(T item) => _list.Contains(item);
        public bool TryGetFirstOrDefault(out T item) => TryGet(0, out item);
        public void Add(T item) => _list.Add(item);
        public void AddRange(BetterList<T> list) => AddRange(list.ToList());
        public void Shuffle() => _list.Shuffle();
        public void Clear() => _list.Clear();
    }
}