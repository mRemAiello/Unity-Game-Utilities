using System;

namespace GameUtils
{
    [Serializable]
    public abstract class ItemFeature
    {
        public Type FeatureType => GetType();

        //
        public TFeature Get<TFeature>() where TFeature : ItemFeature
        {
            return this as TFeature;
        }

        public bool TryGet<TFeature>(out TFeature feature) where TFeature : ItemFeature
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