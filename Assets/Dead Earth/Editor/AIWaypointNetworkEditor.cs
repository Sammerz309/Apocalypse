using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class NewBehaviourScript : Editor
{

    private void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        for (int i = 0; i < network.Waypoints.Count; i++)
        {
            if (network.Waypoints[i] != null)
            {
                Handles.Label(network.Waypoints[i].position, "Waypoint " + i.ToString());
            }
        } // End for loop

        if (network.DisplayMode == PathDisplayMode.Connections)
        {
            Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1];

            for (int i = 0; i <= network.Waypoints.Count; i++)
            {
                int index = i != network.Waypoints.Count ? i : 0; // After last waypoint get the waypoint at first index

                if (network.Waypoints[index] != null)
                {
                    linePoints[i] = network.Waypoints[index].position;
                }
                else
                {
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
                }
            } // End for loop

            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePoints);
        } // End if
        else
        {
            NavMeshPath path = new NavMeshPath();

            if (network.Waypoints[network.UIStart] != null && network.Waypoints[network.UIEnd] != null) // If start and/or end points are not null
            {
                Vector3 startPoint = network.Waypoints[network.UIStart].position;
                Vector3 endPoint = network.Waypoints[network.UIEnd].position;

                NavMesh.CalculatePath(startPoint, endPoint, NavMesh.AllAreas, path);

                Handles.color = Color.green;
                Handles.DrawPolyLine(path.corners);
            }
        }
    } // End OnSceneGUI

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        network.DisplayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.DisplayMode);

        if (network.DisplayMode == PathDisplayMode.Paths)
        {
            network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count - 1);
            network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count - 1);
        }
        
        DrawDefaultInspector();
    }

}
