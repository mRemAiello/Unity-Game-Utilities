using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Singleton manager that provides access to attribute data assets.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class AttributeDataManager : GenericDataManager<AttributeDataManager, AttributeData>
    {
    }
}
