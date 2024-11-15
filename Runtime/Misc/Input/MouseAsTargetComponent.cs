using UnityEngine;

namespace GameUtils
{
    public class MouseAsTargetComponent : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;

        [Space]
        [SerializeField] private bool _posLockX;
        [SerializeField] private bool _posLockY;
        [SerializeField] private bool _posLockZ;

        void Start()
        {
            if (_targetCamera == null)
            {
                _targetCamera = Camera.main;
            }
        }

        void Update()
        {
            if (!InputManager.InstanceExists)
            {
                Debug.Log("Please create a InputManager instance.");
                return;
            }

            //
            Vector3 mousePosition =  InputManager.Instance.CurrentPositionVector3;
            Vector3 target;

            //
            bool orthographic = _targetCamera.orthographic;
            if (orthographic)
            {
                //Orthographic camera, simply find world position and use axis lock mask
                target = _targetCamera.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                float distanceToCamera = VectorWithNegatedAxisMask(Vector3.zero, transform.position - _targetCamera.transform.position).magnitude;
                target = _targetCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distanceToCamera));
            }

            //
            transform.position = VectorWithAxisMask(transform.position, target);
        }

        private Vector3 VectorWithAxisMask(Vector3 defaultVector, Vector3 target)
        {
            Vector3 res = defaultVector;
            if (!_posLockX)
            {
                res.x = target.x;
            }
            if (!_posLockY)
            {
                res.y = target.y;
            }
            if (!_posLockZ)
            {
                res.z = target.z;
            }

            return res;
        }

        private Vector3 VectorWithNegatedAxisMask(Vector3 defaultVector, Vector3 target)
        {
            Vector3 res = defaultVector;
            if (_posLockX)
            {
                res.x = target.x;
            }
            if (_posLockY)
            {
                res.y = target.y;
            }
            if (_posLockZ)
            {
                res.z = target.z;
            }

            return res;
        }
    }
}