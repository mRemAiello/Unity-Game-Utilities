using System;

namespace GameUtils
{
    [Serializable]
    /// <summary>
    /// Base class for item feature data that exposes type-safe access helpers.
    /// </summary>
    public abstract class ItemFeatureData
    {
        /// <summary>
        /// Gets the runtime type of the feature instance.
        /// </summary>
        public Type FeatureType => GetType();

        /// <summary>
        /// Attempts to cast this feature to the requested feature type.
        /// </summary>
        /// <typeparam name="TFeature">Requested feature type.</typeparam>
        /// <returns>The feature cast to <typeparamref name="TFeature"/> if compatible; otherwise null.</returns>
        public TFeature Get<TFeature>() where TFeature : ItemFeatureData
        {
            return this as TFeature;
        }

        /// <summary>
        /// Attempts to retrieve this feature as the requested feature type.
        /// </summary>
        /// <typeparam name="TFeature">Requested feature type.</typeparam>
        /// <param name="feature">The feature instance if the cast succeeds.</param>
        /// <returns>True when the cast succeeds; otherwise false.</returns>
        public bool TryGet<TFeature>(out TFeature feature) where TFeature : ItemFeatureData
        {
            if (this is TFeature typedFeature)
            {
                feature = typedFeature;
                return true;
            }

            feature = null;
            return false;
        }
    }
}
