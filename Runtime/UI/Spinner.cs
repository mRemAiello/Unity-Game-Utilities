using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class Spinner : UIAnimation
    {
        [Tab("Settings")]
        [SerializeField] private bool _rotateLeft = true;
        [SerializeField] private float _rotationSpeed = 10f;
        
        //
        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _spinEnabled = false;

        //
        public bool RotateLeft => _rotateLeft;
        public float RotationSpeed => _rotationSpeed;
            
        private void Update()
        {
            if (_spinEnabled)
            {
                //
                float rotationAmount = (_rotateLeft ? -1f : 1f) * _rotationSpeed * Time.deltaTime;

                //
                transform.Rotate(0, rotationAmount, 0);
            }
        }

        protected override void OnShow(bool showAnimation = true)
        {
            _spinEnabled = true;
        }

        protected override void OnHide(bool showAnimation = true)
        {
            _spinEnabled = false;
        }

        //
        public void SetRotationSpeed(float newSpeed) => _rotationSpeed = newSpeed;
        public void SetRotationDirection(bool nowLeft) => _rotateLeft = nowLeft;
    }
}