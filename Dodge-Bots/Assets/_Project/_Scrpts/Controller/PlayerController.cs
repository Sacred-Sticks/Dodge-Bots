using Kickstarter.Inputs;
using UnityEngine;

namespace Dodge_Bots
{
    public class PlayerController : CharacterController, IInputReceiver
    {
        [SerializeField] private Vector2Input movementInput;

        private Vector3 rawMovementInput;
        
        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
        }

        private void OnMovementInputChange(Vector2 input)
        {
            rawMovementInput = new Vector3(input.x, 0, input.y);
        }
        #endregion
        
        #region UnityEvents
        private void FixedUpdate()
        {
            MoveTowards(rawMovementInput);
        }
        #endregion
    }
}
