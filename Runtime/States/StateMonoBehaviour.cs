using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Debug")]
    public abstract class StateMonoBehaviour<T> : MonoBehaviour, ILoggable where T : MonoBehaviour
    {
        [SerializeField, Group("Debug")] private bool _logEnabled = true;

        //
        public T StateMachine { get; private set; }
        public bool LogEnabled => _logEnabled;

        //
        public void InjectStateMachine(StackStateMachineMonoBehaviour<T> stateMachine)
        {
            StateMachine = stateMachine as T;
            this.Log("BaseStateMachine Assigned");
        }

        // Injects a single-state machine reference while keeping the same typed FSM API.
        public void InjectStateMachine(SingleStateMachineMonoBehaviour<T> stateMachine)
        {
            StateMachine = stateMachine as T;
            this.Log("SingleStateMachine Assigned");
        }

        public virtual void OnEnterState(StateMonoBehaviour<T> prevState)
        {
            var from = prevState != null ? prevState.GetType().Name : "None";
            this.Log($"OnEnterState from {from}");
        }

        public virtual void OnExitState(StateMonoBehaviour<T> nextState)
        {
            var to = nextState != null ? nextState.GetType().Name : "None";
            this.Log($"OnExitState to {to}");
        }

        //
        public virtual void OnAwake() => this.Log("OnAwake");
        public virtual void OnStart() => this.Log("OnStart");
        public virtual void OnUpdate() => this.Log("OnUpdate");
    }
}
