using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dodge_Bots
{
    public class BallChargeUI : MonoBehaviour, IObserver<Ball.BallState>
    {
        private ProgressBar bar;

        private const string chargeBar = "charge_bar";

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.AddToClassList("container");
            BuildDocument(root);
            bar.lowValue = 0;
            bar.highValue = 1;
            bar.value = bar.highValue;
        }

        private void Start()
        {
            var player = FindObjectOfType<Player>();
            var ball = player.GetComponentInChildren<BallController>();
            ball.AddObserver(this);
        }

        private void BuildDocument(VisualElement root)
        {
            bar = root.CreateChild<ProgressBar>(chargeBar);
        }

        #region Notifications
        public void OnNotify(Ball.BallState argument)
        {
            bar.value = argument.Charge / argument.MaxCharge;
        }
        #endregion
    }
}
