using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class RotationBrain : RotationController, IObserver<RotationBrain.TargetAimChange>, IRotator
    {
        [SerializeField]
        private Vector3 target;

        #region UnityEvents
        private void Awake()
        {
            transform.root.GetComponentInChildren<WaypointsBrain>().AddObserver(this);
        }

        private void Start()
        {
            target = transform.position + new Vector3(3, 0, 0);
        }
        #endregion

        #region Rotator
        public void HandleRotation()
        {
            var direction = Vector3.ProjectOnPlane(target - transform.position, Vector3.up);
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
            public TargetAimChange(Vector3 target) => Target = target;
            public Vector3 Target { get; }
        }
        #endregion
    }
}
