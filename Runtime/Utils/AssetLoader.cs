using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUtils
{
    public class AssetLoader
    {
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