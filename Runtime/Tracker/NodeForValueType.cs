using System;

namespace GameUtils
{
    public class NodeForValueType<T> : BaseNode<T> where T : struct, IEquatable<T>
    {
        public NodeForValueType(Func<T> getter, Action onChangedCallback1, Action<T> onChangedCallback2, Action<T, T> onChangedCallback3)
            : base(getter, onChangedCallback1, onChangedCallback2, onChangedCallback3)
        { }

        protected override bool Equal(T a, T b)
        {
            return a.Equals(b);
        }
    }
}
