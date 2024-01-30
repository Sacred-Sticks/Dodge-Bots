using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class CharacterController : Observable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;
        
        // Cached References & Constant Values
        protected Rigidbody body;
        private const float tolerance = 0.25f;
        private const float movementForce = 50;
        private float speedSquared;
        private float jumpVelocity;
        
        #region UnityEvents
        private void Awake()
        {
            transform.root.TryGetComponent(out body);
        }

        private void Start()
        {
            speedSquared = movementSpeed * movementSpeed;
            jumpVelocity = Mathf.Sqrt(Mathf.Abs(jumpHeight * Physics.gravity.y * 2));
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

        protected void Jump()
        {
            body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }
    }
}
