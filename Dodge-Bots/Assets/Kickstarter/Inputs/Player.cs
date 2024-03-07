using Kickstarter.Bootstrapper;
using System.Collections;
using UnityEngine;

namespace Kickstarter.Inputs
{
    /// <summary>
    /// Represents a player in the game, facilitating input registration and deregistration.
    /// </summary>
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerIdentifier Identifier { get; private set; }

        /// <summary>
        /// Enumeration representing different player identifications.
        /// </summary>
        public enum PlayerIdentifier
        {
            Player1,
            Player2,
            Player3,
            Player4,
        }
        
        private IInputReceiver[] inputReceivers;

        //public void Awake_()
        //{
        //    inputReceivers = GetComponentsInChildren<IInputReceiver>();
        //}
        
        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            inputReceivers = GetComponentsInChildren<IInputReceiver>();
            foreach (var inputReceiver in inputReceivers)
                inputReceiver.RegisterInputs(Identifier);
        }

        private void OnEnable()
        {
            if (inputReceivers == null)
                return;
            foreach (var inputReceiver in inputReceivers)
                inputReceiver.RegisterInputs(Identifier);
        }

        private void OnDisable()
        {
            if (inputReceivers == null)
                return;
            foreach (var inputReceiver in inputReceivers)
                inputReceiver.DeregisterInputs(Identifier);
        }
    }
}
