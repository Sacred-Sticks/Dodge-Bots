using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class WaypointsBrain : LocomotionController, IBrain
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointDistanceThreshold;

        private bool overrideTarget;
        private int currentWaypointIndex;
        private Transform activeTarget;
        private Transform ActiveTarget
        {
            get => activeTarget;
            set
            {
                activeTarget = value;
                NotifyObservers(new RotationBrain.TargetAimChange(activeTarget));
            }
        }

        // Constants
        private const float jumpDelay = 1.5f;

        #region UnityEvents
        private void Start()
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
            ActiveTarget = waypoints[currentWaypointIndex];
            StartCoroutine(BrainLoop());
            StartCoroutine(JumpLoop());
        }
        #endregion

        #region Brain
        public void LoseTarget()
        {
            overrideTarget = false;
            ActiveTarget = waypoints[Random.Range(0, waypoints.Length)];
        }

        public void FollowTarget(GameObject target)
        {
            overrideTarget = true;
            ActiveTarget = target.transform;
        }
        #endregion

        private IEnumerator BrainLoop()
        {
            overrideTarget = false;
            var delay = new WaitForSeconds(Time.fixedDeltaTime);
            while (true)
            {
                CheckGrounded();
                HandleMovement();
                yield return delay;
            }
        }

        private IEnumerator JumpLoop()
        {
            var delay = new WaitForSeconds(jumpDelay);
            yield return delay;
            while (true)
            {
                yield return delay;
                Jump();
            }
        }

        private void HandleMovement()
        {
            float distanceSquared = Vector3.SqrMagnitude(waypoints[currentWaypointIndex].position - transform.position);
            if (!overrideTarget && distanceSquared < waypointDistanceThreshold * waypointDistanceThreshold)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                ActiveTarget = waypoints[currentWaypointIndex];
            }

            var globalDirection = ActiveTarget.position - transform.root.position;

            MoveTowards(globalDirection.normalized);
        }
    }
}
