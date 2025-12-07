using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
	[CustomPropertyDrawer(typeof(ShowPropertiesAttribute))]
	public sealed class ShowPropertiesDrawer : ScrutableObjectDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);
		}
	}
}
