using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorFromMarker : MonoBehaviour {

	public Quaternion _defaultRotation;
	public Vector3 _defaultForward;
	public Transform ARMarkerMovable;
	public Transform ARMarkerFixed;
	public float Speed = 0.1f;
	public float Accuracy = 5f;
	public bool IsActive;

	// Use this for initialization
	void Start () {
		_defaultRotation = transform.rotation;
		_defaultForward = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		IsActive = ARMarkerMovable.gameObject.activeInHierarchy;
		if (ARMarkerMovable.gameObject.activeInHierarchy) {
			//float distance = Vector3.Distance (ARMarkerMovable.position, ARMarkerFixed.position);
			Vector3 difference = ARMarkerMovable.position - ARMarkerFixed.position;
			float distance = difference.magnitude;
			int direction = difference.x  > 0 ? 1 : -1;
			Vector3 newRotation = Quaternion.Euler (distance * Accuracy * distance, 0, 0) * _defaultForward;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(newRotation), Time.time * Speed);
		} else {
			transform.rotation = Quaternion.Slerp (transform.rotation, _defaultRotation, Time.time * Speed);
		}
	}
}
