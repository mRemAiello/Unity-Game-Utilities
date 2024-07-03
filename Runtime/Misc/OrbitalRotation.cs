using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class OrbitalRotation : MonoBehaviour
    {
        [Tab("References")]
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private  GameObject[] _spheres;
        
        [Tab("Animation")]
        [SerializeField] private float _rotationSpeed = 50f;
        
        [Space]
        [SerializeField] private float _minDistance = 1f;
        [SerializeField] private float _maxDistance = 3f;
        [SerializeField] private float _oscillationSpeed = 1f;

        // private
        private float[] _initialAngles;
        private float[] _individualDistances;
        private float _oscillationTimer = 0f;

        private void Start()
        {
            //
            _initialAngles = new float[_spheres.Length];
            _individualDistances = new float[_spheres.Length];

            // 
            for (int i = 0; i < _spheres.Length; i++)
            {
                float angle = i * Mathf.PI * 2f / _spheres.Length;
                _initialAngles[i] = angle;
                _individualDistances[i] = Random.Range(_minDistance, _maxDistance);
                Vector3 newPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _individualDistances[i];
                _spheres[i].transform.position = _centerPoint.position + newPos;
            }
        }

        private void Update()
        {
            _oscillationTimer += Time.deltaTime * _oscillationSpeed;

            //
            for (int i = 0; i < _spheres.Length; i++)
            {
                //
                float currentDistance = Mathf.Lerp(_minDistance, _maxDistance, (Mathf.Sin(_oscillationTimer + i) + 1) / 2);
                float angle = _initialAngles[i] + _rotationSpeed * Time.deltaTime;

                //
                Vector3 newPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * currentDistance;
                _spheres[i].transform.position = _centerPoint.position + newPos;

                //
                _initialAngles[i] = angle;
            }
        }

        //
        public void SetRotationSpeed(float rotationSpeed) => _rotationSpeed = rotationSpeed;
        public void SetMinDistance(float minDistance) => _minDistance = minDistance;
        public void SetMaxDistance(float maxDistance) => _maxDistance = maxDistance;
        public void SetOscillationSpeed(float oscillationSpeed) => _oscillationSpeed = oscillationSpeed;
    }
}
