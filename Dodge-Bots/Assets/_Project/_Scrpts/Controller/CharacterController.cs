using System;
using System.Collections;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class CharacterController : Observable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        protected bool isGrounded;

        private Coroutine trampolineRoutine;
        
        // Cached References & Constant Values
        protected Rigidbody body;
        private float jumpVelocity;
        private const float radiusMultiplier = 0.5f;
        private const float groundDistance = 1f;
        private float groundRadius;
        
        #region UnityEvents
        private void Awake()
        {
            transform.root.TryGetComponent(out body);
        }

        private void Start()
        {
            jumpVelocity = Mathf.Sqrt(Mathf.Abs(jumpHeight * Physics.gravity.y * 2));
            transform.root.TryGetComponent(out CapsuleCollider capsule);
            groundRadius = capsule.radius * radiusMultiplier;
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
            CheckGrounded();
            if (!isGrounded)
            {
                NotifyObservers(new OnMidAirJump());
                return;
            }
            body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }

        private void CheckGrounded()
        {
            var ray = new Ray(transform.position + transform.up, -transform.up);
            isGrounded = Physics.SphereCast(ray, groundRadius, groundDistance);
        }
        
        #region Notifications
        public struct OnMidAirJump : INotification
        {
            
        }
        #endregion
    }
}
