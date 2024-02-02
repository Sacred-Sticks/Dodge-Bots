using System;
using System.Collections;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class Controller : Observable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        protected bool isGrounded;
        private Vector3 activeVelocity;

        private Coroutine trampolineRoutine;
        
        // Cached References & Constant Values
        protected Rigidbody body;
        private float jumpVelocity;
        private const float tolerance = 0.1f;
        private const float accelerationRate = 0.1f;
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
            if (direction == Vector3.zero && isGrounded)
                DecelerateLocalVelocity(); // only decelerate local velocity when grounded to mimic friction
            var currentVelocity = Vector3.ProjectOnPlane(body.velocity, transform.up);
            float directionAccuracy = Vector3.Dot(direction, currentVelocity) - 1;
            if (activeVelocity.sqrMagnitude >= movementSpeed * movementSpeed && !isGrounded)
                if (directionAccuracy < tolerance)
                    return; // prevent overcorrecting velocity mid-air
            if (directionAccuracy > tolerance && currentVelocity.sqrMagnitude > movementSpeed * movementSpeed)
                return; // prevent moving too fast
            var acceleration = direction * (movementSpeed * accelerationRate);
            activeVelocity += acceleration;
            body.AddForce(acceleration, ForceMode.VelocityChange);
        }
        
        protected void Jump()
        {
            if (!isGrounded)
            {
                NotifyObservers(Event.AirJump);
                return;
            }
            body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }

        protected void CheckGrounded()
        {
            var ray = new Ray(transform.position + transform.up, -transform.up);
            isGrounded = Physics.SphereCast(ray, groundRadius, groundDistance);
        }

        private void DecelerateLocalVelocity()
        {
            activeVelocity -= activeVelocity * (movementSpeed * accelerationRate);
        }
        
        #region Notifications
        public enum Event
        {
            Jump,
            AirJump,
        }
        #endregion
    }

}
