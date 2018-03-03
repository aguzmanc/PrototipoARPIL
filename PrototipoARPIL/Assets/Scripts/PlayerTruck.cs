﻿using System.Collections;
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
	public int baseTurnAngle = 20;

	// Audios
	public GameObject SoundEffectPrototype;
	public AudioClip OilAudio;
	public AudioClip GasAudio;
	public AudioClip MotorAudio;
	public AudioClip MotorNoGasAudio;

	AudioSource _audioSource;
	bool _hasGas = true;

	//Debugging
	float _smallOffset;
	float _currentOffset;
	bool _canChange;

	//Debugging
	public float _speed = 0;
	bool _slowDownFast = false;
	[HideInInspector]
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
		_smallOffset = offset / 10;

		_audioSource = GetComponent<AudioSource> ();
		_audioSource.clip = MotorAudio;
		_audioSource.Play ();
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
		if (_canChange && (Input.GetKeyDown(KeyCode.Space) || DetectTouch())) {
			_canChange = false;
			offset *= -1;
		} else {
			if (offset > 0 && _currentOffset <= offset) { //Player is turning to right lane
				_currentOffset += _smallOffset;
				transform.GetChild(0).transform.localEulerAngles = new Vector3(0, baseTurnAngle - _speed * 1.25f, 0);
				transform.GetChild(1).transform.localEulerAngles = new Vector3(0, baseTurnAngle - _speed * 1.25f, 0);
			}
			else if (offset < 0 && _currentOffset >= offset) { //Player is turning to left lane
				_currentOffset -= _smallOffset;
				transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -baseTurnAngle + _speed * 1.25f, 0);
				transform.GetChild(1).transform.localEulerAngles = new Vector3(0, -baseTurnAngle + _speed * 1.25f, 0);
			}

			transform.GetChild(0).transform.localPosition = new Vector3(_currentOffset, 0.5f, 0);
			transform.GetChild(1).transform.localPosition = new Vector3(_currentOffset, 0.5f, 0);

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

		if (_speed > MinSpeed) {
			if (!_hasGas) {
				_audioSource.clip = MotorAudio;
				_audioSource.Play ();
			}
			_hasGas = true;
		} else {
			if (_hasGas) {
				_audioSource.clip = MotorNoGasAudio;
				_audioSource.Play ();
			}
			_hasGas = false;
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
			Instantiate (SoundEffectPrototype).GetComponent<SoundEffectController> ().Play (OilAudio);
			_slowDownFast = true;
		}
		else if (col.gameObject.CompareTag("Gas")) {
			Instantiate (SoundEffectPrototype).GetComponent<SoundEffectController> ().Play (GasAudio);
			GetComponent<GasController>().Refilling = true;
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

	bool DetectTouch() {
		return Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began;
	}
}
