﻿// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;

	[CustomEditor(typeof(WaveDistortion))]
	public class WaveDistortionEditor : BaseEffectEditor
	{
		private SerializedProperty p_Amplitude;
		private SerializedProperty p_Waves;
		private SerializedProperty p_ColorGlitch;
		private SerializedProperty p_Phase;

		private void OnEnable()
		{
			p_Amplitude = serializedObject.FindProperty("Amplitude");
			p_Waves = serializedObject.FindProperty("Waves");
			p_ColorGlitch = serializedObject.FindProperty("ColorGlitch");
			p_Phase = serializedObject.FindProperty("Phase");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Phase);
			EditorGUILayout.PropertyField(p_Amplitude);
			EditorGUILayout.PropertyField(p_Waves);
			EditorGUILayout.PropertyField(p_ColorGlitch);

			serializedObject.ApplyModifiedProperties();
		}
	}
}