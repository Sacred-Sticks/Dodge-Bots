using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class Ball : Observable
    {
        [SerializeField] private float launchVelocity;
        [SerializeField] private float maxCharge;
        [SerializeField] private float rechargeRate;
        [SerializeField] private float dechargeRate;
        [SerializeField] private float deadTime;

        protected bool isBallActive;
        private float ballCharge;
        private float deadTimer;
        private bool canCharge = true;

        private Rigidbody body;
        private Transform directionSource;

        #region UnityEvents
        private void Awake()
        {
            body = GetComponentInParent<Rigidbody>();
            var camera = Camera.main;
            directionSource = camera.transform;
        }

        private void Start()
        {
            ballCharge = maxCharge;
        }

        private void Update()
        {
            ChargeBall();
        }
        #endregion

        private void ChargeBall()
        {
            if (!canCharge)
            {
                deadTimer += Time.deltaTime;
                if (deadTimer >= deadTime)
                    canCharge = true;
                return;
            }

            ballCharge += Time.deltaTime * (isBallActive ? -dechargeRate : rechargeRate);
            ballCharge = Mathf.Clamp(ballCharge, 0, maxCharge);
            NotifyObservers(new BallState(isBallActive, ballCharge));

            if (ballCharge != 0)
                return;
            canCharge = false;
            deadTimer = 0;
        }

        protected void Propel()
        {
            if (ballCharge == 0)
                return;
            body.AddForce(directionSource.forward * launchVelocity, ForceMode.VelocityChange);
        }

        #region Notifiers
        public struct BallState : INotification
        {
            public BallState(bool isActive, float charge)
            {
                IsActive = isActive;
                Charge = charge;
            }

            public bool IsActive { get; }
            public float Charge { get; }
        }
        #endregion
    }
}
