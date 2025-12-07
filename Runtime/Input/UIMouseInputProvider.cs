using System;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtils
{
    [RequireComponent(typeof(Collider))]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class UIMouseInputProvider : MonoBehaviour, IMouseInput
    {
        [SerializeField, ReadOnly, Group("debug")] private Vector3 _oldDragPosition;

        //
        public DragDirection DragDirection => GetDragDirection();
        public Vector2 MousePosition => Input.mousePosition;

        void Awake()
        {
            // Currently using PhysicsRaycaster, but can be also considered PhysicsRaycaster2D.
            if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
                throw new Exception(GetType() + " needs an " + typeof(PhysicsRaycaster) + " on the MainCamera");
        }

        DragDirection GetDragDirection()
        {
            var currentPosition = Input.mousePosition;
            var normalized = (currentPosition - _oldDragPosition).normalized;
            _oldDragPosition = currentPosition;

            if (normalized.x > 0)
                return DragDirection.Right;

            if (normalized.x < 0)
                return DragDirection.Left;

            if (normalized.y > 0)
                return DragDirection.Top;

            return normalized.y < 0 ? DragDirection.Down : DragDirection.None;
        }

        Action<PointerEventData> IMouseInput.OnPointerDown { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnPointerUp { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnPointerClick { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnBeginDrag { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnDrag { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnEndDrag { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnDrop { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnPointerEnter { get; set; } = eventData => { };
        Action<PointerEventData> IMouseInput.OnPointerExit { get; set; } = eventData => { };

        //
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) =>
            ((IMouseInput)this).OnBeginDrag.Invoke(eventData);

        void IDragHandler.OnDrag(PointerEventData eventData) => ((IMouseInput)this).OnDrag.Invoke(eventData);

        void IDropHandler.OnDrop(PointerEventData eventData) => ((IMouseInput)this).OnDrop.Invoke(eventData);

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => ((IMouseInput)this).OnEndDrag.Invoke(eventData);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerClick.Invoke(eventData);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerDown.Invoke(eventData);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerUp.Invoke(eventData);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerEnter.Invoke(eventData);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerExit.Invoke(eventData);
    }
}
