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
    public float Width = 0.2f;
    public float Height = 0.1f;


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
    }

    public void GenerateMesh()
    {
        Vector3[] points = path.GetRawPoints(PointsPerSegment);

        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

        Vector3[] vs = new Vector3[(points.Length-1)*8];
        int[] ts = new int[(points.Length-1) * 6 * 3];

        Vector3 up = Vector3.up * Height;

        int k;
        Vector3 a=Vector3.zero, b=Vector3.zero, forward=Vector3.zero, right=Vector3.zero;
        for(int i=0;i<points.Length-1;i++) {
            a = points[i];
            b = points[i+1];

            forward = b-a;
            right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

            k=i*4;

            vs[k] = a+right;
            vs[k+1] = a+right+up;
            vs[k+2] = a-right+up;
            vs[k+3] = a-right;

            int[] tmp = new int[] {
                k,  k+1,k+4,
                k+1,k+5,k+4,
                k+1,k+2,k+5,
                k+2,k+6,k+5 ,
                k+3,k+7,k+6,
                k+3,k+6,k+2
            };

            System.Array.Copy(tmp, 0, ts, i*18, tmp.Length);
        }

        // Last segment
        /*
        a = points[points.Length-1];
        b = points[points.Length-2];
        forward = a-b;
        right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;
        k = (points.Length-1) * 4;
        vs[k] =   a+right;
        vs[k+1] = a+right+up;
        vs[k+2] = a-right+up;
        vs[k+3] = a-right;
        */

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.Clear();

        mesh.vertices = vs;
        mesh.triangles = ts;
    }
}
