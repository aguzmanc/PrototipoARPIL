using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlowdown : MonoBehaviour {

	GameObject _player;

	void OnTriggerEnter (Collider col) {

		_player = col.gameObject;
		if (_player.CompareTag ("Player"))
            _player.GetComponent<PlayerTruck>().isInObstacle = true;
    }

	void OnTriggerExit(Collider col) {
		_player.GetComponent<PlayerTruck> ().isInObstacle = false;
	}
}
