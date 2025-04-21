using System;
using System.Collections.Generic;
using System.IO;
using GameUtils;
using TriInspector;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CreateAssetMenu(menuName = Constant.ADDRESSABLES_NAME + "Auto Bundle")]
    public class AutoBundles : ScriptableObject, ILoggable
    {
        [SerializeField, TableList(Draggable = true)] private List<AutoBundleData> _bundleDatas;

        //
        [SerializeField] private bool _logEnabled = false;
        [SerializeField] private List<string> _excludedFolders = new();
        [SerializeField] private List<string> _mergedFolders = new();

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button(ButtonSizes.Medium)]
        protected virtual List<string> CreateAutoAssetFolders(int depth)
        {
            List<string> result = new();
            string assetsPath = Application.dataPath;

            //
            _bundleDatas = new List<AutoBundleData>();

            // 
            ExploreFolders(assetsPath, 1, depth, result, _excludedFolders, _mergedFolders);
            for (int i = 0; i < result.Count; i++)
            {
                result[i] = "Assets" + result[i].Replace(assetsPath, "").Replace("\\", "/");
                string groupName = GetGroupName(result[i]);
                _bundleDatas.Add(new AutoBundleData(result[i], groupName));
            }

            return result;
        }

        // Funzione ricorsiva per esplorare le cartelle
        protected virtual void ExploreFolders(string currentPath, int currentDepth, int maxDepth,
                                                List<string> result, List<string> excludedFolders, List<string> mergedFolders)
        {
            if (currentDepth > maxDepth)
                return;

            //
            string[] subFolders = Directory.GetDirectories(currentPath);
            foreach (string folder in subFolders)
            {
                string folderName = Path.GetFileName(folder);

                //
                if (excludedFolders.Contains(folderName))
                    continue;

                //
                result.Add(folder);

                //
                if (mergedFolders.Contains(folderName))
                    continue;

                // 
                ExploreFolders(folder, currentDepth + 1, maxDepth, result, excludedFolders, mergedFolders);
            }
        }

        [Button(ButtonSizes.Medium)]
        protected virtual void MarkAllFilesAsAddressables()
        {
            // 
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                this.LogError("No Addressables settings found.");
                return;
            }

            //
            foreach (var bundleData in _bundleDatas)
            {
                var group = SetupGroup(settings, bundleData);

                //
                string[] files = Directory.GetFiles(bundleData.FolderName, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    //
                    if (AssetDatabase.IsValidFolder(file))
                        continue;

                    //
                    if (file.EndsWith(".meta"))
                        continue;

                    //
                    string relativePath = file.Replace(Application.dataPath, "").Replace("\\", "/");
                    var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(relativePath), group);

                    // 
                    var assetType = AssetDatabase.GetMainAssetTypeAtPath(relativePath);
                    if (assetType != null)
                    {
                        SetupLabel(assetType, entry, bundleData);
                        SetupAddress(entry);

                        // 
                        this.Log($"Added {relativePath} as Addressable.");
                    }
                }
            }

            // 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //
            this.Log("All files marked as Addressables.");
        }

        private string GetGroupName(string folderName)
        {
            // 
            string groupName = folderName.Replace("Assets/", "").Replace("/", "").Replace("\\", "").Replace(" ", "");
            return groupName;
        }

        private AddressableAssetGroup SetupGroup(AddressableAssetSettings settings, AutoBundleData bundleData)
        {
            // 
            var group = settings.FindGroup(bundleData.GroupName);
            if (group == null)
            {
                var contentGroup = typeof(ContentUpdateGroupSchema);
                var bundledAssetGroup = typeof(BundledAssetGroupSchema);

                //
                group = settings.CreateGroup(bundleData.GroupName, false, false, true, null, contentGroup, bundledAssetGroup);
            }

            return group;
        }

        private void SetupLabel(Type assetType, AddressableAssetEntry entry, AutoBundleData bundleData)
        {
            // 
            entry.labels.Clear();

            //
            var labels = bundleData.Labels;
            labels.Add(assetType.Name);

            // 
            foreach (var label in labels)
            {
                if (!entry.labels.Contains(label))
                {
                    entry.SetLabel(label, true, true);
                }
            }
        }

        private void SetupAddress(AddressableAssetEntry entry)
        {
            string fileName = Path.GetFileNameWithoutExtension(entry.address).Replace("_", " ");
            entry.SetAddress(fileName);
        }
    }
}