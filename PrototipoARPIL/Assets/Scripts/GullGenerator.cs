using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullGenerator : MonoBehaviour {

	public Transform Gully;

	Vector3 NW;
	Vector3 SE;
	Vector3 NE;
	Vector3 SW;

	void Start () {
		NW = new Vector3(-40, 30, 40);
		SE = new Vector3(40, 30, -40);
		NE = new Vector3(40, 30, 40);
		SW = new Vector3(-40, 30, -40);
	}
	
	void GenerateGulls() {
		int gullQuantity = Random.Range(3, 9);
		for(int i = 0; i <= gullQuantity; i++) {
			Instantiate(Gully, NW, new Quaternion(0, 0, 0, 0));
		}
	}

	void Update () {
		
	}
}
