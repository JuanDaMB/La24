// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(Halftone))]
	public class HalftoneEditor : BaseEffectEditor
	{
		private SerializedProperty p_Scale;
		private SerializedProperty p_DotSize;
		private SerializedProperty p_Angle;
		private SerializedProperty p_Smoothness;
		private SerializedProperty p_Center;
		private SerializedProperty p_Desaturate;

		private void OnEnable()
		{
			p_Scale = serializedObject.FindProperty("Scale");
			p_DotSize = serializedObject.FindProperty("DotSize");
			p_Angle = serializedObject.FindProperty("Angle");
			p_Smoothness = serializedObject.FindProperty("Smoothness");
			p_Center = serializedObject.FindProperty("Center");
			p_Desaturate = serializedObject.FindProperty("Desaturate");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Scale);
			EditorGUILayout.PropertyField(p_DotSize);
			EditorGUILayout.PropertyField(p_Smoothness);
			EditorGUILayout.PropertyField(p_Angle);

			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_Center);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(p_Desaturate);

			serializedObject.ApplyModifiedProperties();
		}
	}
}