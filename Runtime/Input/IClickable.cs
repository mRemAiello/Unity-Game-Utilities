using UnityEngine;

namespace GameUtils
{
    // Defines a clickable contract for raycasted objects.
    public interface IClickable
    {
        // Receives the world hit point from the click raycast.
        void OnClick(Vector3 hitPoint);
    }
}
