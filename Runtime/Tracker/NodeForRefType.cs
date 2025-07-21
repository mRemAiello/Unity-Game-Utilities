using System;

namespace GameUtils
{
    public class NodeForRefType<T> : BaseNode<T> where T : class
    {
        public NodeForRefType(Func<T> getter, Action onChangedCallback1, Action<T> onChangedCallback2, Action<T, T> onChangedCallback3)
            : base(getter, onChangedCallback1, onChangedCallback2, onChangedCallback3)
        { }

        protected override bool Equal(T a, T b)
        {
            return a == b;
        }
    }
}