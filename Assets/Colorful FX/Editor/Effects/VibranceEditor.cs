// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Vibrance))]
	public class VibranceEditor : BaseEffectEditor
	{
		private SerializedProperty p_Amount;
		private SerializedProperty p_RedChannel;
		private SerializedProperty p_GreenChannel;
		private SerializedProperty p_BlueChannel;
		private SerializedProperty p_AdvancedMode;

		private void OnEnable()
		{
			p_Amount = serializedObject.FindProperty("Amount");
			p_RedChannel = serializedObject.FindProperty("RedChannel");
			p_GreenChannel = serializedObject.FindProperty("GreenChannel");
			p_BlueChannel = serializedObject.FindProperty("BlueChannel");
			p_AdvancedMode = serializedObject.FindProperty("AdvancedMode");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			p_AdvancedMode.boolValue = GUILayout.Toggle(p_AdvancedMode.boolValue, GetContent("Advanced Mode"), EditorStyles.miniButton);

			EditorGUILayout.PropertyField(p_Amount, GetContent("Vibrance"));

			if (p_AdvancedMode.boolValue)
			{
				EditorGUI.indentLevel++;
				{
					EditorGUILayout.PropertyField(p_RedChannel);
					EditorGUILayout.PropertyField(p_GreenChannel);
					EditorGUILayout.PropertyField(p_BlueChannel);
				}
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}