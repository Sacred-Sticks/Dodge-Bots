using UnityEngine;

namespace Dodge_Bots
{
    public interface IEntityDetector
    {
        public GameObject Detect(float visionRange, float visionAngle, LayerMask layers);
    }
}
