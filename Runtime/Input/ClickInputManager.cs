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

        // Enables the referenced input actions when the component becomes active.
        private void OnEnable()
        {
            if (_pointerPositionAction != null)
            {
                _pointerPositionAction.action.Enable();
            }

            if (_clickAction != null)
            {
                _clickAction.action.Enable();
            }
        }

        // Disables the referenced input actions when the component becomes inactive.
        private void OnDisable()
        {
            if (_pointerPositionAction != null)
            {
                _pointerPositionAction.action.Disable();
            }

            if (_clickAction != null)
            {
                _clickAction.action.Disable();
            }
        }

        // Checks for click input and performs a raycast from the main camera.
        private void Update()
        {
            if (_pointerPositionAction == null || _clickAction == null)
            {
                return;
            }

            if (!_clickAction.action.WasPerformedThisFrame())
            {
                return;
            }

            Vector2 pointerPosition = _pointerPositionAction.action.ReadValue<Vector2>();
            Ray ray = _mainCamera.ScreenPointToRay(pointerPosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance))
            {
                return;
            }

            if (!hitInfo.collider.TryGetComponent<IClickable>(out var clickable))
            {
                return;
            }

            clickable.OnClick(hitInfo.point);
        }
    }
}
