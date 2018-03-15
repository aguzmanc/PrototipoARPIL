using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour {

	public Transform Cloud;
	[Range(0.01f, 0.1f)]
	public float cloudSpeed = 0.02f;

	//NW - SE - NE -SW
	Vector3[] startSpawnPoints = new[] {
		new Vector3(-10, 30, 40),
		new Vector3(10, 30, -40),
		new Vector3(40, 30, 10),
		new Vector3(-40, 30, -10)
	};
	Vector3 selectedEndSpawnPoint;
	bool cloudsGenerated = true;

	void Start() {
		GenerateClouds();
	}

	void GenerateClouds() {
		int selectedStartSpawnIndex = Random.Range(0, 4);
		Vector3 selectedStartSpawnPoint = startSpawnPoints[selectedStartSpawnIndex];
		transform.position = selectedStartSpawnPoint;

		var cloud1 = Instantiate(Cloud, selectedStartSpawnPoint, new Quaternion(0, 0, 0, 0));
		cloud1.transform.parent = gameObject.transform;
		var cloud2 = Instantiate(Cloud, selectedStartSpawnPoint + new Vector3(3, -1, 3), new Quaternion(0, 0, 0, 0));
		cloud2.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
		cloud2.transform.parent = gameObject.transform;
		var cloud3 = Instantiate(Cloud, selectedStartSpawnPoint + new Vector3(-5, -4, 0), new Quaternion(0, 0, 0, 0));
		cloud3.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
		cloud3.transform.parent = gameObject.transform;

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

	void Update() {
		if (!cloudsGenerated) {
			cloudsGenerated = true;
			GenerateClouds();
		} else {
			if (transform.position == selectedEndSpawnPoint) {
				foreach (Transform child in transform)
					GameObject.Destroy(child.gameObject);
				cloudsGenerated = false;
				transform.eulerAngles = Vector3.zero;
			}

			transform.position = Vector3.MoveTowards(transform.position, selectedEndSpawnPoint, cloudSpeed);
		}
	}
}
