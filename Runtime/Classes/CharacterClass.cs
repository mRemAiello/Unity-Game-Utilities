using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(fileName = "CharacterClass", menuName = "GD/RPG/Character Class", order = 0)]
    [DeclareBoxGroup("class", Title = "Class")]
    public class CharacterClass : ItemAssetBase
    {
        [TableList(), SerializeField] private List<StatBaseValue> _stats = new();

        //
        public List<StatBaseValue> Stats => _stats;
    }
}