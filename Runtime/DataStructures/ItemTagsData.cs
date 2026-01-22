using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("features", Title = "Features")]
    public class ItemTagsData : ItemIdentifierData, ITaggable
    {
        [SerializeField, Group("features")] private List<GameTag> _tags = new();

        //
        public IReadOnlyList<GameTag> Tags => _tags;
    }
}