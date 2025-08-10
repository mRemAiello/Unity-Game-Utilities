using UnityEngine;

namespace GameUtils
{
    public interface IProjectile
    {
        //
        public Vector3 GetProjectileMoveDir();
        public float GetNextYTrajectoryPosition();
        public float GetNextPositionYCorrectionAbsolute();
        public float GetNextXTrajectoryPosition();
        public float GetNextPositionXCorrectionAbsolute();

        //
        void InitProjectile(Vector3 target, float maxMoveSpeed, float trajectoryMaxHeight);
        void InitAnimationCurves(AnimationCurve trajectoryCurve, AnimationCurve axisCorrectCurve, AnimationCurve projSpeedCurve);
    }
}