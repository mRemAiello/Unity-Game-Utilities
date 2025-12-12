using GameUtils;
using TriInspector;
using UnityEngine;

namespace Tools.UI.Card
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.CARDS_NAME + "Card Config")]
    [DeclareBoxGroup("Disable")]
    [DeclareBoxGroup("Hover")]
    [DeclareBoxGroup("Bend")]
    [DeclareBoxGroup("Rotation")]
    [DeclareBoxGroup("Movement")]
    [DeclareBoxGroup("Scale")]
    [DeclareBoxGroup("Draw")]
    [DeclareBoxGroup("Discard")]
    public class CardParameters : ScriptableObject
    {
        [SerializeField, Range(0.1f, 1), Group("Disable")] private float _disabledAlpha;
        [SerializeField, Range(0, 4), Group("Hover")] private float _hoverHeight;
        [SerializeField, Group("Hover")] private bool _hoverRotation;
        [SerializeField, Range(0.9f, 2f), Group("Hover")] private float _hoverScale;
        [SerializeField, Range(0, 25), Group("Hover")] private float _hoverSpeed;
        [SerializeField, Range(0f, 1f), Group("Bend")] private float _height;
        [SerializeField, Range(0f, -5f), Group("Bend")] private float _spacing;
        [SerializeField, Range(0, 60), Group("Bend")] private float _bentAngle;
        [SerializeField, Range(0, 60), Group("Rotation")] private float _rotationSpeed;
        [SerializeField, Range(0, 1000), Group("Rotation")] private float _rotationSpeedP2;
        [SerializeField, Range(0, 15), Group("Movement")] private float _movementSpeed;
        [SerializeField, Range(0, 15), Group("Scale")] private float _scaleSpeed;
        [SerializeField, Range(0, 1), Group("Draw")] private float _startSizeWhenDraw;
        [SerializeField, Range(0, 1), Group("Discard")] private float _discardedSize;

        //
        [Button]
        public void SetDefaults()
        {
            _disabledAlpha = 0.5f;

            _hoverHeight = 1;
            _hoverRotation = false;
            _hoverScale = 1.3f;
            _hoverSpeed = 15f;

            _height = 0.12f;
            _spacing = -2;
            _bentAngle = 20;

            _rotationSpeedP2 = 500;
            _rotationSpeed = 20;
            _movementSpeed = 4;
            _scaleSpeed = 7;

            _startSizeWhenDraw = 0.05f;
            _discardedSize = 0.5f;
        }

        //
        public float DisabledAlpha => _disabledAlpha;
        public float HoverHeight => _hoverHeight;
        public bool HoverRotation => _hoverRotation;
        public float HoverScale => _hoverScale;
        public float HoverSpeed => _hoverSpeed;
        public float Height => _height;
        public float Spacing => _spacing;
        public float BentAngle => _bentAngle;
        public float RotationSpeed => _rotationSpeed;
        public float RotationSpeedP2 => _rotationSpeedP2;
        public float MovementSpeed => _movementSpeed;
        public float ScaleSpeed => _scaleSpeed;
        public float StartSizeWhenDraw => _startSizeWhenDraw;
        public float DiscardedSize => _discardedSize;
    }
}