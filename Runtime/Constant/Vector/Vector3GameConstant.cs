using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.CONSTANTS_NAME + "Vector3")]
    public class Vector3GameConstant : GameConstant
    {
        [SerializeField] private Vector3 _value;

        //
        public Vector3 VectorValue => _value;
        public override string StringValue => _value.ToString();
    }
}