using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class PcMotor : MonoBehaviour
{

    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    public float repathRate = 0.5f;

    public bool reachedEndOfPath = false;

    protected Transform focusTarget;
    protected Vector3 moveTarget;
    protected Seeker seeker;
    protected CharacterController controller;
    protected int currentWaypoint = 0;
    protected float lastRepath = float.NegativeInfinity;
}
