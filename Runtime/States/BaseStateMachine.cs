using System;
using System.Collections.Generic;

namespace GameUtils
{
    public abstract class BaseStateMachine : ILoggable
    {
        readonly Stack<IState> stack = new();
        readonly Dictionary<Type, IState> register = new();

        //
        public bool IsInitialized { get; protected set; }
        public bool LogEnabled => true;
        public IStateMachineHandler Handler { get; set; }
        public IState Current => PeekState();

        //
        protected BaseStateMachine(IStateMachineHandler handler = null) => Handler = handler;

        //
        public void RegisterState(IState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("Null is not a valid state");
            }

            //
            var type = state.GetType();
            register.Add(type, state);
            this.Log($"{Handler.Name} Registered: {type}");
        }

        public void Initialize()
        {
            //create states
            OnBeforeInitialize();

            //register all states
            foreach (var state in register.Values)
                state.OnInitialize();

            //
            IsInitialized = true;
            OnInitialize();

            //
            this.Log($"{Handler.Name}, Initialized!");
        }

        protected virtual void OnBeforeInitialize()
        {
        }

        protected virtual void OnInitialize()
        {
        }

        public bool IsCurrent(IState state)
        {
            if (state == null)
                throw new ArgumentNullException();

            return Current?.GetType() == state.GetType();
        }

        public void PushState<T>(bool isSilent = false) where T : IState
        {
            var stateType = typeof(T);
            var state = register[stateType];
            PushState(state, isSilent);
        }

        public void PushState(IState state, bool isSilent = false)
        {
            var type = state.GetType();
            if (!register.ContainsKey(type))
                throw new ArgumentException("State " + state + " not registered yet.");

            this.Log($"{Handler.Name}, Push state: {type}");
            if (stack.Count > 0 && !isSilent)
                Current?.OnExitState();

            stack.Push(state);
            state.OnEnterState();
        }

        public void PopState(bool isSilent = false)
        {
            if (Current == null)
                return;

            var state = stack.Pop();
            this.Log($"{Handler.Name}, Pop state: {state.GetType()}");
            state.OnExitState();

            if (!isSilent)
                Current?.OnEnterState();
        }

        public virtual void Clear()
        {
            foreach (var state in register.Values)
                state.OnClear();

            //
            stack.Clear();
            register.Clear();
        }

        //
        public void Update() => Current?.OnUpdate();
        public bool IsCurrent<T>() where T : IState => Current?.GetType() == typeof(T);
        public IState PeekState() => stack.Count > 0 ? stack.Peek() : null;
    }
}