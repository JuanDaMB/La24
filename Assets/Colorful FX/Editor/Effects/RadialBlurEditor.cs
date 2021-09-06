// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(RadialBlur))]
	public class RadialBlurEditor : BaseEffectEditor
	{
		private SerializedProperty p_Strength;
		private SerializedProperty p_Samples;
		private SerializedProperty p_Center;
		private SerializedProperty p_Quality;
		private SerializedProperty p_Sharpness;
		private SerializedProperty p_Darkness;
		private SerializedProperty p_EnableVignette;

		private void OnEnable()
		{
			p_Strength = serializedObject.FindProperty("Strength");
			p_Samples = serializedObject.FindProperty("Samples");
			p_Center = serializedObject.FindProperty("Center");
			p_Quality = serializedObject.FindProperty("Quality");
			p_Sharpness = serializedObject.FindProperty("Sharpness");
			p_Darkness = serializedObject.FindProperty("Darkness");
			p_EnableVignette = serializedObject.FindProperty("EnableVignette");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Quality);

			if (p_Quality.intValue == (int)RadialBlur.QualityPreset.Custom)
				EditorGUILayout.PropertyField(p_Samples);

			EditorGUILayout.PropertyField(p_Strength);
			EditorGUILayout.PropertyField(p_Center, GetContent("Center Point"));
			EditorGUILayout.PropertyField(p_EnableVignette, GetContent("Vignette"));

			if (p_EnableVignette.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(p_Sharpness);
				EditorGUILayout.PropertyField(p_Darkness);
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}