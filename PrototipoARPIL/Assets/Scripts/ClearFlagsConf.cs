using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFlagsConf : MonoBehaviour {

	Camera arCamera;

	// Use this for initialization
	void Start () {
		arCamera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		arCamera.clearFlags = CameraClearFlags.Depth;
	}
}
