// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(Noise))]
	public class NoiseEditor : BaseEffectEditor
	{
		private SerializedProperty p_Mode;
		private SerializedProperty p_Animate;
		private SerializedProperty p_Seed;
		private SerializedProperty p_Strength;
		private SerializedProperty p_LumContribution;

		private void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Animate = serializedObject.FindProperty("Animate");
			p_Seed = serializedObject.FindProperty("Seed");
			p_Strength = serializedObject.FindProperty("Strength");
			p_LumContribution = serializedObject.FindProperty("LumContribution");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_Animate);

			EditorGUI.BeginDisabledGroup(p_Animate.boolValue);
			{
				EditorGUILayout.PropertyField(p_Seed);
			}
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(p_Strength);
			EditorGUILayout.PropertyField(p_LumContribution, GetContent("Luminance Contribution"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}