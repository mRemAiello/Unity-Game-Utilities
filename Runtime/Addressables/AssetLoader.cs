using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUtils
{
    public class AssetLoader
    {
        /// <summary>
        /// Loads an asset referenced by <paramref name="assetReference"/> using Addressables.
        /// The call is fully asynchronous and will return when the operation completes.
        /// 
        /// In case of failure the method logs the error and rethrows the exception so that
        /// callers can handle it with a try/catch block or faulted Task callbacks.
        /// If the operation completes with <see cref="AsyncOperationStatus.None"/> the
        /// method logs a warning and returns <c>default</c>.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="assetReference">Addressable reference to the asset.</param>
        /// <returns>A task that resolves to the loaded asset.</returns>
        public static async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            if (assetReference == null)
                return default;

            AsyncOperationHandle<T> handle;

            if (assetReference.OperationHandle.IsValid())
            {
                handle = assetReference.OperationHandle.Convert<T>();

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load asset {assetReference.SubObjectName}: {handle.OperationException}");
                    throw handle.OperationException ?? new Exception("AsyncOperation failed without exception.");
                }

                // Status == None, wait for completion of the ongoing operation
                await handle.Task;
            }
            else
            {
                handle = assetReference.LoadAssetAsync<T>();
                await handle.Task;
            }

            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    return handle.Result;
                case AsyncOperationStatus.Failed:
                    Debug.LogError($"Failed to load asset {assetReference.SubObjectName}: {handle.OperationException}");
                    throw handle.OperationException ?? new Exception("AsyncOperation failed without exception.");
                default:
                    Debug.LogWarning($"Asset load completed with unexpected status: {handle.Status}");
                    return default;
            }
        }
    }
}
