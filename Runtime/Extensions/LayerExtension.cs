using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public static class LayerExtension
    {
        public static void SetLayerMask(this GameObject obj, LayerMask layerMask)
        {
            List<int> layers = GetLayersFromMask(layerMask);
            
            //
            if (layers.Count == 1)
            {
                obj.layer = layers[0];
            }
            else if (layers.Count > 1)
            {
                Debug.LogError("You cannot assign more than a Layer in LayerMask");
            }
            else
            {
                Debug.LogError("Not a valid LayerMask.");
            }
        }

        private static List<int> GetLayersFromMask(LayerMask layerMask)
        {
            List<int> layers = new();
            int layerMaskValue = layerMask.value;

            //
            for (int i = 0; i < 32; i++)
            {
                if ((layerMaskValue & (1 << i)) != 0)
                {
                    layers.Add(i);
                }
            }

            return layers;
        }

        public static LayerMask GetLayerMask(this GameObject obj)
        {
            return 1 << obj.layer;
        }

        public static bool ContainsLayerMask(this GameObject obj, LayerMask layerMask)
        {
            return layerMask.Contains(obj);
        }

        public static void Add(ref this LayerMask mask, int layer)
        {
            mask |= 1 << layer;
        }
        
        public static void Remove(ref this LayerMask mask, int layer)
        {
            mask &= ~(1 << layer);
        }
        
        public static bool Contains(this LayerMask mask, int layer)
        {
            return 0 != (mask & (1 << layer));
        }
        
        public static bool Contains(this LayerMask mask, GameObject obj)
        {
            if (obj == null)
                return false;

            return 0 != (mask & (1 << obj.layer));
        }
        
        public static bool Contains(this LayerMask mask, Component component)
        {
            if (component == null)
                return false;

            return 0 != (mask & (1 << component.gameObject.layer));
        }
    }
}
