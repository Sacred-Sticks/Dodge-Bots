using System;
using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float launchVelocity;
        [SerializeField] private float maxCharge;
        [SerializeField] private float ballChargeCost;
        [SerializeField] private float rechargeRate;
        [SerializeField] private float dechargeRate;
        [SerializeField] private float deadTime;

        public float MaxCharge => maxCharge;
        public Action<float> OnChargeChange;

        private bool isBallActive;
        private float ballCharge;
        private float BallCharge
        {
            get => ballCharge;
            set
            {
                ballCharge = value;
                OnChargeChange?.Invoke(ballCharge / MaxCharge);
            }
        }
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

        private Rigidbody body;

        #region UnityEvents
        private void Awake()
        {
            body = GetComponentInParent<Rigidbody>();
        }

        protected virtual void Start()
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
    }
}
