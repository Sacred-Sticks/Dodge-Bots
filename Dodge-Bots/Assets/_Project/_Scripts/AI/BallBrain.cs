using System.Collections;
using UnityEngine;

namespace Dodge_Bots
{
    public class BallBrain : Ball, IBallBrain
    {
        private Transform _target;

        private const float delayDuration = 0.5f;

        private IEnumerator TargetFollower()
        {
            var delay = new WaitForSeconds(delayDuration);
            while (_target != null)
            {
                yield return delay;
            }
        }

        #region BallBrain
        public void FollowTarget(Transform target)
        {
            _target = target;
            StartCoroutine(TargetFollower());
        }

        public void LoseTarget()
        {
            _target = null;
            StopAllCoroutines();
        }
        #endregion
    }

    public interface IBallBrain
    {
        void FollowTarget(Transform target);
        void LoseTarget();
    }
}
