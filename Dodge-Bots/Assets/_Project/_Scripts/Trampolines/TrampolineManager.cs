using System.Collections.Generic;
using UnityEngine;

namespace Dodge_Bots
{
    public class TrampolineManager : MonoBehaviour
    {
        private static readonly Dictionary<Vector3, ITrampoline> trampolines = new Dictionary<Vector3, ITrampoline>();

        private void Awake()
        {
            trampolines.Clear();
        }

        public static void AddTrampoline(Vector3 key, ITrampoline trampoline)
        {
            trampolines.Add(key, trampoline);
        }

        public static bool TryGetTrampoline(Vector3 key, out ITrampoline trampoline)
        {
            bool output = trampolines.ContainsKey(key);
            trampoline = output ? trampolines[key] : null;
            return output;
        }
    }
}
