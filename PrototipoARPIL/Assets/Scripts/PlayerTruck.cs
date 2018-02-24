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
	[Range(1, 3)]
	public float offset = 1.5f;

	//Debugging
	public float _speed = 0;
	public bool _slowDownFast = false;
	public bool _slowDown = false;

	Vector3[] _wayPoints;
	int _targetWayPointIndex = 0; 
	Vector3 _targetWayPoint;

	void Start() {
		//DUMMY wayPoints = GameObject.FindGameObjectWithTag ("Road").GetComponent<WaypointGenerator> ().GetPoints ();
		_wayPoints = GameObject.FindGameObjectWithTag("Road").GetComponent<PathCreator>().GetRawPoints();
		_targetWayPoint = _wayPoints[_targetWayPointIndex];

		transform.GetChild(0).transform.localPosition = new Vector3(offset, 0.5f, 0);
		transform.GetChild(1).transform.localPosition = new Vector3(offset, 0.5f, 0);

	}

	void Update() {
		//HandleMovementByInput ();

		HandleOffset();
		if (_targetWayPointIndex < this._wayPoints.Length - 1) {
			if (transform.position == _targetWayPoint)
				_targetWayPoint = _wayPoints[_targetWayPointIndex++];
			SetSpeed();
			Move ();
		} else {
			_targetWayPointIndex = 0;
		}
	}

	void HandleOffset() {
		//Input.touches[0].fingerId == 0 || 
		if (Input.GetKeyDown(KeyCode.Space)) {
			offset *= -1;
			transform.GetChild(0).transform.localPosition = new Vector3(offset, 0.5f, 0);
			transform.GetChild(1).transform.localPosition = new Vector3(offset, 0.5f, 0);
		}
	}

	void SetSpeed() {
		if (_slowDownFast) {
			if (_speed > MinSpeed)
				_speed -= Deceleration * Time.deltaTime;
			else
				_speed = MinSpeed;
		} else if (_slowDown) {
			if (_speed > MinSpeed)
				_speed -= (Deceleration / 5) * Time.deltaTime;
			else
				_speed = MinSpeed;
		} else {
			if (_speed < MaxSpeed)
				_speed += Acceleration * Time.deltaTime;
			else
				_speed = MaxSpeed;
		}
	}

	void Move() {
		float fixedSpeed = _speed * 0.02f;

		transform.position = Vector3.MoveTowards(transform.position, _targetWayPoint, fixedSpeed);

		//transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint - transform.position, fixedSpeed, 0);
		Vector3 directionOfMovement = _targetWayPoint - transform.position;
		if (directionOfMovement != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (directionOfMovement), Time.deltaTime * MaxSpeed/2);
	}

	public void OnTriggerEnterChild(Collider col) {
		if (col.gameObject.CompareTag("Obstacle"))
			_slowDownFast = true;
		else if (col.gameObject.CompareTag("Gas")) {
			GetComponent<GasController>()._gasQuantity = 100;
			_slowDown = false;
		}
	}

	public void OnTriggerExitChild(Collider col) {
		if (col.gameObject.CompareTag("Obstacle"))
			_slowDownFast = false;
	}

	void HandleMovementByInput() {
		float hMovement = Input.GetAxis ("Horizontal");
		float vMovement = Input.GetAxis ("Vertical");

		float fixedSpeed = MaxSpeed * 0.1f;

		transform.Translate (fixedSpeed * hMovement, 0, fixedSpeed * vMovement);
	}
}
