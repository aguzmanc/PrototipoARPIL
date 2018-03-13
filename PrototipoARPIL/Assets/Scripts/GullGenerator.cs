using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullGenerator : MonoBehaviour {

	public Transform Gully;

	//NW - SE - NE -SW
	Vector3[] startSpawnPoints = new[] {
		new Vector3(-40, 30, 40),
		new Vector3(40, 30, -40),
		new Vector3(40, 30, 40),
		new Vector3(-40, 30, -40)
	};
	Vector3 selectedEndSpawnPoint;
	float gullSpeed = 0.1f;
	bool gullsGenerated = true;

	void Start() {
		GenerateGulls();
	}

	void GenerateGulls() {
		//int gullQuantity = Random.Range(3, 9);
		int selectedStartSpawnIndex = Random.Range(0, 4);
		Vector3 selectedStartSpawnPoint = startSpawnPoints[selectedStartSpawnIndex];
		transform.position = selectedStartSpawnPoint;

		var gull1 = Instantiate(Gully, selectedStartSpawnPoint, new Quaternion(0, 0, 0, 0));
		gull1.transform.parent = gameObject.transform;
		var gull2 = Instantiate(Gully, selectedStartSpawnPoint + new Vector3(1, -1, 0), new Quaternion(0, 0, 0, 0));
		gull2.transform.parent = gameObject.transform;
		var gull3 = Instantiate(Gully, selectedStartSpawnPoint + new Vector3(-1, -1, 0), new Quaternion(0, 0, 0, 0));
		gull3.transform.parent = gameObject.transform;
		var gull4 = Instantiate(Gully, selectedStartSpawnPoint + new Vector3(2, -2, 0), new Quaternion(0, 0, 0, 0));
		gull4.transform.parent = gameObject.transform;
		var gull5 = Instantiate(Gully, selectedStartSpawnPoint + new Vector3(-2, -2, 0), new Quaternion(0, 0, 0, 0));
		gull5.transform.parent = gameObject.transform;

		switch (selectedStartSpawnIndex) {
			case 0:
				selectedEndSpawnPoint = startSpawnPoints[1];
				transform.eulerAngles = new Vector3(0, -45, 0);
				break;
			case 1:
				selectedEndSpawnPoint = startSpawnPoints[0];
				transform.eulerAngles = new Vector3(0, 135, 0);
				break;
			case 2:
				selectedEndSpawnPoint = startSpawnPoints[3];
				transform.eulerAngles = new Vector3(0, 45, 0);
				break;
			case 3:
				selectedEndSpawnPoint = startSpawnPoints[2];
				transform.eulerAngles = new Vector3(0, -135, 0);
				break;
		}
	}

	void Update () {
		if (!gullsGenerated) {
			gullsGenerated = true;
			GenerateGulls();
		} else {
			if (Mathf.Abs(transform.position.x) > 40) {
				foreach (Transform child in transform)
					GameObject.Destroy(child.gameObject);
				gullsGenerated = false;
				transform.eulerAngles = Vector3.zero;
			}
			
			transform.position = Vector3.MoveTowards(transform.position, selectedEndSpawnPoint + new Vector3(1,0,1), gullSpeed);
		}
	}
}
