using UnityEngine;

namespace GameUtils
{
    public interface ICardComponents
    {
        Camera MainCamera { get; }
        SpriteRenderer[] Renderers { get; }
        SpriteRenderer Renderer { get; }
        Collider Collider { get; }
        Rigidbody Rigidbody { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehavior { get; }
        GameObject GameObject { get; }
        Transform Transform { get; }
    }
}