using System;

namespace GameUtils
{
    public abstract class BaseNode<T> : ITrackerNode<T>
    {
        private Func<T> _getter;
        private Action _onChangedCallback1;
        private Action<T> _onChangedCallback2;
        private Action<T, T> _onChangedCallback3;
        private T _prevValue;
        private T _curValue;
        private bool _changed;

        public bool Changed => _changed;
        public T PreviousValue => _prevValue;
        public T CurrentValue => _curValue;

        public BaseNode(Func<T> getter, Action onChangedCallback1, Action<T> onChangedCallback2, Action<T, T> onChangedCallback3)
        {
            _getter = getter;
            _curValue = getter.Invoke();
            _onChangedCallback1 = onChangedCallback1;
            _onChangedCallback2 = onChangedCallback2;
            _onChangedCallback3 = onChangedCallback3;
        }

        public void Check()
        {
            T newValue = _getter();

            if (_changed = !Equal(_curValue, newValue))
            {
                try
                {
                    _onChangedCallback1?.Invoke();
                    _onChangedCallback2?.Invoke(newValue);
                    _onChangedCallback3?.Invoke(_curValue, newValue);
                }
                finally
                {
                    _prevValue = _curValue;
                    _curValue = newValue;
                }
            }
        }

        public void ForceInvoke()
        {
            _onChangedCallback1?.Invoke();
            _onChangedCallback2?.Invoke(_curValue);
            _onChangedCallback3?.Invoke(_prevValue, _curValue);
        }

        protected abstract bool Equal(T a, T b);
    }
}
