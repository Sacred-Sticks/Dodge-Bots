using UnityEngine;

namespace Dodge_Bots
{
    public interface ITrampoline
    {
        public void Jump(Rigidbody body);
        public void Bounce(Rigidbody body, Vector3 collisionVelocity);
    }
}
