using Kickstarter.Inputs;
using UnityEngine;

namespace Dodge_Bots
{
    public class PlayerController : CharacterController, IInputReceiver
    {
        [SerializeField] private Vector2Input movementInput;
        [SerializeField] private FloatInput jumpInput;

        private Vector3 rawMovementInput;
        
        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
            jumpInput.RegisterInput(OnJumpInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
            jumpInput.DeregisterInput(OnJumpInputChange, playerIdentifier);
        }

        private void OnMovementInputChange(Vector2 input)
        {
            rawMovementInput = new Vector3(input.x, 0, input.y);
        }

        private void OnJumpInputChange(float input)
        {
            if (input == 0)
                return;
            Jump();
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
