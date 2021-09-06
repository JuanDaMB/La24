// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Glitch))]
	public class GlitchEditor : BaseEffectEditor
	{
		private SerializedProperty p_RandomActivation;
		private SerializedProperty p_RandomEvery;
		private SerializedProperty p_RandomDuration;

		private SerializedProperty p_Mode;
		private SerializedProperty p_InterferencesSettings;
		private SerializedProperty p_TearingSettings;

		private SerializedProperty p_InterferencesSpeed;
		private SerializedProperty p_InterferencesDensity;
		private SerializedProperty p_InterferencesMaxDisplacement;

		private SerializedProperty p_TearingSpeed;
		private SerializedProperty p_TearingIntensity;
		private SerializedProperty p_TearingMaxDisplacement;
		private SerializedProperty p_TearingAllowFlipping;
		private SerializedProperty p_TearingYuvColorBleeding;
		private SerializedProperty p_TearingYuvOffset;

		private void OnEnable()
		{
			p_RandomActivation = serializedObject.FindProperty("RandomActivation");
			p_RandomEvery = serializedObject.FindProperty("RandomEvery");
			p_RandomDuration = serializedObject.FindProperty("RandomDuration");

			p_Mode = serializedObject.FindProperty("Mode");
			p_InterferencesSettings = serializedObject.FindProperty("SettingsInterferences");
			p_TearingSettings = serializedObject.FindProperty("SettingsTearing");

			p_InterferencesSpeed = p_InterferencesSettings.FindPropertyRelative("Speed");
			p_InterferencesDensity = p_InterferencesSettings.FindPropertyRelative("Density");
			p_InterferencesMaxDisplacement = p_InterferencesSettings.FindPropertyRelative("MaxDisplacement");

			p_TearingSpeed = p_TearingSettings.FindPropertyRelative("Speed");
			p_TearingIntensity = p_TearingSettings.FindPropertyRelative("Intensity");
			p_TearingMaxDisplacement = p_TearingSettings.FindPropertyRelative("MaxDisplacement");
			p_TearingAllowFlipping = p_TearingSettings.FindPropertyRelative("AllowFlipping");
			p_TearingYuvColorBleeding = p_TearingSettings.FindPropertyRelative("YuvColorBleeding");
			p_TearingYuvOffset = p_TearingSettings.FindPropertyRelative("YuvOffset");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_RandomActivation);

			if (p_RandomActivation.boolValue)
			{
				DoTimingUI(p_RandomEvery, GetContent("Every"), 50f);
				DoTimingUI(p_RandomDuration, GetContent("For"), 50f);
				EditorGUILayout.Space();
			}

			EditorGUILayout.PropertyField(p_Mode);

			if (p_Mode.enumValueIndex == (int)Glitch.GlitchingMode.Interferences)
			{
				DoInterferencesUI();
			}
			else if (p_Mode.enumValueIndex == (int)Glitch.GlitchingMode.Tearing)
			{
				DoTearingUI();
			}
			else // Complete
			{
				EditorGUILayout.LabelField(GetContent("Interferences"), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				{
					DoInterferencesUI();
				}
				EditorGUI.indentLevel--;

				EditorGUILayout.Space();
				EditorGUILayout.LabelField(GetContent("Tearing"), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				{
					DoTearingUI();
				}
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void DoInterferencesUI()
		{
			EditorGUILayout.PropertyField(p_InterferencesSpeed);
			EditorGUILayout.PropertyField(p_InterferencesDensity);
			EditorGUILayout.PropertyField(p_InterferencesMaxDisplacement);
		}

		private void DoTearingUI()
		{
			EditorGUILayout.PropertyField(p_TearingSpeed);
			EditorGUILayout.PropertyField(p_TearingIntensity);
			EditorGUILayout.PropertyField(p_TearingMaxDisplacement);
			EditorGUILayout.PropertyField(p_TearingYuvColorBleeding, GetContent("YUV Color Bleeding"));

			if (p_TearingYuvColorBleeding.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(p_TearingYuvOffset, GetContent("Offset"));
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(p_TearingAllowFlipping);
		}

		private void DoTimingUI(SerializedProperty prop, GUIContent label, float labelWidth)
		{
			Vector2 v = prop.vector2Value;

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(EditorGUIUtility.labelWidth - 3);
				GUILayout.Label(label, GUILayout.ExpandWidth(false), GUILayout.Width(labelWidth));
				v.x = EditorGUILayout.FloatField(v.x, GUILayout.MaxWidth(75));
				GUILayout.Label(GetContent("to"), GUILayout.ExpandWidth(false));
				v.y = EditorGUILayout.FloatField(v.y, GUILayout.MaxWidth(75));
				GUILayout.Label(GetContent("second(s)"), GUILayout.ExpandWidth(false));
			}
			EditorGUILayout.EndHorizontal();

			prop.vector2Value = v;
		}
	}
}