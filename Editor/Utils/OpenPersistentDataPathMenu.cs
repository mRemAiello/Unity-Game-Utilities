using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameUtils.Editor
{
    public static class OpenPersistentDataPathMenu
    {
        [MenuItem(GUConstants.MENU_NAME + "Open Persistent Data Path")]
        public static void Open()
        {
            string persistentDataPath = Application.persistentDataPath;

            if (!Directory.Exists(persistentDataPath))
            {
                Directory.CreateDirectory(persistentDataPath);
            }

            EditorUtility.RevealInFinder(persistentDataPath);
        }
    }
}