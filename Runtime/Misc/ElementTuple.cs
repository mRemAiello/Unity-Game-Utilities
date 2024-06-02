using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class ElementTuple<T1, T2>
    {
        [SerializeField] private T1 _item1;
        [SerializeField] private T2 _item2;

        // private fields
        public T1 Item1 => _item1;
        public T2 Item2 => _item2;

        public ElementTuple(T1 firstData, T2 secondData)
        {
            _item1 = firstData;
            _item2 = secondData;
        }
    }
}