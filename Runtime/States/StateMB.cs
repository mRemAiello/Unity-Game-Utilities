using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public abstract class StateMB<T> : MonoBehaviour, ILoggable where T : MonoBehaviour
    {
        [SerializeField, Group("debug")] private bool _logEnabled = true;

        //
        public T Fsm { get; private set; }
        public bool LogEnabled => _logEnabled;

        //
        public void InjectStateMachine(StackStateMachineMB<T> stateMachine)
        {
            Fsm = stateMachine as T;
            this.Log("BaseStateMachine Assigned");
        }

        // Injects a single-state machine reference while keeping the same typed FSM API.
        public void InjectStateMachine(SingleStateMachineMB<T> stateMachine)
        {
            Fsm = stateMachine as T;
            this.Log("SingleStateMachine Assigned");
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }

        //
        public virtual void OnEnterState() => this.Log("OnEnterState <---------");
        public virtual void OnExitState() => this.Log("OnExitState <---------");
    }
}
