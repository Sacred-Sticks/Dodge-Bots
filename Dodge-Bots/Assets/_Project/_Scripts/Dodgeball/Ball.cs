using Kickstarter.Observer;
using System;
using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class Ball : Observable
    {
        [SerializeField] private float launchVelocity;
        [SerializeField] private float maxCharge;
        [SerializeField] private float ballChargeCost;
        [SerializeField] private float rechargeRate;
        [SerializeField] private float dechargeRate;
        [SerializeField] private float deadTime;

        private bool isBallActive;
        private float ballCharge;
        private bool decharging;
        private Coroutine chargeRoutine;

        protected bool IsBallActive
        {
            get => isBallActive;
            set
            {
                isBallActive = value;
                Func<IEnumerator> charge = IsBallActive ? null : Recharge;
                if (decharging)
                {
                    StopCoroutine(chargeRoutine);
                    decharging = false;
                }
                if (charge != null)
                    chargeRoutine = StartCoroutine(charge());
            }
        }
        private float BallCharge
        {
            get => ballCharge;
            set
            {
                ballCharge = value;
                NotifyObservers(new BallState(isBallActive, maxCharge, ballCharge));
            }
        }

        private Rigidbody body;

        #region UnityEvents
        private void Awake()
        {
            body = GetComponentInParent<Rigidbody>();
        }

        private void Start()
        {
            BallCharge = maxCharge;
        }
        #endregion

        protected void Propel(Vector3 direction)
        {
            if (BallCharge < ballChargeCost)
                return;
            body.AddForce(direction * launchVelocity, ForceMode.VelocityChange);
            BallCharge -= ballChargeCost;
            if (chargeRoutine != null)
                StopCoroutine(chargeRoutine);
            chargeRoutine = StartCoroutine(Decharge());
        }

        private IEnumerator Decharge()
        {
            decharging = true;
            while (BallCharge > 0)
            {
                BallCharge -= Time.deltaTime * dechargeRate;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            BallCharge = 0;
            decharging = false;
        }

        private IEnumerator Recharge()
        {
            yield return new WaitForSeconds(deadTime);
            while (BallCharge < maxCharge)
            {
                BallCharge += Time.deltaTime * rechargeRate;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            BallCharge = maxCharge;
        }   

        #region Notifiers
        public struct BallState : INotification
        {
            public BallState(bool isActive, float maxCharge, float charge)
            {
                IsActive = isActive;
                MaxCharge = maxCharge;
                Charge = charge;
            }

            public bool IsActive { get; }
            public float MaxCharge { get; }
            public float Charge { get; }
        }
        #endregion
    }
}
