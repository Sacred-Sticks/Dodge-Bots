using Kickstarter.Inputs;
using UnityEngine;

namespace Dodge_Bots
{
    public class PlayerRotationController : RotationController, IInputReceiver
    {
        [SerializeField] private float cameraSpeed;
        [SerializeField] private Vector2Input rotationInput;
        [SerializeField] private float verticalRange;
        
        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            rotationInput.RegisterInput(OnRotationInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            rotationInput.DeregisterInput(OnRotationInputChange, playerIdentifier);
        }

        private void OnRotationInputChange(Vector2 input)
        {
            RotateCamera(input.y);
            RotateTowards(input.x);
        }
        #endregion

        private void RotateCamera(float direction)
        {
            var rotation = transform.rotation.eulerAngles;
            rotation.x -= direction * cameraSpeed;
            if (rotation.x < 180 && rotation.x > verticalRange)
                rotation.x = verticalRange;
            if (rotation.x > 180 && rotation.x < 360 - verticalRange)
                rotation.x = 360 - verticalRange;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
