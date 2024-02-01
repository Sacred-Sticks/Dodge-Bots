﻿using System.Collections;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    [RequireComponent(typeof(Rigidbody))]
    public class TrampolineCollider: MonoBehaviour, IObserver<CharacterController.OnMidAirJump>
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
            GetComponentInChildren<CharacterController>().AddObserver(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnTrampolineCollision(collision);
        }
        #endregion
        
        private void OnTrampolineCollision(Collision collision)
        {
            if (!TrampolineManager.TryGetTrampoline(collision.transform.position, out var trampoline))
                return;
            if (!trampolineJumping)
            {
                trampoline.Bounce(body, collision.relativeVelocity);
                return;
            }
            StopCoroutine(trampolineRoutine);
            trampolineJumping = false;
            trampoline.Jump(body);
        }
        
        protected IEnumerator TrampolineJumpTimer()
        {
            trampolineJumping = true;
            yield return new WaitForSeconds(jumpDelay);
            trampolineJumping = false;
        }

        public void OnNotify(CharacterController.OnMidAirJump argument)
        {
            trampolineRoutine = StartCoroutine(TrampolineJumpTimer());
        }
    }
}
