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
        public void InjectStateMachine(StateMachineMB<T> stateMachine)
        {
            Fsm = stateMachine as T;
            this.Log("BaseStateMachine Assigned");
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