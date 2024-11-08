using UnityEngine;

namespace GameUtils
{
    public class Constant<T> : ScriptableObject
    {
        [SerializeField] private T _value;

        //
        public T Value => _value;
    }
}