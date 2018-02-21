using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour 
{
	[HideInInspector]
	public Path path;

    [Range(2, 100)]
    public int PointsPerSegment = 30;
    public GameObject TestPointPrototype;


	public void CreatePath()
	{
		path = new Path (transform.position);
	}

   
    public Vector3[] GetRawPoints()
    {
        return path.GetRawPoints(PointsPerSegment);
    }


    public void GenerateTestPoints()
    {
        Vector3[] points = path.GetRawPoints(PointsPerSegment);

        for(int i=0;i<points.Length;i++) {
            GameObject obj = Instantiate<GameObject>(TestPointPrototype, points[i], Quaternion.identity, transform);
            obj.name = string.Format("{0:00}", i);
        }
    }
}
