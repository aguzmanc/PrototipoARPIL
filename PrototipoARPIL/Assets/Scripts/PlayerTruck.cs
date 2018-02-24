using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : MonoBehaviour {

	[Range(4,10)]
	public float MaxSpeed = 6;
	[Range(1, 3)]
	public float MinSpeed = 3;
	public float Acceleration = 6;
	public float Deceleration = 30;

	//Debugging
	public float _speed = 0;
	public bool _isInObstacle = false;
	public bool stop = false;

	Vector3[] _wayPoints;
	int _targetWayPointIndex = 0; 
	Vector3 _targetWayPoint;

	void Start() {
		//DUMMY wayPoints = GameObject.FindGameObjectWithTag ("Road").GetComponent<WaypointGenerator> ().GetPoints ();
		_wayPoints = GameObject.FindGameObjectWithTag("Road").GetComponent<PathCreator>().GetRawPoints();
		_targetWayPoint = _wayPoints[_targetWayPointIndex];
	}

	void Update() {
		//HandleMovementByInput ();

		if (_targetWayPointIndex < this._wayPoints.Length - 1) {
			if (transform.position == _targetWayPoint)
				_targetWayPoint = _wayPoints[_targetWayPointIndex++];
			SetSpeed();
			Move ();
		} else {
			_targetWayPointIndex = 0;   
		}
	}

	void SetSpeed() {
		if (stop) {
			if (_speed > 0)
				_speed -= (Deceleration / 5) * Time.deltaTime;
			else
				_speed = 0;
		} else if (!_isInObstacle && !stop) {
			if (_speed < MaxSpeed)
				_speed += Acceleration * Time.deltaTime;
			else
				_speed = MaxSpeed;
		} else if (_isInObstacle) {
			if (_speed > MinSpeed)
				_speed -= Deceleration * Time.deltaTime;
			else
				_speed = MinSpeed;
		}
	}

	void Move() {
		float fixedSpeed = _speed * 0.02f;

		transform.position = Vector3.MoveTowards(transform.position, _targetWayPoint, fixedSpeed);

		//transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint - transform.position, fixedSpeed, 0);
		Vector3 directionOfMovement = _targetWayPoint - transform.position;
		if (directionOfMovement != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (directionOfMovement), Time.deltaTime * MaxSpeed);
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag("Obstacle"))
			_isInObstacle = true;
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.CompareTag("Obstacle"))
			_isInObstacle = false;
	}

	void HandleMovementByInput() {
		float hMovement = Input.GetAxis ("Horizontal");
		float vMovement = Input.GetAxis ("Vertical");

		float fixedSpeed = MaxSpeed * 0.1f;

		transform.Translate (fixedSpeed * hMovement, 0, fixedSpeed * vMovement);
	}
}
