// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(RGBSplit))]
	public class RGBSplitEditor : BaseEffectEditor
	{
		private SerializedProperty p_Amount;
		private SerializedProperty p_Angle;

		private void OnEnable()
		{
			p_Amount = serializedObject.FindProperty("Amount");
			p_Angle = serializedObject.FindProperty("Angle");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_Angle);

			serializedObject.ApplyModifiedProperties();
		}
	}
}