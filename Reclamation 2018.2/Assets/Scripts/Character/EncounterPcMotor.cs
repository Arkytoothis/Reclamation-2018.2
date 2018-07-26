using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
public class EncounterPcMotor : PcMotor
{
    public bool following = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
    }

    //public void SetMoveTarget(bool follow, Vector3 moveTarget)
    //{
    //    following = follow;
    //    this.moveTarget = moveTarget;
    //    followTransform.position = moveTarget;
    //    seeker.StartPath(transform.position, moveTarget, OnPathComplete);
    //    FaceTarget(this.moveTarget);
    //}

    public void OnPathComplete(Path p)
    {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (focusTarget != null)
        {
            //agent.SetDestination(target.position);
            FaceTarget(focusTarget.position);
        }

        //if (Vector3.Distance(transform.position, EncounterCursor.instance.transform.position) <= 0)
        //{
        //    return;
        //}
        //if (path == null)
        //{
        //    // We have no path to follow yet, so don't do anything
        //    return;
        //}
        //else
        //{
            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;                

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, moveTarget, OnPathComplete);
            

            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.
            reachedEndOfPath = false;
            // The distance to the next waypoint in the path
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToTarget = Vector3.Distance(transform.position, moveTarget);

                if (path != null) distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                else distanceToWaypoint = 0f;

                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        following = false;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector3 velocity = dir * speed * speedFactor;

            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            controller.SimpleMove(velocity);

            //if (reachedEndOfPath == false)
            //    FaceTarget(path.vectorPath[currentWaypoint]);

            if (dir.x != 0 && dir.z != 0 && distanceToWaypoint > 0.05f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
    }

    //public void FollowTarget(Interactable interactable)
    //{
    //    focusTarget = interactable.interactionTransform;
    //    SetMoveTarget(true, focusTarget.position);
    //}

    //public void FollowTarget(Transform targetTransform)
    //{
    //    followTransform = targetTransform;

    //    SetMoveTarget(true, followTransform.position);
    //}

    //public void StopFollowingTarget()
    //{
    //    focusTarget = null;
    //}

    public void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.x != 0 && direction.z != 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }
}