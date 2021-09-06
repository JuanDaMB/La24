// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(Wiggle))]
	public class WiggleEditor : BaseEffectEditor
	{
		private SerializedProperty p_Mode;
		private SerializedProperty p_Timer;
		private SerializedProperty p_Speed;
		private SerializedProperty p_Frequency;
		private SerializedProperty p_Amplitude;
		private SerializedProperty p_AutomaticTimer;

		private void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Timer = serializedObject.FindProperty("Timer");
			p_Speed = serializedObject.FindProperty("Speed");
			p_Frequency = serializedObject.FindProperty("Frequency");
			p_Amplitude = serializedObject.FindProperty("Amplitude");
			p_AutomaticTimer = serializedObject.FindProperty("AutomaticTimer");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_AutomaticTimer);

			if (p_AutomaticTimer.boolValue)
				EditorGUILayout.PropertyField(p_Speed);
			else
				EditorGUILayout.PropertyField(p_Timer);

			EditorGUILayout.PropertyField(p_Frequency);
			EditorGUILayout.PropertyField(p_Amplitude);

			serializedObject.ApplyModifiedProperties();
		}
	}
}