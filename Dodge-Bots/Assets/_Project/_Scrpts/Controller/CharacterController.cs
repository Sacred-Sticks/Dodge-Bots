using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class CharacterController : Observable
    {
        [SerializeField] private float movementSpeed;
        
        // Cached References & Constant Values
        protected Rigidbody body;
        private const float tolerance = 0.25f;
        private const float movementForce = 50;
        private float speedSquared;
        
        #region UnityEvents
        private void Awake()
        {
            transform.root.TryGetComponent(out body);
        }

        private void Start()
        {
            speedSquared = movementSpeed * movementSpeed;
        }
        #endregion

        protected void MoveTowards(Vector3 direction)
        {
            direction = (transform.right * direction.x + transform.forward * direction.z).normalized;
            float directioAccuracy = Vector3.Dot(direction, body.velocity.normalized);
            if (Mathf.Abs(directioAccuracy - 1) < tolerance && body.velocity.sqrMagnitude > speedSquared)
                return;
            body.AddForce(direction * movementForce, ForceMode.Acceleration);
        }
    }
}
