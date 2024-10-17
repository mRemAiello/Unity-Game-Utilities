using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;
using static UnityEngine.InputSystem.InputAction;

namespace GameUtils
{
    public class InputManager : Singleton<InputManager>
    {
        [Tab("References")]
        [SerializeField] private InputActionReference _pointAction;
        [SerializeField] private InputActionReference _clickAction;

        [Tab("Events")]
        [SerializeField] private SelectableEventAsset _onItemSelected;

        [Space]
        [SerializeField] private VoidEventAsset _onMouseUp;
        [SerializeField] private VoidEventAsset _onMouseDown;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private Vector2 _currentPosition;

        //
        private ISelectable _previousSelectable = null;
        private ISelectable _currentSelectable = null;

        //
        public Vector2 CurrentPositionVector2 => _currentPosition;
        public Vector3 CurrentPositionVector3 => new(_currentPosition.x, _currentPosition.y, 0);

        protected override void OnPostAwake()
        {
            //
            _previousSelectable = null;
            _currentSelectable = null;

            //
            _pointAction.action.performed += OnCursorChangePosition;
            _clickAction.action.performed += OnClick;
        }

        private void OnCursorChangePosition(CallbackContext context)
        {
            _currentPosition = context.ReadValue<Vector2>();
            
            //
            var raycastHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(_currentPosition));

            //
            ISelectable newSelectable = null;
            float closestDistance = Mathf.Infinity;
            foreach (RaycastHit hit in raycastHits)
            {
                if (!hit.transform.gameObject.TryGetComponent<ISelectable>(out var selectable))
                    continue;

                Vector3 itemPosition = hit.transform.position;
                var mousePosition = new Vector3(_currentPosition.x, _currentPosition.y, Camera.main.nearClipPlane);
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                float distance = Vector3.Distance(worldMousePosition, itemPosition);

                //
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    newSelectable = selectable;
                }
            }

            // 
            if (_currentSelectable != newSelectable)
            {
                _currentSelectable?.Deselect();
                _currentSelectable = newSelectable;
                _currentSelectable?.Select();
                _onItemSelected?.Invoke(_currentSelectable);
            }
        }

        private void OnClick(CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (value == 1)
            {
                _onMouseDown?.Invoke();
            }
            else
            {
                _onMouseUp?.Invoke();
            }
        }
    }
}