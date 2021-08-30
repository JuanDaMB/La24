// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(VintageFast))]
	public class VintageFastEditor : BaseEffectEditor
	{
		private SerializedProperty p_Filter;
		private SerializedProperty p_Amount;
		private SerializedProperty p_ForceCompatibility;

		private void OnEnable()
		{
			p_Filter = serializedObject.FindProperty("Filter");
			p_Amount = serializedObject.FindProperty("Amount");
			p_ForceCompatibility = serializedObject.FindProperty("ForceCompatibility");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Filter);
			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_ForceCompatibility);

			serializedObject.ApplyModifiedProperties();
		}
	}
}