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


	public List<int> faces = new List<int>();

    public void GenerateMesh()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

        Vector3[] points = path.GetRawPoints(PointsPerSegment);
        ALLPOINTS = points;

        Vector3 up = Vector3.up * Height;
        List<Vector3> vs = new List<Vector3>();

		List<Vector3> normals = new List<Vector3> ();
		normals.AddRange (new Vector3[points.Length * 12]);

		faces = new List<int> ();
        Vector3 a=Vector3.zero, b=Vector3.zero, forward=Vector3.zero, right=Vector3.zero;

		// generate cuts first
		List<Vector3[]> cuts = new List<Vector3[]> ();

		for (int i = 0; i < points.Length; i++) {
			if(i<points.Length-2) { // for all the points
				a = points[i];
				b = points[i+1];
			} else { // for the last point
				a = points[i-1];
				b = path.isClosed ? points [0] : points [i];
			}

			forward = b-a;
			right = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * Width;

			cuts.Add (
				new Vector3[4] {
					a + right,	
					a + right + up,
					a - right + up,
					a - right
				});
		}


        // generate vertices
		Vector3[] VA = null, VB=null;
        for(int i=0;i<points.Length;i++) {
            if(i<points.Length-2) { // for all the points
				VA = cuts[i];
                VB = cuts[i+1];
            } else { // for the last point
				VA = cuts[i-1];
				VB = path.isClosed ? cuts[0] : cuts[i];
            }

			vs.Add (VA [0]);
			vs.Add (VA [1]);
			vs.Add (VB [1]);
			vs.Add (VB [0]);

			vs.Add (VA [1]);
			vs.Add (VA [2]);
			vs.Add (VB [2]);
			vs.Add (VB [1]);

			vs.Add (VA [2]);
			vs.Add (VA [3]);
			vs.Add (VB [3]);
			vs.Add (VB [2]);

			int[] tmp = new int[] {
				0,1,2,
				0,2,3, 
				4,5,6,
				4,6,7,
				8,9,10,
				8,10,11
			};

			Vector3[] face = new Vector3[3];
			int[] idxVert = new int[3];

			for(int k=0;k<tmp.Length;k++){
				int vertexNumber = i*12 + tmp[k];
				faces.Add(vertexNumber);

				face [k % 3] = vs [vertexNumber];
				idxVert [k % 3] = vertexNumber;

				if(k%3==2){
					Vector3 normal = Vector3.Cross (face [1] - face [0],face [2] - face [0]);

					normals [idxVert [0]] = normal;
					normals [idxVert [1]] = normal;
					normals [idxVert [2]] = normal;
				}
			}
        }

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.Clear();

        mesh.vertices = vs.ToArray();
        mesh.triangles = faces.ToArray();
        mesh.normals = normals.ToArray();
    }
}
