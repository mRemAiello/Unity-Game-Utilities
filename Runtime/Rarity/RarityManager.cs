using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Manager that exposes all <see cref="Rarity"/> definitions and allows lookup by internal name or ID.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class RarityManager : ItemAssetManager<RarityManager, Rarity>
    {
    }
}
