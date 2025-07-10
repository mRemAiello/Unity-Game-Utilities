using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    public class CurrencyEditorWindow : GenericAssetEditorWindow<CurrencyData>
    {
        [MenuItem(Constant.MENU_NAME + "Currency Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<CurrencyEditorWindow>();
            window.titleContent = new GUIContent("Currency Editor");
            window.Show();
        }
    }
}