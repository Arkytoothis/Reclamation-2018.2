using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class PcMotor : MonoBehaviour
{

    public Path path;
    public Transform followTransform;

    public float speed = 3f;
    public float nextWaypointDistance = 0.5f;
    public float repathRate = 0.5f;
    public float turnSpeed = 10f;

    public bool reachedEndOfPath = false;

    protected float distanceToTarget;
    protected float distanceToWaypoint;
    public Transform focusTarget;
    public Vector3 moveTarget;
    protected Seeker seeker;
    protected CharacterController controller;
    protected int currentWaypoint = 0;
    protected float lastRepath = float.NegativeInfinity;
}
