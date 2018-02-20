using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : MonoBehaviour {

	public float MovementSpeed = 3;

	public Vector3[] wayPoints;
	public int currentWayPoint = 0; 
	Vector3 targetWayPoint;

	void Start() {
		wayPoints = GameObject.Find ("Road").GetComponent<WaypointGenerator> ().GetPoints ();
	}

	void Update () {
		//handleMovementByInput ();

		if (currentWayPoint < this.wayPoints.Length) {
			targetWayPoint = wayPoints [currentWayPoint];
			move ();
		}
	}

	void handleMovementByInput() {
		float hMovement = Input.GetAxis ("Horizontal");
		float vMovement = Input.GetAxis ("Vertical");

		float fixedSpeed = MovementSpeed * 0.1f;

		transform.Translate (fixedSpeed * hMovement, 0, fixedSpeed * vMovement);
	}

	void move(){
		float fixedSpeed = MovementSpeed * 0.1f;

		transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint - transform.position, fixedSpeed, 0);
		transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, fixedSpeed);

		if(transform.position == targetWayPoint) {
			currentWayPoint++;

			if (currentWayPoint == this.wayPoints.Length)
				currentWayPoint = 0;

			targetWayPoint = wayPoints[currentWayPoint];
		}
	} 
}
