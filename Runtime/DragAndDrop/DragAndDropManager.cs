using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Drag and Drop manager.
    /// </summary>
    [DeclareBoxGroup("settings", Title = "Settings")]
    [DeclareBoxGroup("cards", Title = "Cards")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class DragAndDropManager : Singleton<DragAndDropManager>, ILoggable
    {
        [SerializeField, Group("settings")] private LayerMask _raycastMask;
        [SerializeField, Group("settings")] private bool _hideCursor;
        [SerializeField, Group("settings")] private int _hitsCount = 5;
        [SerializeField, Range(0.1f, 2.0f), Group("cards")] private float _dragSpeed = 1.0f;
        [SerializeField, Range(0.0f, 10.0f), Group("cards")] private float _height = 1.0f;
        [SerializeField, Group("cards")] private Vector2 _cardSize;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = false;
        [SerializeField, Group("debug"), ReadOnly] private IDraggable _currentDrag;
        [SerializeField, Group("debug"), ReadOnly] private IDraggable _possibleDrag;
        [SerializeField, Group("debug"), ReadOnly] private Transform _currentDragTransform;
        [SerializeField, Group("debug"), ReadOnly] private RaycastHit[] _raycastHits;
        [SerializeField, Group("debug"), ReadOnly] private RaycastHit[] _cardHits;
        [SerializeField, Group("debug"), ReadOnly] private Ray _mouseRay;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _oldMouseWorldPosition;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void OnEnable()
        {
            _raycastHits = new RaycastHit[_hitsCount];
            _cardHits = new RaycastHit[4];
            _possibleDrag = null;
            _currentDragTransform = null;

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
            if (Physics.RaycastNonAlloc(_mouseRay, _raycastHits, Camera.main.farClipPlane, _raycastMask) > 0)
            {
                // We order the impacts according to distance.
                Array.Sort(_raycastHits, (x, y) => x.distance.CompareTo(y.distance));

                // We are only interested in the first one.
                hit = _raycastHits[0].transform;
            }

            return hit;
        }

        /// <summary>Detects an IDrop object under the mouse pointer.</summary>
        /// <returns>IDrop or null.</returns>
        private IDroppable DetectDroppable()
        {
            IDroppable droppable = null;

            // The four corners of the card.
            Vector3 cardPosition = _currentDragTransform.position;
            Vector2 halfCardSize = _cardSize * 0.5f;
            Vector3[] cardConner =
            {
                new(cardPosition.x + halfCardSize.x, cardPosition.y, cardPosition.z - halfCardSize.y),
                new(cardPosition.x + halfCardSize.x, cardPosition.y, cardPosition.z + halfCardSize.y),
                new(cardPosition.x - halfCardSize.x, cardPosition.y, cardPosition.z - halfCardSize.y),
                new(cardPosition.x - halfCardSize.x, cardPosition.y, cardPosition.z + halfCardSize.y)
            };
            int cardHitIndex = 0;
            Array.Clear(_cardHits, 0, _cardHits.Length);

            // We launch the four rays.
            for (int i = 0; i < cardConner.Length; ++i)
            {
                Ray ray = new(cardConner[i], Vector3.down);

                int hits = Physics.RaycastNonAlloc(ray, _raycastHits, Camera.main.farClipPlane, _raycastMask);
                if (hits > 0)
                {
                    // We order the impacts by distance from the origin of the ray.
                    Array.Sort(_raycastHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

                    // We are only interested in the closest one.
                    _cardHits[cardHitIndex++] = _raycastHits[0];
                }
            }

            if (cardHitIndex > 0)
            {
                // We are looking for the nearest possible IDrop.
                Array.Sort(_cardHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

                if (_cardHits[0].transform != null)
                    droppable = _cardHits[0].transform.GetComponent<IDroppable>();
            }

            return droppable;
        }

        /// <summary>Detects an IDrag object under the mouse pointer.</summary>
        /// <returns>IDrag or null.</returns>
        public IDraggable DetectDraggable()
        {
            _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            Transform hit = MouseRaycast();
            IDraggable draggable = null;
            if (hit != null)
            {
                draggable = hit.GetComponent<IDraggable>();
                if (draggable is { IsDraggable: true })
                    _currentDragTransform = hit;
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
                if (Input.GetMouseButtonDown(0) == true)
                {
                    // Is there an IDrag object under the mouse pointer?
                    if (draggable != null)
                    {
                        // We already have an object to start the drag operation!
                        _currentDrag = draggable;
                        //currentDragTransform = hit;
                        _oldMouseWorldPosition = MousePositionToWorldPoint();

                        // Hide the mouse icon.
                        Cursor.visible = false;
                        // And we lock the movements to the window frame,
                        // so we can't move objects out of the camera's view.          
                        Cursor.lockState = CursorLockMode.Confined;

                        // The drag operation begins.
                        _currentDrag.Dragging = true;
                        _currentDrag.OnBeginDrag(new Vector3(_raycastHits[0].point.x, _raycastHits[0].point.y + _height, _raycastHits[0].point.z));
                    }
                }
                else
                {
                    // Left mouse button not pressed?

                    // We pass over a new IDrag?
                    if (draggable != null && _possibleDrag == null)
                    {
                        // We execute its OnPointerEnter.
                        _possibleDrag = draggable;
                        _possibleDrag.OnPointerEnter(_raycastHits[0].point);
                    }

                    // We are leaving an IDrag?
                    if (draggable == null && _possibleDrag != null)
                    {
                        // We execute its OnPointerExit.
                        _possibleDrag.OnPointerExit(_raycastHits[0].point);
                        _possibleDrag = null;

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
                    Vector3 offset = (mouseWorldPosition - _oldMouseWorldPosition) * _dragSpeed;

                    // OnDrag is executed.
                    _currentDrag.OnDrag(offset, droppable);

                    _oldMouseWorldPosition = mouseWorldPosition;
                }
                else if (Input.GetMouseButtonUp(0) == true)
                {
                    // The left mouse button is released and
                    // the drag operation is finished.
                    _currentDrag.Dragging = false;
                    _currentDrag.OnEndDrag(_raycastHits[0].point, droppable);
                    _currentDrag = null;
                    _currentDragTransform = null;

                    // We return the mouse icon to its normal state.
                    if (!_hideCursor)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
            }
        }

        private void ResetCursor() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}