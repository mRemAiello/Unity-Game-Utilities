using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class ElementTuple<T, K>
    {
        [SerializeField] private T _firstElement;
        [SerializeField] private K _secondElement;

        // private fields
        public T FirstElement => _firstElement;
        public K SecondElement => _secondElement;

        public ElementTuple(T firstData, K secondData)
        {
            _firstElement = firstData;
            _secondElement = secondData;
        }
    }
}