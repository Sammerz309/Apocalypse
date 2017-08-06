﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    // Inspector Assigned Variable
    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentWaypointIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;

    // Private Members
    private NavMeshAgent _navAgent = null;

	// Use this for initialization
	void Start ()
    {
        // Cache NavMeshAgent Reference
        _navAgent = GetComponent<NavMeshAgent>();

        if (WaypointNetwork == null) return;

        SetNextDestination(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        HasPath = _navAgent.hasPath;
        PathPending = _navAgent.pathPending;

		if (!HasPath && !PathPending)
        {
            SetNextDestination(true);
        }
        else if (_navAgent.isPathStale)
        {
            SetNextDestination(false);
        }
	}

    void SetNextDestination (bool increment)
    {
        // If no network return
        if (!WaypointNetwork) return;

        // Calculatehow much the current waypoint index needs to be incremented
        int incStep = increment ? 1 : 0;

        // Calculate index of next waypoint factoring in the increment with wrap-around and fetch waypoint 
        int nextWaypoint = (CurrentWaypointIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentWaypointIndex + incStep;
        Transform nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

        // Assuming we have a valid waypoint transform
        if (nextWaypointTransform != null)
        {
            // Update the current waypoint index, assign its position as the NavMeshAgents
            // Destination and then return
            CurrentWaypointIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }

        // We did not find a valid waypoint in the list for this iteration
        CurrentWaypointIndex++;
    }
}
