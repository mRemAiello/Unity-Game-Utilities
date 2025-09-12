using System.Collections.Generic;

namespace GameUtils
{
    public interface ITaggable
    {
        public List<GameTag> Tags { get; }
    }
}