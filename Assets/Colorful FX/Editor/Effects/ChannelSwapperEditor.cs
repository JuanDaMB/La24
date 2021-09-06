// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(ChannelSwapper))]
	public class ChannelSwapperEditor : BaseEffectEditor
	{
		private SerializedProperty p_RedSource;
		private SerializedProperty p_GreenSource;
		private SerializedProperty p_BlueSource;

		private void OnEnable()
		{
			p_RedSource = serializedObject.FindProperty("RedSource");
			p_GreenSource = serializedObject.FindProperty("GreenSource");
			p_BlueSource = serializedObject.FindProperty("BlueSource");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_RedSource);
			EditorGUILayout.PropertyField(p_GreenSource);
			EditorGUILayout.PropertyField(p_BlueSource);

			serializedObject.ApplyModifiedProperties();
		}
	}
}