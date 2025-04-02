using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("stat", Title = "Stat")]
    [CreateAssetMenu(fileName = "Stat Data", menuName = "GD/RPG/Stat", order = 0)]
    public class StatData : ItemAssetBase
    {
        [SerializeField, Group("stat")] private int _baseValue = 0;
        [SerializeField, Group("stat")] private int _minValue;
        [SerializeField, Group("stat")] private int _maxValue;
    }
}