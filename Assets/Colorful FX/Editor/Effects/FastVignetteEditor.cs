// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using ColorMode = FastVignette.ColorMode;

	[CustomEditor(typeof(FastVignette))]
	public class FastVignetteEditor : BaseEffectEditor
	{
		private SerializedProperty p_Mode;
		private SerializedProperty p_Color;
		private SerializedProperty p_Center;
		private SerializedProperty p_Sharpness;
		private SerializedProperty p_Darkness;

		private void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Color = serializedObject.FindProperty("Color");
			p_Center = serializedObject.FindProperty("Center");
			p_Sharpness = serializedObject.FindProperty("Sharpness");
			p_Darkness = serializedObject.FindProperty("Darkness");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);

			if (p_Mode.intValue == (int)ColorMode.Colored)
				EditorGUILayout.PropertyField(p_Color);

			EditorGUILayout.PropertyField(p_Center);
			EditorGUILayout.PropertyField(p_Sharpness);
			EditorGUILayout.PropertyField(p_Darkness);

			serializedObject.ApplyModifiedProperties();
		}
	}
}