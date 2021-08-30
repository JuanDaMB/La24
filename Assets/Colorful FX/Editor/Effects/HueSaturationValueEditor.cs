// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(HueSaturationValue))]
	public class HueSaturationValueEditor : BaseEffectEditor
	{
		private static string[] channels = { "Master", "Reds", "Yellows", "Greens", "Cyans", "Blues", "Magentas" };

		private SerializedProperty p_MasterHue;
		private SerializedProperty p_MasterSaturation;
		private SerializedProperty p_MasterValue;

		private SerializedProperty p_RedsHue;
		private SerializedProperty p_RedsSaturation;
		private SerializedProperty p_RedsValue;

		private SerializedProperty p_YellowsHue;
		private SerializedProperty p_YellowsSaturation;
		private SerializedProperty p_YellowsValue;

		private SerializedProperty p_GreensHue;
		private SerializedProperty p_GreensSaturation;
		private SerializedProperty p_GreensValue;

		private SerializedProperty p_CyansHue;
		private SerializedProperty p_CyansSaturation;
		private SerializedProperty p_CyansValue;

		private SerializedProperty p_BluesHue;
		private SerializedProperty p_BluesSaturation;
		private SerializedProperty p_BluesValue;

		private SerializedProperty p_MagentasHue;
		private SerializedProperty p_MagentasSaturation;
		private SerializedProperty p_MagentasValue;

		private SerializedProperty p_AdvancedMode;
		private SerializedProperty p_CurrentChannel;

		private void OnEnable()
		{
			p_MasterHue = serializedObject.FindProperty("MasterHue");
			p_MasterSaturation = serializedObject.FindProperty("MasterSaturation");
			p_MasterValue = serializedObject.FindProperty("MasterValue");

			p_RedsHue = serializedObject.FindProperty("RedsHue");
			p_RedsSaturation = serializedObject.FindProperty("RedsSaturation");
			p_RedsValue = serializedObject.FindProperty("RedsValue");

			p_YellowsHue = serializedObject.FindProperty("YellowsHue");
			p_YellowsSaturation = serializedObject.FindProperty("YellowsSaturation");
			p_YellowsValue = serializedObject.FindProperty("YellowsValue");

			p_GreensHue = serializedObject.FindProperty("GreensHue");
			p_GreensSaturation = serializedObject.FindProperty("GreensSaturation");
			p_GreensValue = serializedObject.FindProperty("GreensValue");

			p_CyansHue = serializedObject.FindProperty("CyansHue");
			p_CyansSaturation = serializedObject.FindProperty("CyansSaturation");
			p_CyansValue = serializedObject.FindProperty("CyansValue");

			p_BluesHue = serializedObject.FindProperty("BluesHue");
			p_BluesSaturation = serializedObject.FindProperty("BluesSaturation");
			p_BluesValue = serializedObject.FindProperty("BluesValue");

			p_MagentasHue = serializedObject.FindProperty("MagentasHue");
			p_MagentasSaturation = serializedObject.FindProperty("MagentasSaturation");
			p_MagentasValue = serializedObject.FindProperty("MagentasValue");

			p_AdvancedMode = serializedObject.FindProperty("AdvancedMode");
			p_CurrentChannel = serializedObject.FindProperty("e_CurrentChannel");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			bool advanced = p_AdvancedMode.boolValue;
			int channel = p_CurrentChannel.intValue;

			EditorGUILayout.BeginHorizontal();
			if (advanced) channel = EditorGUILayout.Popup(channel, channels);
			else channel = 0;

			advanced = GUILayout.Toggle(advanced, GetContent("Advanced Mode"), EditorStyles.miniButton);
			EditorGUILayout.EndHorizontal();

			switch (channel)
			{
				case 1:
					Channel(p_RedsHue, p_RedsSaturation, p_RedsValue);
					break;

				case 2:
					Channel(p_YellowsHue, p_YellowsSaturation, p_YellowsValue);
					break;

				case 3:
					Channel(p_GreensHue, p_GreensSaturation, p_GreensValue);
					break;

				case 4:
					Channel(p_CyansHue, p_CyansSaturation, p_CyansValue);
					break;

				case 5:
					Channel(p_BluesHue, p_BluesSaturation, p_BluesValue);
					break;

				case 6:
					Channel(p_MagentasHue, p_MagentasSaturation, p_MagentasValue);
					break;

				default:
					Channel(p_MasterHue, p_MasterSaturation, p_MasterValue);
					break;
			}

			p_AdvancedMode.boolValue = advanced;
			p_CurrentChannel.intValue = channel;

			serializedObject.ApplyModifiedProperties();
		}

		private void Channel(SerializedProperty hue, SerializedProperty saturation, SerializedProperty value)
		{
			EditorGUILayout.PropertyField(hue, GetContent("Hue"));
			EditorGUILayout.PropertyField(saturation, GetContent("Saturation"));
			EditorGUILayout.PropertyField(value, GetContent("Value"));
		}
	}
}