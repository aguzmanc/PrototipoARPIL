using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NormalDetector : MonoBehaviour 
{
	Mesh _mesh;
	[Range(0.1f, 2f)]
	public float LengthNormals = 0.2f;
	public Color ColorNormals = Color.blue;

	void OnEnable()
	{
		_mesh = GetComponentInChildren<MeshFilter> ().sharedMesh;
	}


	void Update () 
	{
		for (int i = 0; i < _mesh.vertices.Length; i++) {
			Vector3 v = transform.TransformPoint (_mesh.vertices [i]);
			Vector3 n = transform.TransformPoint (_mesh.normals [i]);
			Debug.DrawRay (v,n.normalized * LengthNormals, ColorNormals);
		}
	}
}
