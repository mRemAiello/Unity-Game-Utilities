using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Visual")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class ProjectileVisual2D : MonoBehaviour
    {
        [SerializeField, Group("Visual")] private Transform _projectileVisual;
        [SerializeField, Group("Visual")] private Transform _projectileShadow;
        [SerializeField, Group("Visual")] private Projectile2D _projectile;
        [SerializeField, Group("Visual")] private float _shadowPositionDivider = 6f;

        //
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _target;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _trajectoryStartPosition;

        //
        public void SetTarget(Vector3 target)
        {
            _target = target;
        }

        void Start()
        {
            _trajectoryStartPosition = transform.position;
        }

        void Update()
        {
            UpdateProjectileRotation();
            UpdateShadowPosition();

            //
            float trajectoryProgressMagnitude = (transform.position - _trajectoryStartPosition).magnitude;
            float trajectoryMagnitude = (_target - _trajectoryStartPosition).magnitude;

            //
            float trajectoryProgressNormalized = trajectoryProgressMagnitude / trajectoryMagnitude;
            if (trajectoryProgressNormalized < .7f)
            {
                UpdateProjectileShadowRotation();
            }
        }

        private void UpdateProjectileRotation()
        {
            Vector3 projectileMoveDir = _projectile.GetProjectileMoveDir();
            _projectileVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
        }

        private void UpdateProjectileShadowRotation()
        {
            if (_projectile == null || _projectileShadow == null)
            {
                return;
            }

            //
            Vector3 projectileMoveDir = _projectile.GetProjectileMoveDir();
            _projectileShadow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
        }

        private void UpdateShadowPosition()
        {
            if (_projectile == null || _projectileShadow == null)
            {
                return;
            }

            //
            Vector3 newPosition = transform.position;
            Vector3 trajectoryRange = _target - _trajectoryStartPosition;

            //
            if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y))
            {
                // Projectile is curved on the X axis
                float nextX = _projectile.GetNextXTrajectoryPosition();
                float nextXAbsolute = _projectile.GetNextPositionXCorrectionAbsolute();
                newPosition.x = _trajectoryStartPosition.x + nextX / _shadowPositionDivider + nextXAbsolute;
            }
            else
            {
                // Projectile is curved on the Y axis
                float nextY = _projectile.GetNextYTrajectoryPosition();
                float nextYAbsolute = _projectile.GetNextPositionYCorrectionAbsolute();
                newPosition.y = _trajectoryStartPosition.y + nextY / _shadowPositionDivider + nextYAbsolute;
            }

            //
            _projectileShadow.position = newPosition;
        }
    }
}