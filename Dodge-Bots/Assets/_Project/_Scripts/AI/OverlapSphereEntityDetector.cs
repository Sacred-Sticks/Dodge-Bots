using System.Linq;
using UnityEngine;

namespace Dodge_Bots
{
    public class OverlapSphereEntityDetector : MonoBehaviour, IEntityDetector
    {
        private Collider[] overlappingColliders;
        private float[] angles;
        private const int MAX_COLLIDERS = 10;

        #region UnityEvents
        private void Start()
        {
            overlappingColliders = new Collider[MAX_COLLIDERS];
            angles = new float[MAX_COLLIDERS];
        }
        #endregion

        #region EntityDetector
        public GameObject Detect(float visionRange, float visionAngle, LayerMask layers)
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.root.position, visionRange, overlappingColliders, layers);

            for (int i = 0; i < numColliders; i++)
            {
                if (overlappingColliders[i].gameObject == transform.root.gameObject)
                    continue;

                Vector3 direction = overlappingColliders[i].transform.position - transform.root.position;
                angles[i] = Vector3.Angle(transform.root.forward, direction);
            }

            var visibleColliders = overlappingColliders
                .Where(c => c != null)
                .Where(c =>
                {
                    var direction = c.transform.position - transform.root.position;
                    var angle = Vector3.Angle(transform.root.forward, direction);
                    return angle < visionAngle;
                })
                .Select(c => c.transform.root.gameObject)
                .Where(g => g != transform.root.gameObject);

            return visibleColliders.FirstOrDefault();
        }
        #endregion
    }
}
