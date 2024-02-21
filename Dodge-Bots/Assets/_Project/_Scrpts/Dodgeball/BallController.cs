using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

namespace Dodge_Bots
{
    public class BallController : Observable, IInputReceiver
    {
        [SerializeField] private FloatInput aimInput;
        [SerializeField] private FloatInput ballToggleInput;
        
        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            aimInput.RegisterInput(OnAimInputChange, playerIdentifier);
            ballToggleInput.RegisterInput(OnBallToggleInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            aimInput.DeregisterInput(OnAimInputChange, playerIdentifier);
            ballToggleInput.DeregisterInput(OnBallToggleInputChange, playerIdentifier);
        }

        private void OnAimInputChange(float input)
        {
            
        }

        private void OnBallToggleInputChange(float input)
        {
            
        }
        #endregion
    }
}
