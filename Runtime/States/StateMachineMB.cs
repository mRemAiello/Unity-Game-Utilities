using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public abstract class StateMachineMB<T> : MonoBehaviour where T : MonoBehaviour
    {
        readonly Stack<StateMB<T>> _stack = new();
        readonly Dictionary<Type, StateMB<T>> _statesRegister = new();
        public bool EnableLogs = true;
        public bool IsInitialized { get; private set; }

        //
        public void Initialize()
        {
            OnBeforeInitialize();

            //grab all states of this BaseStateMachine Type attached to this gameobject
            var allStates = GetComponents<StateMB<T>>();

            //StatesRegister all states
            foreach (var state in allStates)
            {
                var type = state.GetType();
                _statesRegister.Add(type, state);
                state.InjectStateMachine(this);
            }

            //
            IsInitialized = true;
            OnInitialize();

            //
            this.Log("Initialized!");
        }

        protected virtual void OnBeforeInitialize()
        {
        }

        protected virtual void OnInitialize()
        {
        }
        protected virtual void Awake()
        {
            Initialize();
            foreach (var state in _statesRegister.Values)
                state.OnAwake();

            this.Log("States Awaken");
        }

        protected virtual void Start()
        {
            foreach (var state in _statesRegister.Values)
                state.OnStart();

            this.Log("States Started");
        }

        protected virtual void Update()
        {
            var current = PeekState();
            if (current != null)
                current.OnUpdate();
        }

        public bool IsCurrent<T1>() where T1 : StateMB<T>
        {
            var current = PeekState();
            if (current == null)
                return false;
            return current.GetType() == typeof(T1);
        }

        public bool IsCurrent(StateMB<T> state)
        {
            if (state == null)
                throw new ArgumentNullException();

            var current = PeekState();
            if (current == null)
                return false;
            return current.GetType() == state.GetType();
        }

        public void PushState<T1>(bool isSilent = false) where T1 : StateMB<T>
        {
            var stateType = typeof(T1);
            var state = _statesRegister[stateType];
            PushState(state, isSilent);
        }

        public void PushState(StateMB<T> state, bool isSilent = false)
        {
            if (!_statesRegister.ContainsKey(state.GetType()))
                throw new ArgumentException("State " + state + " not registered yet.");

            this.Log($"Operation: Push, state: {state.GetType()}");
            if (_stack.Count > 0 && !isSilent)
            {
                var previous = _stack.Peek();
                previous.OnExitState();
            }

            _stack.Push(state);
            state.OnEnterState();
        }

        public StateMB<T> PeekState()
        {
            StateMB<T> state = null;
            if (_stack.Count > 0)
                state = _stack.Peek();

            return state;
        }

        public void PopState(bool isSilent = false)
        {
            if (_stack.Count > 0)
            {
                var state = _stack.Pop();
                this.Log($"Operation: Pop, state: {state.GetType()}");
                state.OnExitState();
            }

            if (_stack.Count > 0 && !isSilent)
            {
                var state = _stack.Peek();
                state.OnEnterState();
            }
        }
    }
}