using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : MonoBehaviour {

	public float MovementSpeed = 3;

	public Transform[] wayPoints;
	public int currentWayPoint = 0; 
	Transform targetWayPoint;

	void Update () {
		//handleMovementByInput ();

		if (currentWayPoint < this.wayPoints.Length) {
			if (targetWayPoint == null)
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

		transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, fixedSpeed, 0);
		transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, fixedSpeed);

		if(transform.position == targetWayPoint.position) {
			currentWayPoint++;

			if (currentWayPoint == this.wayPoints.Length)
				currentWayPoint = 0;

			targetWayPoint = wayPoints[currentWayPoint];
		}
	} 
}
