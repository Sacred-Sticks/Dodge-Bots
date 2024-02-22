using UnityEngine;

namespace Dodge_Bots
{
    public class CursorLock : MonoBehaviour
    {
        #region UnityEvents
        private void Start()
        {
            LockCursor();
        }
        #endregion

        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
