// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(ChannelMixer))]
	public class ChannelMixerEditor : BaseEffectEditor
	{
		private SerializedProperty p_Red;
		private SerializedProperty p_Green;
		private SerializedProperty p_Blue;
		private SerializedProperty p_Constant;

		private SerializedProperty p_CurrentChannel;

		private void OnEnable()
		{
			p_Red = serializedObject.FindProperty("Red");
			p_Green = serializedObject.FindProperty("Green");
			p_Blue = serializedObject.FindProperty("Blue");
			p_Constant = serializedObject.FindProperty("Constant");

			p_CurrentChannel = serializedObject.FindProperty("e_CurrentChannel");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			int currentChannel = p_CurrentChannel.intValue;

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(GetContent("Red"), (currentChannel == 0) ? tabLeftOn : tabLeft)) currentChannel = 0;
			if (GUILayout.Button(GetContent("Green"), (currentChannel == 1) ? tabMiddleOn : tabMiddle)) currentChannel = 1;
			if (GUILayout.Button(GetContent("Blue"), (currentChannel == 2) ? tabRightOn : tabRight)) currentChannel = 2;

			GUILayout.EndHorizontal();

			Vector3 constant = p_Constant.vector3Value;

			if (currentChannel == 0) ChannelUI(p_Red, ref constant.x);
			if (currentChannel == 1) ChannelUI(p_Green, ref constant.y);
			if (currentChannel == 2) ChannelUI(p_Blue, ref constant.z);

			p_Constant.vector3Value = constant;
			p_CurrentChannel.intValue = currentChannel;

			serializedObject.ApplyModifiedProperties();
		}

		private void ChannelUI(SerializedProperty channel, ref float constant)
		{
			Vector3 c = channel.vector3Value;
			c.x = EditorGUILayout.Slider(GetContent("% Red"), c.x, -200f, 200f);
			c.y = EditorGUILayout.Slider(GetContent("% Green"), c.y, -200f, 200f);
			c.z = EditorGUILayout.Slider(GetContent("% Blue"), c.z, -200f, 200f);
			constant = EditorGUILayout.Slider(GetContent("Constant"), constant, -200f, 200f);
			channel.vector3Value = c;
		}
	}
}