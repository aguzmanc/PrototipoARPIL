using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour 
{
	[HideInInspector]
	public Path path;

	[Header("Required")]
    [Range(2, 100)]
    public GameObject TestPointPrototype;

	[Header("Road Configuration")]
	[Range(0, 30)]
	public int PointsPerSegment = 30;
	[Range(0.1f, 2f)]
    public float Width = 0.2f;
	[Range(0.01f, 0.5f)]
    public float Height = 0.1f;
	[Range(0, 50)]
	public int PercentajeLane = 10;


	[Header("Debug")]
	public bool ShowDebugLines = false;
	[Range(0, 1f)]
	public float DebugLinesWidth = 0.2f;


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
		GenerateRoadMesh (transform.Find ("RoadMesh").gameObject);
		GenerateLaneMesh (transform.Find ("LaneMesh").gameObject);
	}
 

	public void GenerateRoadMesh(GameObject child)
    {
        MeshFilter meshFilter = child.GetComponent<MeshFilter>();
        Vector3[] points = path.GetRawPoints(PointsPerSegment);

        List<Vector3> vs = new List<Vector3>();
		List<Vector3> normals = new List<Vector3> (new Vector3[points.Length * 12]);
		List<int> tris = new List<int> ();

		Vector3 up = Vector3.up * Height;
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

        // generate road vertices
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
				tris.Add(vertexNumber);

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
        mesh.triangles = tris.ToArray();
        mesh.normals = normals.ToArray();
    }




	public void GenerateLaneMesh(GameObject child) 
	{
		MeshFilter meshFilter = child.GetComponent<MeshFilter>();
		Vector3[] points = path.GetRawPoints(PointsPerSegment);

		List<Vector3> vs = new List<Vector3>();
		List<int> tris = new List<int> ();

		Vector3 up = Vector3.up * (Height + 0.001f); // a litle bit up
		Vector3 a=Vector3.zero, b=Vector3.zero, forward=Vector3.zero, right=Vector3.zero, innerRight=Vector3.zero;

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
			right = (Quaternion.AngleAxis (90, Vector3.up) * forward).normalized * Width;
			innerRight = (Quaternion.AngleAxis(90, Vector3.up) * forward).normalized * (Width-(Width*2*(  (float)PercentajeLane/100f   )));

			cuts.Add (
				new Vector3[4] {
					a + right + up,
					a + innerRight + up,
					a - innerRight + up,
					a - right + up
				});
		}

		// Generate Lane marks
		Vector3[] VA = null, VB=null;
		for(int i=0;i<points.Length;i++) {
			if(i<points.Length-2) { // for all the points
				VA = cuts[i];
				VB = cuts[i+1];
			} else { // for the last point
				VA = cuts[i-1];
				VB = path.isClosed ? cuts[0] : cuts[i];
			}

			vs.Add (VA[0]);
			vs.Add (VA[1]);
			vs.Add (VB[1]);
			vs.Add (VB[0]);

			vs.Add (VA[3]);
			vs.Add (VB[3]);
			vs.Add (VB[2]);
			vs.Add (VA[2]);

			int[] tmp = new int[] {
				0,1,2,
				0,2,3, 
				4,5,6,
				4,6,7
			};

			Vector3[] face = new Vector3[3];

			for(int k=0;k<tmp.Length;k++){
				int vertexNumber = i*8 + tmp[k];
				tris.Add(vertexNumber);
			}
		}

		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		mesh.Clear();

		mesh.vertices = vs.ToArray();
		mesh.triangles = tris.ToArray();
	}
}
