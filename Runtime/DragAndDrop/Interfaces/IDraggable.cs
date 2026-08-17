using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Draggable object.
    /// </summary>
    public interface IDraggable
    {
        /// <summary> Can it be dragged? </summary>
        public bool IsDraggable { get; }

        /// <summary> A Drag operation is currently underway. </summary>
        public bool Dragging { get; set; }

        /// <summary> Mouse enters the object. </summary>
        /// <param name="mousePosition">Mouse position.</param>
        public void OnPointerEnter(Vector3 mousePosition);

        /// <summary> Mouse exits object. </summary>
        /// <param name="mousePosition">Mouse position.</param>
        public void OnPointerExit(Vector3 mousePosition);

        /// <summary> Drag begins. </summary>
        /// <param name="position">Mouse position.</param>
        /// <param name="height">Height to raise the object while dragging.</param>
        public void OnBeginDrag(Vector3 position, float height);

        /// <summary> A drag is in progress. </summary>
        /// <param name="position"> Current mouse world position on the drag plane. </param>
        /// <param name="height">Height to maintain while dragging.</param>
        /// <param name="droppable">
        /// Object on which a drop may be made, or null. </param>
        public void OnDrag(Vector3 position, float height, IDroppable droppable);

        /// <summary> The drag operation is completed. </summary>
        /// <param name="position">Mouse position.</param>
        /// <param name="droppable">
        /// Object on which a drop may be made, or null. </param>
        public void OnEndDrag(Vector3 position, IDroppable droppable);
    }
}