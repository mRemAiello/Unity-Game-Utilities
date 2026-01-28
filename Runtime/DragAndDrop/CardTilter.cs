using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("pitch", Title = "Pitch")]
    [DeclareBoxGroup("roll", Title = "Roll")]
    [DeclareBoxGroup("misc", Title = "Misc")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class CardTilter : MonoBehaviour
    {
        [SerializeField, Group("pitch")] private float pitchForce = 10.0f;
        [SerializeField, Group("pitch")] private float pitchMinAngle = -25.0f;
        [SerializeField, Group("pitch")] private float pitchMaxAngle = 25.0f;
        [SerializeField, Group("roll")] private float rollForce = 10.0f;
        [SerializeField, Group("roll")] private float rollMinAngle = -25.0f;
        [SerializeField, Group("roll")] private float rollMaxAngle = 25.0f;
        [SerializeField, Group("misc")] private float restTime = 1.0f;

        // Pitch angle and velocity.
        [SerializeField, Group("debug"), ReadOnly] private float _pitchAngle;
        [SerializeField, Group("debug"), ReadOnly] private float _pitchVelocity;

        // Roll angle and velocity.
        [SerializeField, Group("debug"), ReadOnly] private float _rollAngle;
        [SerializeField, Group("debug"), ReadOnly] private float _rollVelocity;

        // To calculate the velocity vector.
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _oldPosition;

        // The original rotation
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _originalAngles;

        //
        private void Awake()
        {
            _oldPosition = transform.position;
            _originalAngles = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            // Calculate offset.
            Vector3 currentPosition = transform.position;
            Vector3 offset = currentPosition - _oldPosition;

            // Limit the angle ranges.
            if (offset.sqrMagnitude > Mathf.Epsilon)
            {
                _pitchAngle = Mathf.Clamp(_pitchAngle + offset.z * pitchForce, pitchMinAngle, pitchMaxAngle);
                _rollAngle = Mathf.Clamp(_rollAngle + offset.x * rollForce, rollMinAngle, rollMaxAngle);
            }

            // The angles have 0 with time.
            _pitchAngle = Mathf.SmoothDamp(_pitchAngle, 0.0f, ref _pitchVelocity, restTime * Time.deltaTime * 10.0f);
            _rollAngle = Mathf.SmoothDamp(_rollAngle, 0.0f, ref _rollVelocity, restTime * Time.deltaTime * 10.0f);

            // Update the card rotation.
            transform.rotation = Quaternion.Euler(_originalAngles.x + _pitchAngle, _originalAngles.y, _originalAngles.z - _rollAngle);

            //
            _oldPosition = currentPosition;
        }
    }
}