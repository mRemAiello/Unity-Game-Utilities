using System;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [Serializable]
    public class AutoBundleData
    {
        [SerializeField] private string _folderName;
        [SerializeField] private string _groupName;

        // public fields
        public string FolderName => _folderName;
        public string GroupName => _groupName;
    }
}