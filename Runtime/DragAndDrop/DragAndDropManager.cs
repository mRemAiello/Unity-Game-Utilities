using System;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUtils
{
    [DefaultExecutionOrder(-10000)]
    [DeclareBoxGroup("Settings")]
    [DeclareBoxGroup("Cards")]
    [DeclareBoxGroup("Debug")]
    public class DragAndDropManager : Singleton<DragAndDropManager>, ILoggable
    {
        [SerializeField, Group("Settings")] private InputActionReference _pointerPositionAction;
        [SerializeField, Group("Settings")] private InputActionReference _clickAction;
        [SerializeField, Group("Settings")] private Camera _camera;
        [SerializeField, Group("Settings")] private LayerMask _raycastMask;
        [SerializeField, Group("Settings")] private bool _hideCursor;
        [SerializeField, Group("Settings")] private int _hitsCount = 5;
        [SerializeField, Range(0.1f, 2.0f), Group("Cards")] private float _dragSpeed = 1.0f;
        [SerializeField, Range(0.0f, 10.0f), Group("Cards")] private float _height = 1.0f;
        [SerializeField, Group("Cards")] private Vector2 _cardSize;

        //
        [SerializeField, Group("Debug")] private bool _logEnabled = false;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _currentDrag;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _hoveredDraggable;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _currentDropTarget;
        [SerializeField, Group("Debug"), ReadOnly] private Transform _currentDragTransform;
        [SerializeField, Group("Debug"), ReadOnly] private RaycastHit[] _raycastHits;
        [SerializeField, Group("Debug"), ReadOnly] private RaycastHit[] _cardHits;
        [SerializeField, Group("Debug"), ReadOnly] private Ray _mouseRay;
        [SerializeField, ReadOnly, Group("Debug")] private Vector2 _mousePosition;
        [SerializeField, Group("Debug"), ReadOnly] private Vector3 _oldMouseWorldPosition;


        //
        public bool LogEnabled => _logEnabled;

        //
        private void OnEnable()
        {
            _raycastHits = new RaycastHit[_hitsCount];
            _cardHits = new RaycastHit[4];
            _hoveredDraggable = null;
            _currentDragTransform = null;

            //
            _clickAction.action.started += HandleMouseDown;
            _clickAction.action.canceled += HandleMouseUp;
            _pointerPositionAction.action.performed += HandlePointerMoved;

            _clickAction.action.Enable();
            _pointerPositionAction.action.Enable();

            //
            ResetCursor();
        }

        private void OnDisable()
        {
            _clickAction.action.started -= HandleMouseDown;
            _clickAction.action.canceled -= HandleMouseUp;
            _pointerPositionAction.action.performed -= HandlePointerMoved;

            _clickAction.action.Disable();
            _pointerPositionAction.action.Disable();
        }

        private void HandleMouseDown(InputAction.CallbackContext ctx)
        {
            if (_currentDrag != null)
                return;

            if (_hoveredDraggable == null)
                return;

            if (_hoveredDraggable is not IDraggable draggable)
            {
                this.Log($"{_hoveredDraggable} is not draggable");
                return;
            }

            //
            _currentDrag = _hoveredDraggable;
            _currentDragTransform = _currentDrag.transform;
            _oldMouseWorldPosition = MousePositionToWorldPoint();

            //
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            //
            this.Log($"Start dragging {_currentDrag}");
            draggable.Dragging = true;
            draggable.OnBeginDrag(new Vector3(_raycastHits[0].point.x, _raycastHits[0].point.y + _height, _raycastHits[0].point.z));
        }

        private void HandlePointerMoved(InputAction.CallbackContext ctx)
        {
            _mousePosition = ctx.ReadValue<Vector2>();
            this.Log($"Mouse screen position: {_mousePosition}");

            if (_currentDrag == null)
            {
                UpdateHover();
                return;
            }

            UpdateDrag();
        }

        private void HandleMouseUp(InputAction.CallbackContext ctx)
        {
            if (_currentDrag == null)
                return;

            IDroppable droppable = _currentDropTarget as IDroppable;
            if (_currentDrag is IDraggable draggable)
            {
                draggable.Dragging = false;
                draggable.OnEndDrag(_raycastHits[0].point, droppable);
            }

            _currentDrag = null;
            _currentDragTransform = null;
            _currentDropTarget = null;

            if (!_hideCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            UpdateHover();
        }

        private void UpdateHover()
        {
            IDraggable newHovered = DetectDraggable();
            MonoBehaviour newHoveredBehaviour = newHovered as MonoBehaviour;

            if (newHoveredBehaviour == _hoveredDraggable)
                return;

            if (_hoveredDraggable is IDraggable oldDraggable)
                oldDraggable.OnPointerExit(_raycastHits[0].point);

            _hoveredDraggable = newHoveredBehaviour;

            if (_hoveredDraggable is IDraggable hoveredDraggable)
            {
                hoveredDraggable.OnPointerEnter(_raycastHits[0].point);
            }
            else
            {
                ResetCursor();
            }
        }

        private void UpdateDrag()
        {
            IDroppable droppable = DetectDroppable();
            _currentDropTarget = droppable as MonoBehaviour;

            // Calculate offset
            Vector3 mouseWorldPosition = MousePositionToWorldPoint();
            Vector3 offset = (mouseWorldPosition - _oldMouseWorldPosition) * _dragSpeed;

            // Drag the card
            if (_currentDrag is IDraggable draggable)
                draggable.OnDrag(offset, droppable);

            //
            _oldMouseWorldPosition = mouseWorldPosition;
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
            if (Physics.RaycastNonAlloc(_mouseRay, _raycastHits, _camera.farClipPlane, _raycastMask) > 0)
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

                int hits = Physics.RaycastNonAlloc(ray, _raycastHits, _camera.farClipPlane, _raycastMask);
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
            _mouseRay = _camera.ScreenPointToRay(_mousePosition);

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

        private Vector3 MousePositionToWorldPoint()
        {
            Vector3 mousePosition = _mousePosition;
            if (_camera.orthographic == false)
                mousePosition.z = 10.0f;

            //
            return _camera.ScreenToWorldPoint(mousePosition);
        }

        //
        private void ResetCursor() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}