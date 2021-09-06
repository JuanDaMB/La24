// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(Strokes))]
	public class StrokesEditor : BaseEffectEditor
	{
		private SerializedProperty p_Mode;
		private SerializedProperty p_Amplitude;
		private SerializedProperty p_Frequency;
		private SerializedProperty p_Scaling;
		private SerializedProperty p_MaxThickness;
		private SerializedProperty p_RedLuminance;
		private SerializedProperty p_GreenLuminance;
		private SerializedProperty p_BlueLuminance;
		private SerializedProperty p_Threshold;
		private SerializedProperty p_Harshness;

		private void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Amplitude = serializedObject.FindProperty("Amplitude");
			p_Frequency = serializedObject.FindProperty("Frequency");
			p_Scaling = serializedObject.FindProperty("Scaling");
			p_MaxThickness = serializedObject.FindProperty("MaxThickness");
			p_RedLuminance = serializedObject.FindProperty("RedLuminance");
			p_GreenLuminance = serializedObject.FindProperty("GreenLuminance");
			p_BlueLuminance = serializedObject.FindProperty("BlueLuminance");
			p_Threshold = serializedObject.FindProperty("Threshold");
			p_Harshness = serializedObject.FindProperty("Harshness");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(p_Amplitude);
			EditorGUILayout.PropertyField(p_Frequency);
			EditorGUILayout.PropertyField(p_Scaling);
			EditorGUILayout.PropertyField(p_MaxThickness);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(p_Threshold);
			EditorGUILayout.PropertyField(p_Harshness);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField(GetContent("Contribution"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_RedLuminance, GetContent("Red"));
				EditorGUILayout.PropertyField(p_GreenLuminance, GetContent("Green"));
				EditorGUILayout.PropertyField(p_BlueLuminance, GetContent("Blue"));
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedProperties();
		}
	}
}