using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUtils
{
    [DefaultExecutionOrder(-10000)]
    [DeclareBoxGroup("Click")]
    public class ClickInputManager : Singleton<ClickInputManager>
    {
        [SerializeField, Group("Click")] private Camera _mainCamera;
        [SerializeField, Group("Click")] private InputActionReference _pointerPositionAction;
        [SerializeField, Group("Click")] private InputActionReference _clickAction;
        [SerializeField, Group("Click")] private float _raycastDistance = 100f;

        private IClickable _hoveredClickable;

        // Enables the referenced input actions when the component becomes active.
        private void OnEnable()
        {
            _pointerPositionAction.action.Enable();
            _clickAction.action.Enable();
        }

        // Disables the referenced input actions when the component becomes inactive.
        private void OnDisable()
        {
            ClearHoveredClickable();

            //
            _pointerPositionAction.action.Disable();
            _clickAction.action.Disable();
        }

        // Checks hover/click input and performs a raycast from the main camera.
        private void Update()
        {
            Vector2 pointerPosition = _pointerPositionAction.action.ReadValue<Vector2>();
            Ray ray = _mainCamera.ScreenPointToRay(pointerPosition);

            IClickable hitClickable = null;
            Vector3 hitPoint = default;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance) && hitInfo.collider.TryGetComponent(out IClickable clickable))
            {
                hitClickable = clickable;
                hitPoint = hitInfo.point;
            }

            if (!ReferenceEquals(_hoveredClickable, hitClickable))
            {
                _hoveredClickable?.OnMouseExit();
                _hoveredClickable = hitClickable;
                _hoveredClickable?.OnMouseEnter(hitPoint);
            }

            if (!_clickAction.action.WasPerformedThisFrame())
            {
                return;
            }

            if (hitClickable == null)
            {
                return;
            }

            hitClickable.OnMouseClick(hitPoint);
        }

        private void ClearHoveredClickable()
        {
            _hoveredClickable?.OnMouseExit();
            _hoveredClickable = null;
        }
    }
}