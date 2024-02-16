using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Dodge_Bots
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour, Kickstarter.Observer.IObserver<LocomotionController.Event>,
        Kickstarter.Observer.IObserver<LocomotionController.MovementChange>
    {
        private Animator animator;
        private readonly int VelocityX = Animator.StringToHash("Velocity X");
        private readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        private readonly int Grounded = Animator.StringToHash("Grounded");
        private const float LayerChangeTime = .125f;

        private Direction fallingArmsLayerStatus = Direction.Static;
        private Coroutine raisingArms;
        private Coroutine loweringArms;

        #region UnityEvents
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            transform.root.GetComponentInChildren<LocomotionController>().AddObserver(this);
            transform.root.GetComponentInChildren<TrampolineCollider>().AddObserver(this);
        }
        #endregion

        private IEnumerator SetLayerWeight(int layerIndex, float targetWeight)
        {
            float initialWeight = animator.GetLayerWeight(layerIndex);
            float weightDifference = targetWeight - initialWeight;
            float timePassed = 0;
            float transitionTime = Mathf.Abs(LayerChangeTime * weightDifference);
            while (timePassed < transitionTime)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                timePassed += Time.deltaTime;
                animator.SetLayerWeight(layerIndex, initialWeight + weightDifference * Time.deltaTime);
            }
            animator.SetLayerWeight(layerIndex, targetWeight);
        }

        #region Notifications
        public void OnNotify(LocomotionController.Event argument)
        {
            switch (argument)
            {
                case LocomotionController.Event.Jump:
                case LocomotionController.Event.Airborne:
                    animator.SetBool(Grounded, false);
                    if (fallingArmsLayerStatus != Direction.Up)
                        RaiseArms();
                    break;
                case LocomotionController.Event.Grounded:
                    animator.SetBool(Grounded, true);
                    if (fallingArmsLayerStatus != Direction.Down)
                        LowerArms();
                    break;
            }
        }

        public void OnNotify(LocomotionController.MovementChange argument)
        {
            animator.SetFloat(VelocityX, argument.LocalDirection.x);
            animator.SetFloat(VelocityZ, argument.LocalDirection.z);
        }
        #endregion

        private void RaiseArms()
        {
            fallingArmsLayerStatus = Direction.Up;
            float changeRate = 1 / LayerChangeTime;
            if (loweringArms != null)
                StopCoroutine(loweringArms);
            raisingArms = StartCoroutine(LerpLayerWeight(1, changeRate, 1));
        }

        private void LowerArms()
        {
            fallingArmsLayerStatus = Direction.Down;
            float changeRate = -1 / LayerChangeTime;
            if (raisingArms != null)
               StopCoroutine(raisingArms);
            loweringArms = StartCoroutine(LerpLayerWeight(1, changeRate, 0));
        }

        private IEnumerator LerpLayerWeight(int targetLayerIndex, float adjustmentRate, float targetWeight)
        {
            Func<float, float, bool> comparison = (f1, f2) => adjustmentRate > 0 ? f1 < f2 : f1 > f2;
            float initialWeight = animator.GetLayerWeight(targetLayerIndex);
            float currentWeight = initialWeight;
            while (comparison(currentWeight, targetWeight))
            {
                animator.SetLayerWeight(targetLayerIndex, currentWeight);
                yield return new WaitForSeconds(Time.deltaTime);
                currentWeight += adjustmentRate * Time.deltaTime;
            }
            animator.SetLayerWeight(targetLayerIndex, targetWeight);
        }
        
        private enum Direction
        {
            Static,
            Up,
            Down,
        }
    }
}
