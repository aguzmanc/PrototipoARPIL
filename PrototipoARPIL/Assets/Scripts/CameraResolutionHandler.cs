using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
			jo.Call ("launchPreferencesActivity");
		}
		#endif
	}
}
