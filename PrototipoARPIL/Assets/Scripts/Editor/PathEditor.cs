﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor 
{
	PathCreator creator;
	Path path;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Cerrar/No Cerrar")) {
			path.ToggleClosed ();

			SceneView.RepaintAll ();
		}

		if (GUILayout.Button ("Reset")) {
			creator.CreatePath ();
			path = creator.path;
			SceneView.RepaintAll ();
		}
	}


	void OnSceneGUI()
	{
		Input ();
		Draw ();
	}


	void Input()
	{
		Event ev = Event.current;
		Vector3 mousePos = HandleUtility.GUIPointToWorldRay (ev.mousePosition).origin;

		if (ev.type == EventType.MouseDown && ev.button == 0 && ev.shift) {
			Undo.RecordObject (creator, "AddSegment");
			path.AddSegment (ToV2(mousePos));
		}
	}

	void Draw()
	{
		for (int i = 0; i < path.NumSegments; i++) {
			Vector2[] points = path.GetPointsInSegment (i);

			Handles.color = Color.black;
			Handles.DrawLine (ToV3(points [1]), ToV3(points [0]));
			Handles.DrawLine (ToV3(points [2]), ToV3(points [3]));
			Handles.DrawBezier (ToV3(points [0]), ToV3(points [3]), ToV3(points [1]), ToV3(points [2]), Color.green, null, 5);
		}

		Handles.color = Color.red;
		for (int i = 0; i < path.NumPoints; i++) {
			Vector3 newPos = Handles.FreeMoveHandle (ToV3 (path [i]), Quaternion.identity, .1f, Vector3.zero, Handles.CylinderHandleCap);

			if (ToV3(path [i]) != newPos) {
				Undo.RecordObject (creator, "Move point");
				path.MovePoint (i, ToV2(newPos));
			}
		}

		if (creator.ShowRawPositions) {
			Vector3[] allPoints = path.GetRawPoints (creator.PointsPerSegment);
			for (int i = 0; i < allPoints.Length; i++) {
				Debug.DrawRay (creator.transform.TransformPoint(allPoints [i]), Vector3.up * creator.DebugLinesWidth, Color.blue);
			}
		}


		if (creator.ShowMarkLines) {
			Vector3[] marks = creator.GetEquidistantPoints (creator.MarkLinesDistance);
			for (int i = 0; i < marks.Length; i++) {
				Debug.DrawRay (creator.transform.TransformPoint(marks [i]), Vector3.up * creator.DebugLinesWidth, Color.red);
			}
		}

		creator.GenerateMesh ();
	}


	void OnEnable()
	{
		creator = (PathCreator)target;
		if (creator.path == null)
			creator.CreatePath ();

		path = creator.path;
	}

	public Vector3 ToV3(Vector2 from)
	{
		return new Vector3 (from.x, 0, from.y); 
	}

	public Vector2 ToV2(Vector3 from)
	{
		return new Vector2 (from.x, from.z);
	}
}
