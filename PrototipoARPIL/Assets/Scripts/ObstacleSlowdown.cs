using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlowdown : MonoBehaviour {

	[Range(5,10)]
	public float SlowDownFactor = 5;

	GameObject _player;
	float _playerOriginalSpeed;

	void OnTriggerEnter (Collider col) {
		if (SlowDownFactor == 0)
			return;

		_player = col.gameObject;
		if (_player.CompareTag ("Player")) {
			_playerOriginalSpeed = _player.GetComponent<PlayerTruck> ().MovementSpeed;

			_player.GetComponent<PlayerTruck> ().MovementSpeed /= SlowDownFactor;
		}
	}

	void OnTriggerExit(Collider col) {
		_player.GetComponent<PlayerTruck> ().MovementSpeed = _playerOriginalSpeed;
	}
}
