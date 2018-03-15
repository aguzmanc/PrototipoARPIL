using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : MonoBehaviour {

	[Range(3.1f, 6)]
	public float MaxSpeed = 5;
	[Range(0.2f, 3)]
	public float MinSpeed = 2;
	public float Acceleration = 3;
	public float Deceleration = 30;
	[Range(0.5f, 2)]
	public float offset = 1;
	[Range(10, 25)]
	public float laneChangingSpeed = 20;

	bool _hasGas = true;

	//Debugging
	float _smallOffset;
	float _currentOffset;
	bool _canChange;

	//Debugging
	float _speed = 0;
	bool _slowDownFast = false;
	[HideInInspector]
	public bool _slowDown = false;

	Vector3[] _wayPoints;
	int _targetWayPointIndex = 0; 
	Vector3 _targetWayPoint;

	public System.EventHandler OnGasRefill;
	public System.EventHandler OnOilSlide;
	public System.EventHandler OnGoalReached;

	void Start() {
		//DUMMY wayPoints = GameObject.FindGameObjectWithTag ("Road").GetComponent<WaypointGenerator> ().GetPoints ();
		_wayPoints = GameObject.FindGameObjectWithTag("Road").GetComponent<PathCreator>().GetRawPoints();
		_targetWayPoint = _wayPoints[_targetWayPointIndex];

		transform.GetChild(0).transform.localPosition = new Vector3(offset, 0.5f, 0);
		transform.GetChild(1).transform.localPosition = new Vector3(offset, 0.5f, 0);
		_smallOffset = offset / laneChangingSpeed;
	}

	void FixedUpdate() {
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
		if (_canChange && (Input.GetKeyDown(KeyCode.Space) || DetectTouch())) {
			_canChange = false;
			offset *= -1;
		} else {
			if (offset > 0 && _currentOffset <= offset) { //Player is turning to right lane
				_currentOffset += _smallOffset;
				transform.GetChild(0).transform.localEulerAngles = new Vector3(0, laneChangingSpeed - _speed * 1.25f, 0);
				transform.GetChild(1).transform.localEulerAngles = new Vector3(0, laneChangingSpeed - _speed * 1.25f, 0);
			}
			else if (offset < 0 && _currentOffset >= offset) { //Player is turning to left lane
				_currentOffset -= _smallOffset;
				transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -laneChangingSpeed + _speed * 1.25f, 0);
				transform.GetChild(1).transform.localEulerAngles = new Vector3(0, -laneChangingSpeed + _speed * 1.25f, 0);
			}

			transform.GetChild(0).transform.localPosition = new Vector3(_currentOffset, 0.1f, 0);
			transform.GetChild(1).transform.localPosition = new Vector3(_currentOffset, 0.1f, 0);

			if ((offset > 0 && _currentOffset >= offset) || (offset < 0 && _currentOffset <= offset)) {
				_canChange = true;
				transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
				transform.GetChild(1).transform.localEulerAngles = new Vector3(0, 0, 0);
			}
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
		if (col.gameObject.CompareTag ("Obstacle")) {
			if (OnOilSlide != null)
				OnOilSlide (this, System.EventArgs.Empty);
			_slowDownFast = true;
			col.gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
		} else if (col.gameObject.CompareTag ("Gas")) {
			if (OnGasRefill != null)
				OnGasRefill (this, System.EventArgs.Empty);
			GetComponent<GasController> ().Refilling = true;
			col.gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
			_slowDown = false;
		} else if (col.gameObject.CompareTag ("Goal")) {
			OnGoalReached (this, System.EventArgs.Empty);
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

	bool DetectTouch() {
		return Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began;
	}
}
