using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour 
{
	[HideInInspector]
	public Path path;

    public GameObject TestPointPrototype;

	public void CreatePath()
	{
		path = new Path (transform.position);
	}


    public void GenerateTestPoints(Vector3[] points)
    {
        for(int i=0;i<points.Length;i++) {
            GameObject obj = Instantiate<GameObject>(TestPointPrototype, points[i], Quaternion.identity, transform);
            obj.name = string.Format("{0:00}", i);

//            GameObject obj = new GameObject(string.Format("{0:00}", i));
//            obj.transform.parent = this.transform;
//            obj.transform.position = points[i];
        }
    }
}
