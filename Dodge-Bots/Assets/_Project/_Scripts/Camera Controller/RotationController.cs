using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public abstract class RotationController : Observable
    {
        [SerializeField] protected float rotationSpeed;

        protected void RotateTowards(float direction)
        {
            var rotation = transform.root.rotation.eulerAngles;
            rotation.y += direction * rotationSpeed;
            transform.root.rotation = Quaternion.Euler(rotation);
        }
    }

}
