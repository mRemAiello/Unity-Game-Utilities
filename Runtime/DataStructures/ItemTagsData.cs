using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Features")]
    public class ItemTagsData : ItemIdentifierData, ITaggable
    {
        [SerializeField, Group("Features")] private List<GameTag> _tags = new();

        //
        public IReadOnlyList<GameTag> Tags => _tags;
    }
}