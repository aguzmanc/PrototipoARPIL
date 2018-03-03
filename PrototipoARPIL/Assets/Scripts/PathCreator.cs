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

 
    public Vector3[] ALLPOINTS;

    public void GenerateMesh()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

        Vector3[] points = path.GetRawPoints(PointsPerSegment);
        Debug.Log(points.Length);
        ALLPOINTS = points;

        Vector3 up = Vector3.up * Height;
        List<Vector3> vs = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        Vector3 a=Vector3.zero, b=Vector3.zero, forward=Vector3.zero, right=Vector3.zero;

        // generate vertices
        for(int i=0;i<points.Length;i++) {
            if(i<points.Length-2) { // for all the points
                a = points[i];
                b = points[i+1];

                forward = b-a;
                right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

                vs.Add(a+right);
                vs.Add(a+right+up);
                vs.Add(a-right+up);
                vs.Add(a-right);
            } else { // for the last point
                a = points[i-1];
                b = points[i];

                forward = b-a;
                right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

                vs.Add(b+right);
                vs.Add(b+right+up);
                vs.Add(b-right+up);
                vs.Add(b-right);
            }
        }

        List<int> faces = new List<int>();

        for(int i=0;i<points.Length-1;i++) {
            a = points[i];
            b = points[i+1];

            forward = b-a;
            right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

            int[] tmp = new int[] {
                0,1,4,
                1,5,4,
                1,2,5,
                2,6,5,
                3,7,6,
                3,6,2
            };

            for(int k=0;k<tmp.Length;k++){
                int vertexNumber = i*4 + tmp[k];
                faces.Add(vertexNumber);

                if(k%3==2){
                    
                    Vector3 va = vs[tmp[k-2]];
                    Vector3 vb = vs[tmp[k-1]];
                    Vector3 vc = vs[tmp[k]];

                    normals.Add(Vector3.Cross(vb - va, vc - va));
                    normals.Add(Vector3.Cross(vb - va, vc - va));
                    normals.Add(Vector3.Cross(vb - va, vc - va));
                }
            }
        }

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.Clear();

        Debug.Log(vs.Count + "-----" + normals.Count);

        //mesh.

        mesh.vertices = vs.ToArray();
        mesh.triangles = faces.ToArray();
        //mesh.normals = normals.ToArray();
    }
}
