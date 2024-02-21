using Kickstarter.Inputs;
using System;
using UnityEngine;

namespace Dodge_Bots
{
    public class BallController : Ball, IInputReceiver
    {
        [SerializeField] private FloatInput aimInput;
        [SerializeField] private FloatInput launchInput;

        const float tolerance = 0.5f;

        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            aimInput.RegisterInput(OnAimInputChange, playerIdentifier);
            launchInput.RegisterInput(OnLaunchInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            aimInput.DeregisterInput(OnAimInputChange, playerIdentifier);
            launchInput.DeregisterInput(OnLaunchInputChange, playerIdentifier);
        }

        private void OnAimInputChange(float input)
        {
            Action action = input > tolerance ? ZoomIn : ZoomOut;
            action();
        }

        private void OnLaunchInputChange(float input)
        {
            isBallActive = input > tolerance;
            Action action = isBallActive ? Propel : null;
            action?.Invoke();
        }
        #endregion

        private void ZoomIn()
        {

        }

        private void ZoomOut()
        {

        }
    }
}
