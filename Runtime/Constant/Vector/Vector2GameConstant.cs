using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Game Utils/Constants/Vector2")]
    public class Vector2GameConstant : GameConstant
    {
        [SerializeField] private Vector2 _value;

        //
        public Vector2 VectorValue => _value;
        public override string StringValue => _value.ToString();
    }
}