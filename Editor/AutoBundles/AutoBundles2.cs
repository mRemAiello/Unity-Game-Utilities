using System.Collections.Generic;
using GameUtils;
using TriInspector;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.ADDRESSABLES_NAME + "Auto Bundle 2")]
    public class AutoBundles2  : ScriptableObject, ILoggable
    {
        [SerializeField] private bool _logEnabled = false;

        //
        [SerializeField, TableList(Draggable = false, AlwaysExpanded = true)] 
        private List<AutoBundleData> _bundleDatas;
        
        [SerializeField,  TableList(Draggable = false, AlwaysExpanded = true)] 
        private List<string> _excludedFolders = new() { "AddressableAssetsData", "Editor", "Plugins", "Resources", "Scripts", "Settings" };
        
        [SerializeField,  TableList(Draggable = false, AlwaysExpanded = true)] 
        private List<string> _mergedFolders = new();
        
        [SerializeField,  TableList(Draggable = false, AlwaysExpanded = true)] 
        private List<string> _excludedExtensions = new() { ".meta", ".cs" };

        //
        public bool LogEnabled => _logEnabled;
    }
}