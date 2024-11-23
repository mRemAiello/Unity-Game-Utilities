using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Game Utils/Constants/String")]
    public class StringConstant : GameConstant
    {
        [SerializeField] private string _value;

        //
        public override string StringValue => _value;
    }
}