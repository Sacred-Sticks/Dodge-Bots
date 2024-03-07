using Kickstarter.Bootstrapper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kickstarter.Inputs
{
    /// <summary>
    /// Manages the initialization and enabling of input assets within the game.
    /// </summary>
    public class InputManager : MonoBehaviour, IAwake
    {
        [SerializeField] private InputAsset[] inputObjects;

        public void Awake_()
        {
            InitializeInputs();
        }

        /// <summary>
        /// Initializes all input assets with connected devices and players.
        /// </summary>
        public void InitializeInputs()
        {
            var devices = InputSystem.devices.ToArray();
            var players = FindObjectsOfType<Player>();
            foreach (var inputObject in inputObjects)
                inputObject.Initialize(devices, players);
            EnableAll();
        }

        /// <summary>
        /// Enables all initialized input assets.
        /// </summary>
        public void EnableAll()
        {
            foreach (var inputObject in inputObjects)
                inputObject.EnableInput();
        }
    }
}
