using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtils
{
    public interface IMouseInput : IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler,
                                        IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // Clicks
        new Action<PointerEventData> OnPointerClick { get; set; }
        new Action<PointerEventData> OnPointerDown { get; set; }
        new Action<PointerEventData> OnPointerUp { get; set; }

        // Drag
        new Action<PointerEventData> OnBeginDrag { get; set; }
        new Action<PointerEventData> OnDrag { get; set; }
        new Action<PointerEventData> OnEndDrag { get; set; }
        new Action<PointerEventData> OnDrop { get; set; }

        // Enter
        new Action<PointerEventData> OnPointerEnter { get; set; }
        new Action<PointerEventData> OnPointerExit { get; set; }

        Vector2 MousePosition { get; }
        DragDirection DragDirection { get; }
    }
}