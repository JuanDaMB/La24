// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(DoubleVision))]
	public class DoubleVisionEditor : BaseEffectEditor
	{
		private SerializedProperty p_Displace;
		private SerializedProperty p_Amount;

		private void OnEnable()
		{
			p_Displace = serializedObject.FindProperty("Displace");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Displace);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}