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
    [CreateAssetMenu(menuName = Constant.CURRENCY_NAME + "Currency Data")]
    public class AutoBundles : ScriptableObject, ILoggable
    {
        [SerializeField] private bool _showLog = false;
        [SerializeField, TableList(Draggable = true)] private List<AutoBundleData> _bundleDatas;

        //
        public bool LogEnabled => _showLog;

        //
        [Button(ButtonSizes.Medium)]
        public void MarkAllFilesAsAddressables()
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

                    //
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