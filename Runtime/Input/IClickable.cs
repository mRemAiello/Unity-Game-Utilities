using UnityEngine;

namespace GameUtils
{
    public interface IClickable
    {
        void OnMouseEnter(Vector3 mousePosition);
        void OnMouseExit();
        void OnMouseClick(Vector3 hitPoint);
    }
}