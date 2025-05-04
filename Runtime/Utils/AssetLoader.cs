using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUtils
{
    public class AssetLoader
    {
        // TODO: Da migliorare
        public static T LoadAssetSync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            if (assetReference == null)
                return default;

            if (assetReference.OperationHandle.IsValid())
            {
                if (assetReference.OperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    return assetReference.OperationHandle.Result as T;
                }
                else if (assetReference.OperationHandle.Status == AsyncOperationStatus.None)
                {
                    return assetReference.LoadAssetAsync<T>().WaitForCompletion();
                }
                else
                {
                    Debug.LogWarning($"Asset already requested with status: {assetReference.OperationHandle.Status}");
                    return null;
                }
            }

            return assetReference.LoadAssetAsync<T>().WaitForCompletion();
        }

        public static void LoadAssetAsync<T>(AssetReference assetReference, Action<AsyncOperationHandle<T>> callback)
        {
            if (assetReference.IsValid() && assetReference.OperationHandle.IsValid())
            {
                if (assetReference.OperationHandle.Status == AsyncOperationStatus.None)
                {
                    assetReference.OperationHandle.Completed += handle => callback(assetReference.OperationHandle.Convert<T>());
                }
                else
                {
                    callback(assetReference.OperationHandle.Convert<T>());
                }
            }
            else
            {
                AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();
                handle.Completed += op => callback(handle);
            }
        }
    }
}