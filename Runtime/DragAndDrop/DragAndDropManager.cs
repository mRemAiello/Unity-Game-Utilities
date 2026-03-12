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
        [SerializeField, Group("Settings")] private LayerMask _dragMask;
        [SerializeField, Group("Settings")] private LayerMask _dropMask;
        [SerializeField, Group("Settings")] private bool _hideCursor;
        [SerializeField, Group("Settings")] private int _hitsCount = 5;
        [SerializeField, Range(0.1f, 2.0f), Group("Cards")] private float _dragSpeed = 1.0f;
        [SerializeField, Range(0.0f, 10.0f), Group("Cards")] private float _height = 1.0f;

        //
        [SerializeField, Group("Debug")] private bool _logEnabled = false;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _currentDrag;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _hoveredDraggable;
        [SerializeField, Group("Debug"), ReadOnly] private MonoBehaviour _currentDropTarget;
        [SerializeField, Group("Debug"), ReadOnly] private Transform _currentDragTransform;
        [SerializeField, ReadOnly, Group("Debug")] private Vector2 _mousePosition;
        [SerializeField, Group("Debug"), ReadOnly] private Vector3 _oldMouseWorldPosition;
        private RaycastHit[] _raycastHits;
        private Ray _mouseRay;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void OnEnable()
        {
            _raycastHits = new RaycastHit[_hitsCount];
            _hoveredDraggable = null;
            _currentDragTransform = null;

            // Click events
            _clickAction.action.started += HandleMouseDown;
            _clickAction.action.canceled += HandleMouseUp;
            _clickAction.action.Enable();

            // Pointer events
            _pointerPositionAction.action.performed += HandlePointerMoved;
            _pointerPositionAction.action.Enable();

            //
            ResetCursor();
        }

        private void OnDisable()
        {
            // Click events
            _clickAction.action.started -= HandleMouseDown;
            _clickAction.action.canceled -= HandleMouseUp;
            _clickAction.action.Disable();

            // Pointer events
            _pointerPositionAction.action.performed -= HandlePointerMoved;
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

            //
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

            //
            IDroppable droppable = _currentDropTarget as IDroppable;
            if (_currentDrag is IDraggable draggable)
            {
                this.Log($"Stop dragging {_currentDrag}, drop on {_currentDropTarget}");
                draggable.Dragging = false;
                draggable.OnEndDrag(_raycastHits[0].point, droppable);
            }

            //
            _currentDrag = null;
            _currentDragTransform = null;
            _currentDropTarget = null;

            //
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

            //
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
            {
                draggable.OnDrag(offset, droppable);
            }

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
            if (Physics.RaycastNonAlloc(_mouseRay, _raycastHits, _camera.farClipPlane, _dragMask) > 0)
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
            if (_currentDragTransform == null || _camera == null)
                return null;

            Vector3 direction = _camera.transform.forward.normalized;
            Vector3 origin = _currentDragTransform.position - direction * 0.05f;

            Ray ray = new Ray(origin, direction);
            int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, _camera.farClipPlane, _dropMask);

            RaycastHit closestHit = default;
            bool found = false;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = _raycastHits[i];

                if (hit.transform == null)
                    continue;

                if (hit.transform.IsChildOf(_currentDragTransform))
                    continue;

                IDroppable droppable = hit.collider.GetComponentInParent<IDroppable>();
                if (droppable == null)
                    continue;

                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                    found = true;
                }
            }

            if (!found)
                return null;

            return closestHit.collider.GetComponentInParent<IDroppable>();
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

        private void OnDrawGizmos()
        {
            if (_currentDragTransform == null || _camera == null)
                return;

            Vector3 direction = _camera.transform.forward.normalized;
            Vector3 origin = _currentDragTransform.position - direction * 0.05f;

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(origin, direction * 5f);
        }

        //
        private void ResetCursor() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}