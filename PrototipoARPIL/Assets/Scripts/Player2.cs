using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {
	public float RotationSpeed = 2;
	public float MovementSpeed = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float hMovement = Input.GetAxis ("Horizontal");
		float vMovement = Input.GetAxis ("Vertical");
		float hRotation = Input.GetAxis ("Mouse X");

		transform.Translate (MovementSpeed * hMovement, 0, MovementSpeed * vMovement);
		transform.Rotate (0, RotationSpeed * hRotation, 0);
		
	}
}
