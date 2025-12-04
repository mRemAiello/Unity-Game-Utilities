using System.Collections.Generic;

namespace GameUtils
{
    public interface ITaggable
    {
        public IReadOnlyList<GameTag> Tags { get; }
    }
}