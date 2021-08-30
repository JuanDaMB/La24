// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(BilateralGaussianBlur))]
	public class BilateralGaussianBlurEditor : BaseEffectEditor
	{
		private SerializedProperty p_Passes;
		private SerializedProperty p_Threshold;
		private SerializedProperty p_Amount;

		private void OnEnable()
		{
			p_Passes = serializedObject.FindProperty("Passes");
			p_Threshold = serializedObject.FindProperty("Threshold");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Passes);
			EditorGUILayout.PropertyField(p_Threshold);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}