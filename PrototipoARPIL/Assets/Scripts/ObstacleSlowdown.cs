using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlowdown : MonoBehaviour {

	void OnTriggerEnter (Collider col) {
		//enemysgameobject.speed *=
		//set timeout
		Destroy (gameObject);
	}
}
