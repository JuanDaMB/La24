// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(PixelMatrix))]
	public class PixelMatrixEditor : BaseEffectEditor
	{
		private SerializedProperty p_Size;
		private SerializedProperty p_Brightness;
		private SerializedProperty p_BlackBorder;

		private void OnEnable()
		{
			p_Size = serializedObject.FindProperty("Size");
			p_Brightness = serializedObject.FindProperty("Brightness");
			p_BlackBorder = serializedObject.FindProperty("BlackBorder");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Size);
			EditorGUILayout.PropertyField(p_Brightness);
			EditorGUILayout.PropertyField(p_BlackBorder);

			serializedObject.ApplyModifiedProperties();
		}
	}
}