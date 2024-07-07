using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class Spinner : UIAnimation
    {
        [Tab("Animation")]
        [SerializeField] private bool _rotateLeft = true;
        [Space]
        [SerializeField] private float _rotationSpeedX = 0f;
        [SerializeField] private float _rotationSpeedY = 10f;
        [SerializeField] private float _rotationSpeedZ = 0f;
        
        //
        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _spinEnabled = false;

        //
        public bool RotateLeft => _rotateLeft;
        public float RotationSpeed => _rotationSpeedY;
       
        private void Update()
        {
            if (_spinEnabled)
            {
                //
                float rotationAmountX = (_rotateLeft ? -1f : 1f) * _rotationSpeedX * Time.deltaTime;
                float rotationAmountY = (_rotateLeft ? -1f : 1f) * _rotationSpeedY * Time.deltaTime;
                float rotationAmountZ = (_rotateLeft ? -1f : 1f) * _rotationSpeedZ * Time.deltaTime;

                //
                transform.Rotate(rotationAmountX, rotationAmountY, rotationAmountZ);
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
        public void SetRotationSpeed(float newSpeed) => _rotationSpeedY = newSpeed;
        public void SetRotationDirection(bool nowLeft) => _rotateLeft = nowLeft;
    }
}