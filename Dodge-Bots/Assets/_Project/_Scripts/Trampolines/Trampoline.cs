using UnityEngine;

namespace Dodge_Bots
{
    [SelectionBase]
    public class Trampoline : MonoBehaviour, ITrampoline
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float bounceMultiplier;

        private float jumpVelocity;
        
        #region UnityEvents
        private void Awake()
        {
            //TrampolineManager.AddTrampoline(transform.position, this);
        }

        private void Start()
        {
            jumpVelocity = Mathf.Sqrt(Mathf.Abs(2 * jumpHeight * Physics.gravity.y));
        }
        #endregion
        
        #region ITrampoline
        public void Jump(Rigidbody body)
        {
            body.AddForce(transform.up * jumpVelocity, ForceMode.VelocityChange);
        }

        public void Bounce(Rigidbody body, Vector3 collisionVelocity)
        {
            var velocity = Vector3.Project(collisionVelocity, transform.up);
            body.AddForce(velocity * bounceMultiplier, ForceMode.VelocityChange);
        }
        #endregion
    }
}
