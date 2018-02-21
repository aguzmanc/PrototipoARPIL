using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    public const int RESOLUTION_PER_SEGMENT = 30;


	[SerializeField, HideInInspector]
	List<Vector2> points;

	[SerializeField]
	bool isClosed;


	public Path(Vector2 centre)
	{
		points = new List<Vector2>
		{
			centre + Vector2.left,
			centre + (Vector2.left+Vector2.up) * .5f,
			centre + (Vector2.right+Vector2.down) * .5f,
			centre + Vector2.right
		};
	}


	public Vector2 this[int i]{ get{ return points [i];}}


	public int NumPoints{get{ return points.Count; }}


	public int NumSegments {get{ return points.Count / 3;}}


	public void AddSegment(Vector2 anchorPos)
	{
		points.Add (points [points.Count - 1] * 2 - points [points.Count - 2]);
		points.Add ((points [points.Count - 1] + anchorPos) * .5f);
		points.Add (anchorPos);
	}

	public Vector2[] GetPointsInSegment(int i)
	{
		return new Vector2[] { 
			points[i*3],
			points[i*3+1],
			points[i*3+2],
			points[LoopIndex(i*3+3)],
		};
	}


	public void MovePoint(int i, Vector2 pos)
	{
		Vector2 deltaMove = pos - points [i];
		points [i] = pos;

		if (i % 3 == 0) {
			if (i + 1 < points.Count || isClosed)
				points [LoopIndex(i + 1)] += deltaMove;

			if (i - 1 >= 0 || isClosed)
				points [LoopIndex(i - 1)] += deltaMove;
		} else {
			bool nextIsAnchor = (i + 1) % 3 == 0;
			int correspondingCtrlIndex = nextIsAnchor ? i + 2 : i - 2;
			int anchorIndex = nextIsAnchor ? i + 1 : i - 1;

			if (correspondingCtrlIndex >= 0 && correspondingCtrlIndex < points.Count || isClosed) {
				float dist = (points [LoopIndex(anchorIndex)] - points [LoopIndex(correspondingCtrlIndex)]).magnitude;
				Vector2 dir = (points [LoopIndex(anchorIndex)] - pos).normalized;
				points [LoopIndex(correspondingCtrlIndex)] = points [LoopIndex(anchorIndex)] + dir * dist;
			}
		}
	}


	public void ToggleClosed()
	{
		isClosed = !isClosed;

		if (isClosed) {
			points.Add (points [points.Count - 1] * 2 - points [points.Count - 2]);
			points.Add (points [0] * 2 - points [1]);
		} else
			points.RemoveRange (points.Count - 2, 2);
	}


	int LoopIndex(int i)
	{
		return (i + points.Count) % points.Count;
	}


    public Vector3[] GetRawPoints(int pointsPerSegment = RESOLUTION_PER_SEGMENT)
    {
        Vector3[] ret = new Vector3[(pointsPerSegment+1) * NumSegments];

        for(int seg=0;seg<NumSegments;seg++) {
            Vector2[] ps = GetPointsInSegment(seg);
            for(int i=0;i<=pointsPerSegment;i++) {
                float t = (float)i/(float)pointsPerSegment;
                Vector2 pos = CubicCurve(ps[0], ps[1], ps[2], ps[3], t);
                ret[pointsPerSegment*seg+i] = new Vector3(pos.x, 0, pos.y);
            }
        }

        return ret;
    }


    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b-a) * t;
    }

    public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p0 = Lerp(a,b,t);
        Vector2 p1 = Lerp(b,c,t);
        return Lerp(p0, p1, t);
    }

    public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p0 = QuadraticCurve(a,b,c,t);
        Vector2 p1 = QuadraticCurve(b,c,d,t);
        return Lerp(p0, p1, t);
    }
}
