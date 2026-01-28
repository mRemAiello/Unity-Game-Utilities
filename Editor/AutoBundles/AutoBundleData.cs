using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [Serializable]
    public class AutoBundleData
    {
        [SerializeField] private string _folderName;
        [SerializeField, HideInInspector] private string _groupName;
        [SerializeField, TableList()] private List<string> _labels;

        // public fields
        public string FolderName => _folderName;
        public string GroupName => _groupName;

        //
        public IReadOnlyList<string> Labels
        {
            get
            {
                _labels ??= new List<string>();
                var list = new List<string>();
                foreach (var item in _labels)
                {
                    list.Add(item);
                }

                return list;
            }
        }

        //
        public AutoBundleData(string folderName, string groupName = "")
        {
            _folderName = folderName;
            _groupName = groupName;
            _labels = new List<string>();
        }
    }
}