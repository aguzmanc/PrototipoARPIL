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

        for(int i=0;i<points.Length;i++)
            Debug.Log(points[i]);

        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

        Vector3[] vs = new Vector3[(points.Length-1)*8];
        int[] ts = new int[(points.Length-1) * 6 * 3];
        Vector3 up = Vector3.up * Height;

        for(int i=0;i<points.Length-1;i++) {
            Vector3 a = points[i], b = points[i+1];
            Vector3 forward = b-a;
            Vector3 right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

            int k=i*8;

            vs[k] = a+right;
            vs[k+1] = a+right+forward;
            vs[k+2] = a-right+forward;
            vs[k+3] = a-right;
            vs[k+4] = a+right+up;
            vs[k+5] = a+right+forward+up;
            vs[k+6] = a-right+forward+up;
            vs[k+7] = a-right+up;

            int[] tmp = new int[] {
                k+6,k+5,k+4,
                k+7,k+6,k+4,
                k+5,k+1,k+0,
                k+0,k+4,k+5,
                k+7,k+3,k+2,
                k+2,k+6,k+7
            };

            //for(int k=0;k<18;k++) 
              //  ts[i*18+k] = tmp[k];

            System.Array.Copy(tmp, 0, ts, i*18, tmp.Length);
        }

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.Clear();

        mesh.vertices = vs;
        mesh.triangles = ts;
    }
}
