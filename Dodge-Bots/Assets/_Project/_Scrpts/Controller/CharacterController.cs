using System.Threading.Tasks;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class CharacterController : Observable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        protected bool isGrounded;
        
        // Cached References & Constant Values
        protected Rigidbody body;
        private float jumpVelocity;
        private const float jumpDelay = 1.5f;
        private const float radiusMultiplier = 0.5f;
        private const float groundDistance = 1.5f;
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

        protected async void Jump()
        {
            CheckGrounded();
            if (!isGrounded)
                return;
            float timer = 0;
            int timeStep = (int)(Time.deltaTime * 1000);
            while (timer < jumpDelay)
            {
                float yPosition = transform.position.y;
                await Task.Delay(timeStep);
                timer += Time.deltaTime;
                if (transform.position.y < yPosition)
                    continue;
                body.AddForce(jumpVelocity * Vector3.up, ForceMode.VelocityChange);
                break;
            }
        }

        private void CheckGrounded()
        {
            var ray = new Ray(transform.position + transform.up, -transform.up);
            isGrounded = Physics.SphereCast(ray, groundRadius, groundDistance);
        }
        
        
    }
}
