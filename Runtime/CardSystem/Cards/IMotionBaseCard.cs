using System;
using System.Collections;
using UnityEngine;

namespace GameUtils
{
    public abstract class IMotionBaseCard
    {
        public Action OnFinishMotion = () => { };
        public bool IsOperating { get; protected set; }
        protected virtual float Threshold => 0.01f;
        protected Vector3 Target { get; set; }
        protected ICard Handler { get; }
        protected float Speed { get; set; }

        //
        protected IMotionBaseCard(ICard handler)
        {
            Handler = handler;
        }

        public void Update()
        {
            if (!IsOperating)
                return;

            if (CheckFinalState())
                OnMotionEnds();
            else
                KeepMotion();
        }

        public virtual void Execute(Vector3 vector, float speed, float delay = 0, bool withZ = false)
        {
            Speed = speed;
            Target = vector;
            if (delay == 0)
                IsOperating = true;
            else
                Handler.MonoBehavior.StartCoroutine(AllowMotion(delay));
        }

        IEnumerator AllowMotion(float delay)
        {
            yield return new WaitForSeconds(delay);
            IsOperating = true;
        }

        //
        protected abstract bool CheckFinalState();
        protected virtual void OnMotionEnds() => OnFinishMotion?.Invoke();
        protected abstract void KeepMotion();

        // TODO: Cancel the Delay Coroutine.
        public virtual void StopMotion() => IsOperating = false;
    }
}