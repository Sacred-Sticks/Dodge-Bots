using Kickstarter.Observer;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dodge_Bots
{
    public class BallChargeUI : MonoBehaviour, IObserver<Ball.BallState>
    {
        private VisualElement bar;

        private const string positioner = "positioner";
        private const string chargeContainer = "chargeContainer";
        private const string chargeBar = "chargeBar";

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.AddToClassList("container");
            BuildDocument(root);
        }

        private void Start()
        {
            var ball = FindObjectOfType<BallController>();
            ball.AddObserver(this);
        }

        private void BuildDocument(VisualElement root)
        {
            bar = root.CreateChild(positioner).CreateChild(chargeContainer).CreateChild(chargeBar);
        }

        #region Notifications
        public void OnNotify(Ball.BallState argument)
        {
            bar.style.flexGrow = argument.Charge / argument.MaxCharge;
        }
        #endregion
    }
}
