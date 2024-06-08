using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using VInspector;

namespace UnityEditor.GameUtils
{
    [CreateAssetMenu(menuName = "Game Utils/Auto Bundles/Auto Bundles")]
    public class AutoBundles : ScriptableObject
    {
        [SerializeField] private List<AutoBundleData> _bundleDatas;

        [Button]
        public void MarkAllFilesAsAddressables()
        {
            // Ottieni il settings object degli Addressables
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("No Addressables settings found.");
                return;
            }

            //
            foreach (var bundleData in _bundleDatas)
            {
                // Creo il gruppo eventualmente
                var group = settings.FindGroup(bundleData.GroupName);
                if (group == null)
                {
                    group = settings.CreateGroup(bundleData.GroupName, false, false, true, null, 
                        typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
                }

                // Scansiona tutti i file nella cartella specificata
                string[] files = Directory.GetFiles(bundleData.FolderName, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    // Escludi cartelle
                    if (AssetDatabase.IsValidFolder(file))
                        continue;

                    // Escludi i file .meta
                    if (file.EndsWith(".meta"))
                        continue;

                    //
                    string relativePath = file.Replace(Application.dataPath, "").Replace("\\", "/");

                    // Aggiungi l'asset come Addressable
                    var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(relativePath), group);
                    Debug.Log($"Added {relativePath} as Addressable.");

                    // Determina il tipo di asset e aggiungi la label
                    var assetType = AssetDatabase.GetMainAssetTypeAtPath(relativePath);
                    if (assetType != null)
                    {
                        // Rimuovo tutte le label
                        entry.labels.Clear();

                        // Aggiungo
                        string label = assetType.Name;
                        if (!entry.labels.Contains(label))
                        {
                            entry.SetLabel(label, true, true);
                            Debug.Log($"Added label '{label}' to {relativePath}.");
                        }
                    }
                }
            }

            // Salva le modifiche
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("All files marked as Addressables.");
        }
    }
}