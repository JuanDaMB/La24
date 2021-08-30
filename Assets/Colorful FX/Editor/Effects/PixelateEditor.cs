// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(Pixelate))]
	public class PixelateEditor : BaseEffectEditor
	{
		private SerializedProperty p_Scale;
		private SerializedProperty p_Ratio;
		private SerializedProperty p_AutomaticRatio;
		private SerializedProperty p_Mode;

		private void OnEnable()
		{
			p_Scale = serializedObject.FindProperty("Scale");
			p_Ratio = serializedObject.FindProperty("Ratio");
			p_AutomaticRatio = serializedObject.FindProperty("AutomaticRatio");
			p_Mode = serializedObject.FindProperty("Mode");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_Scale);
			EditorGUILayout.PropertyField(p_AutomaticRatio);

			if (!p_AutomaticRatio.boolValue)
			{
				EditorGUI.indentLevel++;
				{
					EditorGUILayout.PropertyField(p_Ratio);
				}
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}