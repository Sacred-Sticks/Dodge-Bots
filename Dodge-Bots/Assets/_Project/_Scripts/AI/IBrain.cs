using UnityEngine;

namespace Dodge_Bots
{
    public interface IBrain
    {
        public void LoseTarget();
        public void FollowTarget(GameObject target);
    }

    public interface IRotator
    {
        public void HandleRotation();
    }
}
