namespace GameUtils
{
    /// <summary>
    /// Accept draggable objects.
    /// </summary>
    public interface IDroppable
    {
        /// <summary> Is it droppable? </summary>
        public bool IsDroppable { get; }

        /// <summary> Accept an IDrag? </summary>
        /// <param name="drag">Object IDrag.</param>
        /// <returns>Accept or not the object.</returns>
        public bool AcceptDrop(IDraggable drag);

        /// <summary> Performs the drop operation of an IDrag object. </summary>
        /// <param name="drag">Object IDrag.</param>
        public void OnDrop(IDraggable drag);
    }
}