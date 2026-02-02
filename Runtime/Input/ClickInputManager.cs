using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUtils
{
    // Manages click raycasts using the new Input System.
    public class ClickInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pointerPositionAction;
        [SerializeField] private InputActionReference _clickAction;
        [SerializeField] private float _raycastDistance = 100f;

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

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            Vector2 pointerPosition = _pointerPositionAction.action.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance))
            {
                return;
            }

            IClickable clickable = hitInfo.collider.GetComponent<IClickable>();
            if (clickable == null)
            {
                return;
            }

            clickable.OnClick(hitInfo.point);
        }
    }
}
