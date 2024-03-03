using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class RotationBrain : RotationController, IObserver<RotationBrain.TargetAimChange>, IRotator
    {
        private Transform target;

        #region UnityEvents
        private void Awake()
        {
            transform.root.GetComponentInChildren<WaypointsBrain>().AddObserver(this);
        }

        private void Start()
        {
            target = transform;
        }
        #endregion

        #region Rotator
        public void HandleRotation()
        {
            var direction = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
            var forwards = transform.root.forward;
            float angle = Vector3.SignedAngle(forwards, direction, Vector3.up);
            RotateTowards(angle / 180);
        }
        #endregion

        #region Notifications
        public void OnNotify(TargetAimChange argument)
        {
            target = argument.Target;
        }
        #endregion

        #region Notifiers
        public struct TargetAimChange : INotification
        {
            public TargetAimChange(Transform target) => Target = target;
            public Transform Target { get; }
        }
        #endregion
    }
}
