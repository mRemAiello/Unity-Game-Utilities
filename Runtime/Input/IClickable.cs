using UnityEngine;

namespace GameUtils
{
    public interface IClickable
    {
        int Priority { get; }

        //
        /// <summary> Mouse enters the object. </summary>
        /// <param name="mousePosition">Mouse position.</param>
        public void OnPointerEnter(Vector3 mousePosition);

        /// <summary> Mouse exits object. </summary>
        /// <param name="mousePosition">Mouse position.</param>
        public void OnPointerExit(Vector3 mousePosition);

        /// <summary> Mouse click on the object. </summary>
        /// <param name="hitPoint">Mouse hit point.</param>
        void OnPointerClick(Vector3 hitPoint);
    }
}