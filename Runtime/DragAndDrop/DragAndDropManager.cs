using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Drag and Drop manager.
    /// </summary>
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("references", Title = "References")]
    public class DragAndDropManager : Singleton<DragAndDropManager>, ILoggable
    {
        [SerializeField] private LayerMask _raycastMask;
        [SerializeField, Range(0.1f, 2.0f)] private float dragSpeed = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float height = 1.0f;
        [SerializeField] private Vector2 _cardSize;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = false;
        [SerializeField, Group("debug")] private int _hitsCount = 5;
        [SerializeField, Group("debug")] private IDraggable _currentDrag;
        [SerializeField, Group("debug")] private IDraggable possibleDrag;
        [SerializeField, Group("debug")] private Transform currentDragTransform;
        [SerializeField, Group("debug")] private RaycastHit[] raycastHits;
        [SerializeField, Group("debug")] private RaycastHit[] cardHits;
        [SerializeField, Group("debug")] private Ray mouseRay;
        [SerializeField, Group("debug")] private Vector3 oldMouseWorldPosition;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void OnEnable()
        {
            raycastHits = new RaycastHit[_hitsCount];
            cardHits = new RaycastHit[4];
            possibleDrag = null;
            currentDragTransform = null;

            //
            ResetCursor();
        }

        private Vector3 MousePositionToWorldPoint()
        {
            Vector3 mousePosition = Input.mousePosition;
            if (Camera.main.orthographic == false)
                mousePosition.z = 10.0f;

            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// Returns the Transfrom of the object closest to the origin
        /// of the ray.
        /// </summary>
        /// <returns>Transform or null if there is no impact.</returns>
        private Transform MouseRaycast()
        {
            Transform hit = null;

            // Fire the ray!
            if (Physics.RaycastNonAlloc(mouseRay, raycastHits, Camera.main.farClipPlane, _raycastMask) > 0)
            {
                // We order the impacts according to distance.
                Array.Sort(raycastHits, (x, y) => x.distance.CompareTo(y.distance));

                // We are only interested in the first one.
                hit = raycastHits[0].transform;
            }

            return hit;
        }

        /// <summary>Detects an IDrop object under the mouse pointer.</summary>
        /// <returns>IDrop or null.</returns>
        private IDroppable DetectDroppable()
        {
            IDroppable droppable = null;

            // The four corners of the card.
            Vector3 cardPosition = currentDragTransform.position;
            Vector2 halfCardSize = _cardSize * 0.5f;
            Vector3[] cardConner =
            {
                new(cardPosition.x + halfCardSize.x, cardPosition.y, cardPosition.z - halfCardSize.y),
                new(cardPosition.x + halfCardSize.x, cardPosition.y, cardPosition.z + halfCardSize.y),
                new(cardPosition.x - halfCardSize.x, cardPosition.y, cardPosition.z - halfCardSize.y),
                new(cardPosition.x - halfCardSize.x, cardPosition.y, cardPosition.z + halfCardSize.y)
            };
            int cardHitIndex = 0;
            Array.Clear(cardHits, 0, cardHits.Length);

            // We launch the four rays.
            for (int i = 0; i < cardConner.Length; ++i)
            {
                Ray ray = new(cardConner[i], Vector3.down);

                int hits = Physics.RaycastNonAlloc(ray, raycastHits, Camera.main.farClipPlane, _raycastMask);
                if (hits > 0)
                {
                    // We order the impacts by distance from the origin of the ray.
                    Array.Sort(raycastHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

                    // We are only interested in the closest one.
                    cardHits[cardHitIndex++] = raycastHits[0];
                }
            }

            if (cardHitIndex > 0)
            {
                // We are looking for the nearest possible IDrop.
                Array.Sort(cardHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

                if (cardHits[0].transform != null)
                    droppable = cardHits[0].transform.GetComponent<IDroppable>();
            }

            return droppable;
        }

        /// <summary>Detects an IDrag object under the mouse pointer.</summary>
        /// <returns>IDrag or null.</returns>
        public IDraggable DetectDraggable()
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            Transform hit = MouseRaycast();
            IDraggable draggable = null;
            if (hit != null)
            {
                draggable = hit.GetComponent<IDraggable>();
                if (draggable is { IsDraggable: true })
                    currentDragTransform = hit;
                else
                    draggable = null;
            }

            return draggable;
        }

        private void Update()
        {
            if (_currentDrag == null)
            {
                IDraggable draggable = DetectDraggable();

                // Left mouse button pressed?
                if (Input.GetMouseButtonDown(0) == true)
                {
                    // Is there an IDrag object under the mouse pointer?
                    if (draggable != null)
                    {
                        // We already have an object to start the drag operation!
                        _currentDrag = draggable;
                        //currentDragTransform = hit;
                        oldMouseWorldPosition = MousePositionToWorldPoint();

                        // Hide the mouse icon.
                        Cursor.visible = false;
                        // And we lock the movements to the window frame,
                        // so we can't move objects out of the camera's view.          
                        Cursor.lockState = CursorLockMode.Confined;

                        // The drag operation begins.
                        _currentDrag.Dragging = true;
                        _currentDrag.OnBeginDrag(new Vector3(raycastHits[0].point.x, raycastHits[0].point.y + height, raycastHits[0].point.z));
                    }
                }
                else
                {
                    // Left mouse button not pressed?

                    // We pass over a new IDrag?
                    if (draggable != null && possibleDrag == null)
                    {
                        // We execute its OnPointerEnter.
                        possibleDrag = draggable;
                        possibleDrag.OnPointerEnter(raycastHits[0].point);
                    }

                    // We are leaving an IDrag?
                    if (draggable == null && possibleDrag != null)
                    {
                        // We execute its OnPointerExit.
                        possibleDrag.OnPointerExit(raycastHits[0].point);
                        possibleDrag = null;

                        ResetCursor();
                    }
                }
            }
            else
            {
                IDroppable droppable = DetectDroppable();

                // Is the left mouse button held down?
                if (Input.GetMouseButton(0) == true)
                {
                    // Calculate the offset of the mouse with respect to its previous position.
                    Vector3 mouseWorldPosition = MousePositionToWorldPoint();
                    Vector3 offset = (mouseWorldPosition - oldMouseWorldPosition) * dragSpeed;

                    // OnDrag is executed.
                    _currentDrag.OnDrag(offset, droppable);

                    oldMouseWorldPosition = mouseWorldPosition;
                }
                else if (Input.GetMouseButtonUp(0) == true)
                {
                    // The left mouse button is released and
                    // the drag operation is finished.
                    _currentDrag.Dragging = false;
                    _currentDrag.OnEndDrag(raycastHits[0].point, droppable);
                    _currentDrag = null;
                    currentDragTransform = null;

                    // We return the mouse icon to its normal state.
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        private void ResetCursor() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}