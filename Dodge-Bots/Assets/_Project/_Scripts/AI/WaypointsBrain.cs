using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class WaypointsBrain : LocomotionController
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointDistanceThreshold;

        private int currentWaypointIndex;
        private int CurrentWaypointIndex
        {
            get => currentWaypointIndex;
            set
            {
                currentWaypointIndex = value;
                NotifyObservers(new RotationBrain.TargetAimChange(waypoints[currentWaypointIndex].position));
            }
        }

        private void Start()
        {
            CurrentWaypointIndex = Random.Range(0, waypoints.Length);
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            Jump();
            HandleMovement();
        }

        private void HandleMovement()
        {
            float distanceSquared = Vector3.SqrMagnitude(waypoints[CurrentWaypointIndex].position - transform.position);
            if (distanceSquared < waypointDistanceThreshold * waypointDistanceThreshold)
                CurrentWaypointIndex = (CurrentWaypointIndex + 1) % waypoints.Length;

            var globalDirection = waypoints[currentWaypointIndex].position - transform.root.position;
            MoveTowards(globalDirection.normalized);
        }
    }
}
