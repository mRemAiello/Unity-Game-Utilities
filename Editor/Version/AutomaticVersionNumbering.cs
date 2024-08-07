using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace UnityEditor.GameUtils
{
    [InitializeOnLoad]
    public static class AutomaticVersionNumbering
    {
        private const string AUTO_VERSION_MENU_ITEM = "Window/Game Utils/Toggle Version Numbering";
        private const string IS_ENABLED_KEY = "automatic_version_numbering:is_enabled";

        //
        private static bool _isEnabled;

        static AutomaticVersionNumbering()
        {
            // 
            EditorApplication.delayCall += OnSetup;
        }

        private static void OnSetup()
        {
            EditorApplication.delayCall -= OnSetup;

            // 
            _isEnabled = EditorPrefs.GetBool(IS_ENABLED_KEY, false);

            // 
            Menu.SetChecked(AUTO_VERSION_MENU_ITEM, _isEnabled);
        }

        [PostProcessBuild]
        private static void IncrementVersionNumber(BuildTarget target, string path)
        {
            // 
            if (!_isEnabled)
                return;

            string newVersion = ConstructNewVersionString(PlayerSettings.bundleVersion);

            // 
            if (string.IsNullOrEmpty(newVersion))
            {
                string var = "Version number was not automatically increased because no numbers were detected in the current version: {0}.";
                Debug.LogWarning(string.Format(var, PlayerSettings.bundleVersion));
                return;
            }

            // 
            PlayerSettings.bundleVersion = newVersion;
        }

        [MenuItem(AUTO_VERSION_MENU_ITEM)]
        private static void Toggle()
        {
            // 
            _isEnabled = !_isEnabled;

            // 
            EditorPrefs.SetBool(IS_ENABLED_KEY, _isEnabled);

            // 
            Menu.SetChecked(AUTO_VERSION_MENU_ITEM, _isEnabled);

            //
            if (_isEnabled)
                Debug.Log("Auto Versione enabled.");
            else
                Debug.Log("Auto version disabled.");     
        }

        private static string ConstructNewVersionString(string oldVersionString)
        {
            int lastIndex = -1;
            int firstIndex = -1;
            char[] characters = oldVersionString.ToCharArray();

            // find the first and last index of the last whole number in the version string
            for (int i = characters.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(characters[i]))
                {
                    if (lastIndex < 0)
                        lastIndex = i;

                    if (firstIndex < 0 || firstIndex > i)
                        firstIndex = i;
                }
                else if (lastIndex > 0 && firstIndex > 0)
                    break;
            }

            // version contains no numbers, so we return null
            if (firstIndex < 0 || lastIndex < 0)
                return null;

            int length = lastIndex - firstIndex + 1;
            int originalNumber = int.Parse(oldVersionString.Substring(firstIndex, length));
            int newNumber = originalNumber + 1;

            // replace the old number with the new one
            string newVersionString = oldVersionString.Remove(firstIndex, length);
            newVersionString = newVersionString.Insert(firstIndex, newNumber.ToString());

            // return the new version
            return newVersionString;
        }
    }
}
