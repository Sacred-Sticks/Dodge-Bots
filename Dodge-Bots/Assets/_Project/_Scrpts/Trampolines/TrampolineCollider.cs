using System.Collections;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    [RequireComponent(typeof(Rigidbody))]
    public class TrampolineCollider: Observable, IObserver<LocomotionController.Event>
    {
        private bool trampolineJumping;

        private Coroutine trampolineRoutine;
        
        // Cached References and Constant Values
        private Rigidbody body;
        private const float jumpDelay = 1.5f;
        
        #region UnityEvents
        private void Awake()
        {
            TryGetComponent(out body);
            GetComponentInChildren<LocomotionController>().AddObserver(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnTrampolineCollision(collision);
        }
        #endregion
        
        private void OnTrampolineCollision(Collision collision)
        {
            if (!TrampolineManager.TryGetTrampoline(collision.transform.root.position, out var trampoline))
                return;
            if (!trampolineJumping)
            {
                trampoline.Bounce(body, collision.relativeVelocity);
                NotifyObservers(LocomotionController.Event.Bounce);
                return;
            }
            StopCoroutine(trampolineRoutine);
            trampolineJumping = false;
            trampoline.Jump(body);
            NotifyObservers(LocomotionController.Event.Jump);
        }
        
        protected IEnumerator TrampolineJumpTimer()
        {
            trampolineJumping = true;
            yield return new WaitForSeconds(jumpDelay);
            trampolineJumping = false;
        }

        public void OnNotify(LocomotionController.Event argument)
        {
            trampolineRoutine = argument switch
            {
                LocomotionController.Event.AirJump => StartCoroutine(TrampolineJumpTimer()),
                _ => trampolineRoutine,
            };
        }
    }
}
