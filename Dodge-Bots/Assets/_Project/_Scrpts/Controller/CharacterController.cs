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
        private float jumpVelocity;
        
        #region UnityEvents
        private void Awake()
        {
            transform.root.TryGetComponent(out body);
        }

        private void Start()
        {
            jumpVelocity = Mathf.Sqrt(Mathf.Abs(jumpHeight * Physics.gravity.y * 2));
        }
        #endregion

        protected void MoveTowards(Vector3 direction)
        {
            direction = (transform.right * direction.x + transform.forward * direction.z).normalized;
            var previousVelocity = Vector3.ProjectOnPlane(body.velocity, Vector3.up);
            var velocityChange = direction * movementSpeed - previousVelocity;
            body.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        protected void Jump()
        {
            // Add Ground Detection & Jump Delay
            body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }
    }
}
