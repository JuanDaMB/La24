// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(HueFocus))]
	public class HueFocusEditor : BaseEffectEditor
	{
		private SerializedProperty p_Hue;
		private SerializedProperty p_Range;
		private SerializedProperty p_Boost;
		private SerializedProperty p_Amount;
		private Texture2D m_HueRamp;

		private void OnEnable()
		{
			p_Hue = serializedObject.FindProperty("Hue");
			p_Range = serializedObject.FindProperty("Range");
			p_Boost = serializedObject.FindProperty("Boost");
			p_Amount = serializedObject.FindProperty("Amount");

			m_HueRamp = Resources.Load<Texture2D>("UI/HueRamp");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Separator();

			Rect rect = GUILayoutUtility.GetRect(0, 20);
			GUI.DrawTextureWithTexCoords(rect, m_HueRamp, new Rect(0.5f + p_Hue.floatValue / 360f, 0f, 1f, 1f));

			GUI.enabled = false;
			float min = 180f - p_Range.floatValue;
			float max = 180f + p_Range.floatValue;
			EditorGUILayout.MinMaxSlider(ref min, ref max, 0f, 360f);
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_Hue);
			EditorGUILayout.PropertyField(p_Range);
			EditorGUILayout.PropertyField(p_Boost);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}