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
        /// <param name="position">Mouse position.</param>
        public void OnPointerEnter(Vector3 position);

        /// <summary> Mouse exits object. </summary>
        /// <param name="position">Mouse position.</param>
        public void OnPointerExit(Vector3 position);

        /// <summary> Drag begins. </summary>
        /// <param name="position">Mouse position.</param>
        public void OnBeginDrag(Vector3 position);

        /// <summary> A drag is in progress. </summary>
        /// <param name="deltaPosition"> Mouse offset position. </param>
        /// <param name="droppable">
        /// Object on which a drop may be made, or null. </param>
        public void OnDrag(Vector3 deltaPosition, IDroppable droppable);

        /// <summary> The drag operation is completed. </summary>
        /// <param name="position">Mouse position.</param>
        /// <param name="droppable">
        /// Object on which a drop may be made, or null. </param>
        public void OnEndDrag(Vector3 position, IDroppable droppable);
    }
}