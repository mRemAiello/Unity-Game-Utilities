using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public abstract class SingleStateMachineMB<T> : MonoBehaviour where T : MonoBehaviour
    {
        readonly Dictionary<Type, StateMB<T>> _statesRegister = new();
        StateMB<T> _currentState;

        public bool EnableLogs = true;
        public bool IsInitialized { get; private set; }

        // Initializes and registers all states attached to the same GameObject.
        public void Initialize()
        {
            // Allows subclasses to prepare data before state registration.
            OnBeforeInitialize();

            // Grabs all states compatible with this machine type.
            var allStates = GetComponents<StateMB<T>>();

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

        // Override to execute logic before states are initialized.
        protected virtual void OnBeforeInitialize()
        {
        }

        // Override to execute logic after all states are initialized.
        protected virtual void OnInitialize()
        {
        }

        // Initializes the machine and forwards Awake lifecycle to all registered states.
        protected virtual void Awake()
        {
            Initialize();
            foreach (var state in _statesRegister.Values)
                state.OnAwake();

            this.Log("States Awaken");
        }

        // Forwards Start lifecycle to all registered states.
        protected virtual void Start()
        {
            foreach (var state in _statesRegister.Values)
                state.OnStart();

            this.Log("States Started");
        }

        // Updates only the currently active state.
        protected virtual void Update()
        {
            if (_currentState != null)
                _currentState.OnUpdate();
        }

        // Checks whether the active state matches the requested type.
        public bool IsCurrent<T1>() where T1 : StateMB<T>
        {
            if (_currentState == null)
                return false;

            return _currentState.GetType() == typeof(T1);
        }

        // Checks whether the active state matches the provided state instance type.
        public bool IsCurrent(StateMB<T> state)
        {
            if (state == null)
                throw new ArgumentNullException();

            if (_currentState == null)
                return false;

            return _currentState.GetType() == state.GetType();
        }

        // Activates a state by type, replacing the current one.
        public void PushState<T1>(bool isSilent = false) where T1 : StateMB<T>
        {
            var stateType = typeof(T1);
            var state = _statesRegister[stateType];
            PushState(state, isSilent);
        }

        // Activates a state instance, replacing the current one.
        public void PushState(StateMB<T> state, bool isSilent = false)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!_statesRegister.ContainsKey(state.GetType()))
                throw new ArgumentException("State " + state + " not registered yet.");

            this.Log($"Operation: Push, state: {state.GetType()}");
            if (_currentState != null && !isSilent)
                _currentState.OnExitState();

            _currentState = state;
            _currentState.OnEnterState();
        }

        // Returns the currently active state.
        public StateMB<T> PeekState() => _currentState;

        // Deactivates the current state without restoring any previous one.
        public void PopState(bool isSilent = false)
        {
            if (_currentState == null)
                return;

            var state = _currentState;
            _currentState = null;
            this.Log($"Operation: Pop, state: {state.GetType()}");

            if (!isSilent)
                state.OnExitState();
        }
    }
}
