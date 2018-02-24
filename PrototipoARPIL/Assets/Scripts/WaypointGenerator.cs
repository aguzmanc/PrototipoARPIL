using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour {

	public PlayerTruck Player;
	Vector3[] _waypointVector;

	void Awake () {
		Player = GameObject.Find ("Player").GetComponent<PlayerTruck> ();
		_waypointVector = new Vector3[] { 
			new Vector3 (19.4f, 0f, 15f),
			new Vector3 (12f, 0f, 2.7f),
			new Vector3 (16.2f, 0f, -11.5f),
			new Vector3 (-5.8f, 0f, -4.3f),
			new Vector3 (-14.5f, 0f, 15f),
			new Vector3 (-14.18f, 0f, 15.84f),
			new Vector3 (-13.62f, 0f, 16.68f),
			new Vector3 (-12.88f, 0f, 17.21f),
			new Vector3 (-11.71f, 0f, 17.82f),
			new Vector3 (-10.6f, 0f, 18.21f)

			//new Vector3 (20, 0, 20),
			//new Vector3 (20, 0, -20),
			//new Vector3 (-20, 0, -20),
			//new Vector3 (-20, 0, 20)
		};
	}

	void Start() {
		Player.WayPoints = _waypointVector;
	}

	public Vector3[] GetPoints()
	{
		return _waypointVector;
	}
}
