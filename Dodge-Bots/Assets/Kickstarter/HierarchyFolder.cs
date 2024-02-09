using UnityEngine;

namespace Kickstarter.Organization
{
    public class HierarchyFolder : MonoBehaviour
    {
        private void OnEnable()
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
                transform.GetChild(0).parent = transform.parent;
            Destroy(gameObject);
        }
    }
}
