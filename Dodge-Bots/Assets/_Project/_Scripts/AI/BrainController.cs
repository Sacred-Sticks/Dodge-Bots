using Kickstarter.Observer;
using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class BrainController : Observable
    {
        [SerializeField] private float visionRange;
        [SerializeField] private float visionAngle;
        [SerializeField] private LayerMask layers;

        // Components
        private IEntityDetector entityDetector;
        private IRotator rotator;
        private IBrain brain;

        // Constants
        private const float loopTime = 0.5f;

        #region UnityEvents
        private void Awake()
        {
            entityDetector = transform.root.GetComponentInChildren<IEntityDetector>();
            rotator = transform.root.GetComponentInChildren<IRotator>();
            brain = transform.root.GetComponentInChildren<IBrain>();
        }

        private void Start()
        {
            StartCoroutine(CheckForEntities());
        }

        private void Update()
        {
            rotator.HandleRotation();
        }
        #endregion

        private IEnumerator CheckForEntities()
        {
            var delay = new WaitForSeconds(loopTime);
            for (; ; )
            {
                var target = entityDetector.Detect(visionRange, visionAngle, layers);
                if (target != null)
                    brain.FollowTarget(target);
                else
                    brain.LoseTarget();
                yield return delay;
            }
        }
    }
}
