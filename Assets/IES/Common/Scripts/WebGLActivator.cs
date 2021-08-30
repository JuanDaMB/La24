using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLActivator : MonoBehaviour
{
	#if UNITY_WEBGL
	private void Start () {
		gameObject.SetActive(false);	
	}
	#endif
}
