using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameUtils;
using TriInspector;
using UnityEditor.VersionControl;
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
            SortBundleDatas();
        }

        [Button(ButtonSizes.Medium)]
        public void ClearBundleData()
        {
            _bundleDatas = new List<AutoBundleData>();
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
                        this.Log(subFolder);
                    }
                }
            }

            //
            return result;
        }

        private void AddMissingBundleDatas(List<string> folders, string assetsPath)
        {
            // 
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
