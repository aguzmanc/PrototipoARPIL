using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportObjectsWithTag : MonoBehaviour {

	public string[] TagsToImport;

	void Start()
	{
		Loader.Instance.OnAllScenesLoaded += ImportObjects;
		//Import();
	}

	void ImportObjects(object source, System.EventArgs args) {
		for (int i = 0; i < TagsToImport.Length; i++) {
			GameObject[] objects = GameObject.FindGameObjectsWithTag(TagsToImport[i]);
			for (int j = 0; j < objects.Length; j++) {
				objects [j].transform.SetParent (transform);
			}
		}
	}

	void Import() {
		for (int i = 0; i < TagsToImport.Length; i++) {
			GameObject[] objects = GameObject.FindGameObjectsWithTag(TagsToImport[i]);
			for (int j = 0; j < objects.Length; j++) {
				objects [j].transform.SetParent (transform);
			}
		}
	}
}
