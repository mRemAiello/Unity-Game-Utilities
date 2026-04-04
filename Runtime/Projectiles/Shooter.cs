using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Projectile")]
    [DeclareBoxGroup("Curves")]
    [DeclareBoxGroup("Debug")]
    public class Shooter : MonoBehaviour
    {
        [SerializeField, Group("Projectile")] private GameObject _projPrefab;
        [SerializeField, Group("Projectile")] private Transform _target;
        [SerializeField, Group("Projectile")] private float _shootRate;
        [SerializeField, Group("Projectile")] private float _projMaxMoveSpeed;
        [SerializeField, Group("Projectile")] private float _projMaxHeight;

        //
        [SerializeField, Group("Curves")] private AnimationCurve _projSpeedCurve;
        [SerializeField, Group("Curves")] private AnimationCurve _trajectoryCurve;
        [SerializeField, Group("Curves")] private AnimationCurve _axisCorrectionCurve;

        //
        [SerializeField, Group("Debug"), ReadOnly] private float _shootTimer;

        //
        void Update()
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer > 0)
                return;

            //
            _shootTimer = _shootRate;
            GameObject projGameObject = Instantiate(_projPrefab, transform.position, Quaternion.identity);
            if (projGameObject.TryGetComponent(out IProjectile projectile))
            {
                projectile.InitProjectile(_target.position, _projMaxMoveSpeed, _projMaxHeight);
                projectile.InitAnimationCurves(_trajectoryCurve, _axisCorrectionCurve, _projSpeedCurve);
            }
        }
    }
}