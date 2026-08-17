using System;
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
        private RaycastHit[] _raycastHits = new RaycastHit[16];

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

            GetTopPriorityClickable(ray, out IClickable hitClickable, out Vector3 hitPoint);

            if (!ReferenceEquals(_hoveredClickable, hitClickable))
            {
                _hoveredClickable?.OnPointerExit(hitPoint);
                _hoveredClickable = hitClickable;
                _hoveredClickable?.OnPointerEnter(hitPoint);
            }

            if (!_clickAction.action.WasPerformedThisFrame())
            {
                return;
            }

            if (hitClickable == null)
            {
                return;
            }

            hitClickable.OnPointerClick(hitPoint);
        }

        private void ClearHoveredClickable()
        {
            Vector2 pointerPosition = _pointerPositionAction.action.ReadValue<Vector2>();
            _hoveredClickable?.OnPointerExit(pointerPosition);
            _hoveredClickable = null;
        }

        private void GetTopPriorityClickable(Ray ray, out IClickable clickable, out Vector3 hitPoint)
        {
            clickable = null;
            hitPoint = default;

            int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, _raycastDistance);
            while (hitCount == _raycastHits.Length)
            {
                Array.Resize(ref _raycastHits, _raycastHits.Length * 2);
                hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, _raycastDistance);
            }

            int highestPriority = int.MinValue;
            float nearestDistance = float.PositiveInfinity;

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit raycastHit = _raycastHits[i];
                if (!raycastHit.collider.TryGetComponent(out IClickable candidate))
                {
                    continue;
                }

                bool isHigherPriority = candidate.Priority > highestPriority;
                bool isSamePriorityButCloser = candidate.Priority == highestPriority && raycastHit.distance < nearestDistance;
                if (!isHigherPriority && !isSamePriorityButCloser)
                {
                    continue;
                }

                highestPriority = candidate.Priority;
                nearestDistance = raycastHit.distance;
                clickable = candidate;
                hitPoint = raycastHit.point;
            }
        }
    }
}