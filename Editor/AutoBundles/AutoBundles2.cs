using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameUtils;
using TriInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ADDRESSABLES_NAME + "Auto Bundle 2")]
    public class AutoBundles2 : ScriptableObject, ILoggable
    {
        [SerializeField] private bool _logEnabled = false;

        //
        [SerializeField, TableList(Draggable = false, AlwaysExpanded = true)]
        private List<AutoBundleData> _bundleDatas;

        [SerializeField, TableList(Draggable = false, AlwaysExpanded = true)]
        private List<string> _excludedFolders = new() { "AddressableAssetsData", "Editor", "Plugins", "Resources", "Scripts", "Settings" };

        [SerializeField, TableList(Draggable = false, AlwaysExpanded = true)]
        private List<string> _excludedExtensions = new() { ".meta", ".cs" };

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button(ButtonSizes.Medium)]
        public void PopulateBundleDatasFromAssets()
        {
            _bundleDatas ??= new List<AutoBundleData>();

            // 
            string assetsPath = Application.dataPath;
            List<string> folders = CollectAssetFolders(assetsPath);

            // 
            AddMissingBundleDatas(folders, assetsPath);
            SyncBundleDatasWithFolders(folders, assetsPath);
            SortBundleDatas();
        }

        [Button(ButtonSizes.Medium)]
        public void SyncAddressableGroups()
        {
            // 
            AddressableAssetSettings settings = GetAddressableSettings();
            if (settings == null)
                return;

            // 
            RemoveExcludedExtensionsFromGroups(settings);

            // 
            foreach (AutoBundleData bundleData in _bundleDatas)
            {
                // 
                ProcessBundleData(settings, bundleData);
            }

            // 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private List<string> CollectAssetFolders(string assetsPath)
        {
            List<string> result = new();

            // 
            string[] topLevelFolders = Directory.GetDirectories(assetsPath);
            foreach (string folder in topLevelFolders)
            {
                string relativePath = BuildRelativeAssetPath(folder, assetsPath);
                if (!IsExcluded(relativePath))
                {
                    result.Add(folder);

                    // 
                    string[] subFolders = Directory.GetDirectories(folder);
                    foreach (string subFolder in subFolders)
                    {
                        string subRelativePath = BuildRelativeAssetPath(subFolder, assetsPath);
                        if (IsExcluded(subRelativePath))
                            continue;

                        // 
                        result.Add(subFolder);
                    }
                }
            }

            //
            return result;
        }

        private AddressableAssetSettings GetAddressableSettings()
        {
            // 
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                // 
                this.LogError("No Addressables settings found.");
                return null;
            }

            return settings;
        }

        private void ProcessBundleData(AddressableAssetSettings settings, AutoBundleData bundleData)
        {
            // 
            if (bundleData == null)
                return;

            // 
            if (string.IsNullOrWhiteSpace(bundleData.FolderName))
                return;

            // 
            AddressableAssetGroup group = SetupGroup(settings, bundleData);

            // 
            string[] files = Directory.GetFiles(bundleData.FolderName, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                // 
                if (ShouldSkipFile(file))
                    continue;

                // 
                string assetPath = BuildAssetPathFromFile(file);
                AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(assetPath), group);
                if (entry == null)
                    continue;

                // 
                Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (assetType == null)
                    continue;

                // 
                SetupLabels(assetType, entry, bundleData);
            }
        }

        private void AddMissingBundleDatas(List<string> folders, string assetsPath)
        {
            HashSet<string> existingFolders = BuildExistingFolderSet();

            // 
            foreach (string folder in folders)
            {
                // 
                string assetPath = BuildAssetPath(folder, assetsPath);
                if (existingFolders.Contains(assetPath))
                    continue;

                // 
                string groupName = GetGroupName(assetPath);
                _bundleDatas.Add(new AutoBundleData(assetPath, groupName));
            }
        }

        private void SyncBundleDatasWithFolders(List<string> folders, string assetsPath)
        {
            // 
            HashSet<string> validAssetPaths = new(StringComparer.OrdinalIgnoreCase);

            // 
            foreach (string folder in folders)
            {
                // 
                string assetPath = BuildAssetPath(folder, assetsPath);
                validAssetPaths.Add(assetPath);
            }

            // 
            _bundleDatas = _bundleDatas.Where(bundleData =>
                {
                    if (bundleData == null)
                        return false;

                    // 
                    if (string.IsNullOrWhiteSpace(bundleData.FolderName))
                        return false;

                    // 
                    return validAssetPaths.Contains(bundleData.FolderName);
                })
                .ToList();
        }

        private HashSet<string> BuildExistingFolderSet()
        {
            // 
            HashSet<string> existingFolders = new(StringComparer.OrdinalIgnoreCase);

            // 
            foreach (AutoBundleData bundleData in _bundleDatas)
            {
                // 
                if (bundleData == null)
                    continue;

                // 
                if (string.IsNullOrWhiteSpace(bundleData.FolderName))
                    continue;

                // 
                existingFolders.Add(bundleData.FolderName);
            }

            return existingFolders;
        }

        private bool ShouldSkipFile(string filePath)
        {
            // 
            if (AssetDatabase.IsValidFolder(filePath))
                return true;

            // 
            foreach (string ext in _excludedExtensions)
            {
                // 
                if (filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string BuildAssetPathFromFile(string filePath)
        {
            // 
            string assetPath = "Assets" + filePath.Replace(Application.dataPath, "").Replace("\\", "/");
            return assetPath;
        }

        private AddressableAssetGroup SetupGroup(AddressableAssetSettings settings, AutoBundleData bundleData)
        {
            // 
            AddressableAssetGroup group = settings.FindGroup(bundleData.GroupName);
            if (group == null)
            {
                // 
                Type contentGroup = typeof(ContentUpdateGroupSchema);
                Type bundledAssetGroup = typeof(BundledAssetGroupSchema);

                // 
                group = settings.CreateGroup(bundleData.GroupName, false, false, true, null, contentGroup, bundledAssetGroup);
            }

            return group;
        }

        private void SetupLabels(Type assetType, AddressableAssetEntry entry, AutoBundleData bundleData)
        {
            // 
            entry.labels.Clear();

            // 
            List<string> labels = new(bundleData.Labels)
            {
                assetType.Name
            };

            // 
            foreach (string label in labels)
            {
                // 
                if (!entry.labels.Contains(label))
                    entry.SetLabel(label, true, true);
            }
        }

        private void RemoveExcludedExtensionsFromGroups(AddressableAssetSettings settings)
        {
            // 
            HashSet<string> normalizedExtensions = BuildNormalizedExtensions();
            if (normalizedExtensions.Count == 0)
                return;

            // 
            foreach (AddressableAssetGroup group in settings.groups)
            {
                // 
                if (group == null)
                    continue;

                // 
                List<AddressableAssetEntry> entries = group.entries.ToList();
                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    // 
                    AddressableAssetEntry entry = entries[i];
                    if (entry == null)
                        continue;

                    // 
                    string assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
                    if (string.IsNullOrWhiteSpace(assetPath))
                        continue;

                    // 
                    if (!HasExcludedExtension(assetPath, normalizedExtensions))
                        continue;

                    // 
                    group.RemoveAssetEntry(entry);
                }
            }
        }

        private bool HasExcludedExtension(string assetPath, HashSet<string> normalizedExtensions)
        {
            // 
            foreach (string excludedExtension in normalizedExtensions)
            {
                // 
                if (assetPath.EndsWith(excludedExtension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private HashSet<string> BuildNormalizedExtensions()
        {
            // 
            HashSet<string> normalizedExtensions = new(StringComparer.OrdinalIgnoreCase);

            // 
            foreach (string extension in _excludedExtensions)
            {
                // 
                if (string.IsNullOrWhiteSpace(extension))
                    continue;

                // 
                string normalizedExtension = extension.StartsWith(".") ? extension : $".{extension}";
                normalizedExtensions.Add(normalizedExtension.ToLowerInvariant());
            }

            return normalizedExtensions;
        }

        private string BuildRelativeAssetPath(string folderPath, string assetsPath)
        {
            // 
            string relativePath = folderPath.Replace(assetsPath, string.Empty).Replace("\\", "/").TrimStart('/');
            return relativePath;
        }

        private bool IsExcluded(string relativePath)
        {
            foreach (string excludedFolder in _excludedFolders)
            {
                if (string.Equals(relativePath, excludedFolder, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string BuildAssetPath(string folderPath, string assetsPath)
        {
            string assetPath = "Assets/" + BuildRelativeAssetPath(folderPath, assetsPath);
            return assetPath;
        }

        private string GetGroupName(string assetPath)
        {
            string groupName = assetPath.Replace("Assets/", "").Replace("/", "").Replace("\\", "").Replace(" ", "");
            return groupName;
        }

        private void SortBundleDatas()
        {
            _bundleDatas = _bundleDatas.Where(bundle => bundle != null).OrderBy(bundle => bundle.GroupName, StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
