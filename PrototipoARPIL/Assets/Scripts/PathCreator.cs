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


    }

    public void GenerateMesh(float width=1f, float height=0.1f)
    {
        Vector3[] points = path.GetRawPoints(PointsPerSegment);

        for(int i=0;i<points.Length;i++)
            Debug.Log(points[i]);

        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

        Vector3[] vs = new Vector3[(points.Length-1)*8];
        int[] ts = new int[(points.Length-1) * 6 * 3];
        Vector3 up = Vector3.up * height;

        for(int i=2;i<3;i++) {
            Vector3 a = points[i], b = points[i+1];
            Vector3 forward = b-a;
            Vector3 right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * width;

            vs[i] = a+right;
            vs[i+1] = a+right+forward;
            vs[i+2] = a-right+forward;
            vs[i+3] = a-right;
            vs[i+4] = a+right+up;
            vs[i+5] = a+right+forward+up;
            vs[i+6] = a-right+forward+up;
            vs[i+7] = a-right+up;

            int[] tmp = new int[] {
                i+6,i+5,i+4,
                i+7,i+6,i+4,
                i+5,i+1,i+0,
                i+0,i+4,i+5,
                i+7,i+3,i+2,
                i+2,i+6,i+7
            };

            for(int k=0;k<18;k++) 
                ts[i*18+k] = tmp[k];

            //System.Array.Copy(tmp, 0, ts, i*18, tmp.Length);
        }

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.Clear();

        mesh.vertices = vs;
        mesh.triangles = ts;
    }
}
