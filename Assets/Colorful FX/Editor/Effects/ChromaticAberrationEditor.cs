// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(ChromaticAberration))]
	public class ChromaticAberrationEditor : BaseEffectEditor
	{
		private SerializedProperty p_RedRefraction;
		private SerializedProperty p_GreenRefraction;
		private SerializedProperty p_BlueRefraction;
		private SerializedProperty p_PreserveAlpha;

		private void OnEnable()
		{
			p_RedRefraction = serializedObject.FindProperty("RedRefraction");
			p_GreenRefraction = serializedObject.FindProperty("GreenRefraction");
			p_BlueRefraction = serializedObject.FindProperty("BlueRefraction");
			p_PreserveAlpha = serializedObject.FindProperty("PreserveAlpha");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_RedRefraction);
			EditorGUILayout.PropertyField(p_GreenRefraction);
			EditorGUILayout.PropertyField(p_BlueRefraction);
			EditorGUILayout.PropertyField(p_PreserveAlpha);

			serializedObject.ApplyModifiedProperties();
		}
	}
}