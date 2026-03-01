using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class SingleStateMachineMonoBehaviour<T> : MonoBehaviour, ILoggable where T : MonoBehaviour
    {
        [SerializeField] private bool _logEnabled = false;
        [NonSerialized] private StateMonoBehaviour<T> _currentState;
        readonly Dictionary<Type, StateMonoBehaviour<T>> _statesRegister = new();

        //
        [ShowInInspector, ReadOnly] public StateMonoBehaviour<T> CurrentState => _currentState;
        public bool LogEnabled => _logEnabled;
        public bool IsInitialized { get; private set; }

        // 
        public void Initialize()
        {
            // Allows subclasses to prepare data before state registration.
            OnBeforeInitialize();

            // Grabs all states compatible with this machine type.
            var allStates = GetComponents<StateMonoBehaviour<T>>();

            // Registers each state and injects this machine reference.
            foreach (var state in allStates)
            {
                var type = state.GetType();
                _statesRegister.Add(type, state);
                state.InjectStateMachine(this);
            }

            // Marks the machine as initialized and executes subclass hooks.
            IsInitialized = true;
            OnInitialize();

            // Logs the initialization completion.
            this.Log("Initialized!");
        }

        // Initializes the machine and forwards Awake lifecycle to all registered states.
        private void Awake()
        {
            Initialize();
            foreach (var state in _statesRegister.Values)
                state.OnAwake();

            //
            OnPostAwake();

            //
            this.Log("States Awaken");
        }

        // Forwards Start lifecycle to all registered states.
        private void Start()
        {
            foreach (var state in _statesRegister.Values)
                state.OnStart();

            //
            OnPostStart();

            //
            this.Log("States Started");
        }

        // Updates only the currently active state.
        private void Update()
        {
            if (_currentState != null)
                _currentState.OnUpdate();

            //
            OnPostUpdate();
        }

        // Checks whether the active state matches the requested type.
        public bool IsCurrent<T1>() where T1 : StateMonoBehaviour<T>
        {
            if (_currentState == null)
                return false;

            return _currentState.GetType() == typeof(T1);
        }

        // Checks whether the active state matches the provided state instance type.
        public bool IsCurrent(StateMonoBehaviour<T> state)
        {
            if (state == null)
                throw new ArgumentNullException();

            if (_currentState == null)
                return false;

            return _currentState.GetType() == state.GetType();
        }

        // Activates a state by type, replacing the current one.
        public void ChangeState<T1>(bool isSilent = false) where T1 : StateMonoBehaviour<T>
        {
            var stateType = typeof(T1);
            var state = _statesRegister[stateType];
            ChangeState(state, isSilent);
        }

        // Activates a state instance, replacing the current one.
        public void ChangeState(StateMonoBehaviour<T> state, bool isSilent = false)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!_statesRegister.ContainsKey(state.GetType()))
                throw new ArgumentException("State " + state + " not registered yet.");

            this.Log($"State changed to: {state.GetType()}");
            if (_currentState != null && !isSilent)
                _currentState.OnExitState();

            _currentState = state;
            _currentState.OnEnterState();
        }

        //
        protected virtual void OnBeforeInitialize() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnPostAwake() { }
        protected virtual void OnPostStart() { }
        protected virtual void OnPostUpdate() { }
    }
}
