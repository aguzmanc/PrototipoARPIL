using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : MonoBehaviour {

	public float MovementSpeed = 3;

	public Vector3[] wayPoints;
	public int currentWayPoint = 0; 
	Vector3 targetWayPoint;

	void Start() {
		//DUMMY wayPoints = GameObject.FindGameObjectWithTag ("Road").GetComponent<WaypointGenerator> ().GetPoints ();
		wayPoints = GameObject.FindGameObjectWithTag("Road").GetComponent<PathCreator>().GetRawPoints();
	}

	void Update () {
		//handleMovementByInput ();

		if (currentWayPoint < this.wayPoints.Length) {
			targetWayPoint = wayPoints [currentWayPoint];
			move ();
		}
	}

	void move(){
		float fixedSpeed = MovementSpeed * 0.1f;

		transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, fixedSpeed);

		//transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint - transform.position, fixedSpeed, 0);
		Vector3 directionOfMovement = targetWayPoint - transform.position;
		if (directionOfMovement != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (directionOfMovement), Time.deltaTime * MovementSpeed);

		if(transform.position == targetWayPoint) {
			currentWayPoint++;

			if (currentWayPoint == this.wayPoints.Length)
				currentWayPoint = 0;

			targetWayPoint = wayPoints[currentWayPoint];
		}
	}

	void handleMovementByInput() {
		float hMovement = Input.GetAxis ("Horizontal");
		float vMovement = Input.GetAxis ("Vertical");

		float fixedSpeed = MovementSpeed * 0.1f;

		transform.Translate (fixedSpeed * hMovement, 0, fixedSpeed * vMovement);
	}
}
