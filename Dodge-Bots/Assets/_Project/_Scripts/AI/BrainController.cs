using Kickstarter.Observer;
using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class BrainController : Observable
    {
        [SerializeField] private float loopTime;
        [SerializeField] private float visionRange;
        [SerializeField] private float visionAngle;
        [SerializeField] private LayerMask layers;

        private GameObject target;

        // Components
        private IEntityDetector entityDetector;
        private IRotator rotator;
        private IBrain[] brains;

        #region UnityEvents
        private void Awake()
        {
            entityDetector = transform.root.GetComponentInChildren<IEntityDetector>();
            rotator = transform.root.GetComponentInChildren<IRotator>();
            brains = transform.root.GetComponentsInChildren<IBrain>();
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
            WaypointsBrain waypointsBrain = null;
            foreach (var brain in brains)
            {
                if (brain is WaypointsBrain)
                    waypointsBrain = brain as WaypointsBrain;
            }
            for (; ; )
            {
                var target = entityDetector.Detect(visionRange, visionAngle, layers);
                Debug.Log(target);
                waypointsBrain.Activate();
                yield return delay;
            }
        }
    }
}
