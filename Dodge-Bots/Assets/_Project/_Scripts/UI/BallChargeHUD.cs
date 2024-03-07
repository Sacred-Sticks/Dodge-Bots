using Kickstarter.DependencyInjection;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dodge_Bots
{
    public class BallChargeHUD : MonoBehaviour, IDependencyProvider
    {
        private BallController ballController;
        [Inject] private BallController BallController
        {
            get => ballController;
            set
            {
                ballController = value;
                ballController.OnChargeChange += SetHUDCharge;
            }
        }

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

        private void BuildDocument(VisualElement root)
        {
            bar = root.CreateChild<ProgressBar>(chargeBar);
        }

        private void SetHUDCharge(float argument)
        {
            bar.value = argument;
        }
    }
}
