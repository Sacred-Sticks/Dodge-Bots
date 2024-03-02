using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class WaypointsBrain : LocomotionController, IBrain
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointDistanceThreshold;

        private bool loopActive;
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

        #region UnityEvents
        private void Start()
        {
            CurrentWaypointIndex = Random.Range(0, waypoints.Length);
        }
        #endregion

        private IEnumerator BrainLoop()
        {
            loopActive = true;
            var delay = new WaitForSeconds(Time.fixedDeltaTime);
            while (loopActive)
            {
                CheckGrounded();
                Jump();
                HandleMovement();
                yield return delay;
            }
        }

        private void HandleMovement()
        {
            float distanceSquared = Vector3.SqrMagnitude(waypoints[CurrentWaypointIndex].position - transform.position);
            if (distanceSquared < waypointDistanceThreshold * waypointDistanceThreshold)
                CurrentWaypointIndex = (CurrentWaypointIndex + 1) % waypoints.Length;

            var globalDirection = waypoints[currentWaypointIndex].position - transform.root.position;
            MoveTowards(globalDirection.normalized);
        }

        #region Brain
        public void Activate()
        {
            StartCoroutine(BrainLoop());
        }

        public void Deactivate()
        {
            loopActive = false;
        }
        #endregion
    }
}
