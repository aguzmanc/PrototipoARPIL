using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

	public Transform Target;
	public float Smoothness = 1f;
	public Vector3 Offset;

	void Update () {
		transform.position = Vector3.Lerp (transform.position, Target.position + Offset, Smoothness * Time.deltaTime);

		transform.LookAt (Target);
	}
}
