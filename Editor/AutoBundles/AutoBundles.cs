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
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ADDRESSABLES_NAME + "Auto Bundle")]
    public class AutoBundles : ScriptableObject, ILoggable
    {
        [Title("Results")]
        [SerializeField, ListDrawerSettings(Draggable = false, AlwaysExpanded = true)] private List<AutoBundleData> _bundleDatas;

        [Title("Settings")]
        [SerializeField] private bool _logEnabled = false;
        [SerializeField] private List<string> _excludedFolders = new();
        [SerializeField] private List<string> _mergedFolders = new();
        [SerializeField] private List<string> _excludedExtensions = new() { ".meta", ".cs" };

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button(ButtonSizes.Medium)]
        protected virtual List<string> CreateAutoAssetFolders(int depth)
        {
            // 
            List<string> result = new();
            string assetsPath = Application.dataPath;

            //
            ResetBundleDatas();

            // 
            AutoBundleArgs args = BuildAutoBundleArgs(assetsPath, depth, result);
            ExploreFolders(args);

            //
            PopulateBundleDatas(result, assetsPath);

            return result;
        }

        private void ResetBundleDatas()
        {
            // 
            _bundleDatas = new List<AutoBundleData>();
        }

        private AutoBundleArgs BuildAutoBundleArgs(string assetsPath, int depth, List<string> result)
        {
            // 
            AutoBundleArgs args = new()
            {
                CurrentPath = assetsPath,
                CurrentDepth = 1,
                MaxDepth = depth,
                Result = result,
                ExcludedFolders = _excludedFolders,
                MergedFolders = _mergedFolders
            };

            return args;
        }

        private void PopulateBundleDatas(List<string> folders, string assetsPath)
        {
            // 
            for (int i = 0; i < folders.Count; i++)
            {
                // 
                folders[i] = BuildAssetPath(folders[i], assetsPath);

                //
                string groupName = GetGroupName(folders[i]);
                _bundleDatas.Add(new AutoBundleData(folders[i], groupName));
            }
        }

        private string BuildAssetPath(string folderPath, string assetsPath)
        {
            // 
            string assetPath = "Assets" + folderPath.Replace(assetsPath, "").Replace("\\", "/");
            return assetPath;
        }

        // Funzione ricorsiva per esplorare le cartelle
        protected virtual void ExploreFolders(AutoBundleArgs args)
        {
            if (args.CurrentDepth > args.MaxDepth)
                return;

            //
            string[] subFolders = Directory.GetDirectories(args.CurrentPath);
            foreach (string folder in subFolders)
            {
                string folderName = Path.GetFileName(folder);

                //
                if (args.ExcludedFolders.Contains(folderName))
                    continue;

                //
                args.Result.Add(folder);

                //
                if (args.MergedFolders.Contains(folderName))
                    continue;

                // 
                args.CurrentDepth += 1;
                ExploreFolders(args);
            }
        }

        [Button(ButtonSizes.Medium)]
        protected virtual void MarkAllFilesAsAddressables()
        {
            // 
            var settings = GetAddressableSettings();
            if (settings == null)
                return;

            //
            RemoveExcludedExtensionsFromGroups(settings);

            //
            ProcessBundleDatas(settings);

            // 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //
            this.Log("All files marked as Addressables.");
        }

        private AddressableAssetSettings GetAddressableSettings()
        {
            // 
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                this.LogError("No Addressables settings found.");
                return null;
            }

            return settings;
        }

        private void ProcessBundleDatas(AddressableAssetSettings settings)
        {
            // 
            foreach (var bundleData in _bundleDatas)
            {
                // 
                ProcessBundleData(settings, bundleData);
            }
        }

        private void ProcessBundleData(AddressableAssetSettings settings, AutoBundleData bundleData)
        {
            // 
            var group = SetupGroup(settings, bundleData);

            //
            string[] files = Directory.GetFiles(bundleData.FolderName, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                //
                if (ShouldSkipFile(file))
                    continue;

                //
                string relativePath = file.Replace(Application.dataPath, "").Replace("\\", "/");
                var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(relativePath), group);
                if (entry == null)
                {
                    this.Log($"Failed to make an entry for {relativePath}");
                    continue;
                }

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

        private bool ShouldSkipFile(string filePath)
        {
            // 
            if (AssetDatabase.IsValidFolder(filePath))
                return true;

            //
            foreach (string ext in _excludedExtensions)
            {
                if (filePath.EndsWith(ext))
                    return true;
            }

            return false;
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
                List<AddressableAssetEntry> entries = group.entries;
                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    // 
                    AddressableAssetEntry entry = entries[i];
                    if (entry == null)
                        continue;

                    // 
                    string assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
                    if (string.IsNullOrEmpty(assetPath))
                        continue;

                    // 
                    string extension = Path.GetExtension(assetPath).ToLowerInvariant();
                    if (!normalizedExtensions.Contains(extension))
                        continue;

                    //
                    group.RemoveAssetEntry(entry);

                    //
                    this.Log($"Removed {assetPath} from group {group.Name} due to excluded extension.");
                }
            }
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
            var labels = new List<string>(bundleData.Labels)
            {
                assetType.Name
            };

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
