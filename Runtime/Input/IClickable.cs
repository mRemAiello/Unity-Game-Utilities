using UnityEngine;

namespace GameUtils
{
    public interface IClickable
    {
        int Priority { get; }

        //
        void OnEnter(Vector3 mousePosition);
        void OnExit();
        void OnClick(Vector3 hitPoint);
    }
}