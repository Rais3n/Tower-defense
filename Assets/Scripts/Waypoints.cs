using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] Transform[] waypoint;

    public static Waypoints Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

  

    public Transform GetWaypoint(int index)
    { 
        return waypoint[index];
    }

    public int GetSizeWaypointArray()
    {
        return waypoint.Length;
    }

}
