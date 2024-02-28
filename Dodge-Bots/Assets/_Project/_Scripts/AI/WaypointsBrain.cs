using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class WaypointsBrain : LocomotionController
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointDistanceThreshold;

        private int currentWaypointIndex;

        private void Start()
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            Jump();
            HandleMovement();
        }

        private void HandleMovement()
        {
            float distanceSquared = Vector3.SqrMagnitude(waypoints[currentWaypointIndex].position - transform.position);
            if (distanceSquared < waypointDistanceThreshold * waypointDistanceThreshold)
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            var direction = waypoints[currentWaypointIndex].position - transform.position;
            MoveTowards(direction.normalized);
        }
    }
}
