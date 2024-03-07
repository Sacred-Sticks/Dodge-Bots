using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public abstract class LocomotionController : Observable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        protected bool isGrounded;
        private Vector3 airborneVelocity;
        private Vector3 initialAirborneVelocity;

        private Coroutine trampolineRoutine;

        // Cached References & Constant Values
        protected Rigidbody body;
        private float jumpVelocity;
        private const float groundedAccelerationRate = 50f;
        private const float airborneAccelerationRate = 0.1f;
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
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            NotifyObservers(new MovementChange(transform.InverseTransformDirection(direction)));
            if (!isGrounded)
            {
                AirborneMoveTowards(direction);
                return;
            }
            var currentVelocity = Vector3.ProjectOnPlane(body.velocity, transform.up);
            if (currentVelocity.sqrMagnitude > movementSpeed * movementSpeed)
                return;
            var desiredVelocity = direction * movementSpeed;
            body.AddForce(desiredVelocity - currentVelocity, ForceMode.VelocityChange);
        }

        private void AirborneMoveTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;
            if (Vector3.Dot(direction, initialAirborneVelocity) > 0)
                return;
            if (airborneVelocity.sqrMagnitude > movementSpeed * movementSpeed)
                return;
            var desiredVelocity = direction * movementSpeed;
            var deltaVelocity = (desiredVelocity - airborneVelocity) * airborneAccelerationRate;
            airborneVelocity += deltaVelocity;
            body.AddForce(deltaVelocity, ForceMode.VelocityChange);
        }

        protected void Jump()
        {
            if (!isGrounded)
            {
                NotifyObservers(Event.AirJump);
                return;
            }
            body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
            NotifyObservers(Event.Jump);
        }

        protected void CheckGrounded()
        {
            var ray = new Ray(transform.position + Vector3.up, -Vector3.up);
            bool previouslyGrounded = isGrounded;
            isGrounded = Physics.SphereCast(ray, groundRadius, groundDistance);
            NotifyObservers(isGrounded ? Event.Grounded : Event.Airborne);
            if (!previouslyGrounded || isGrounded)
                return;
            initialAirborneVelocity = Vector3.ProjectOnPlane(body.velocity, Vector3.up);
            airborneVelocity = Vector3.zero;
        }

        #region Notifications
        public enum Event
        {
            Jump,
            Bounce,
            AirJump,
            Grounded,
            Airborne,
        }

        public struct MovementChange : INotification
        {
            public Vector3 LocalDirection { get; }

            public MovementChange(Vector3 localDirection)
            {
                LocalDirection = localDirection.normalized;
            }
        }
        #endregion
    }

}
