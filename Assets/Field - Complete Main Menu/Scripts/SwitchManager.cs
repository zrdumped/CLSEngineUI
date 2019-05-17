using UnityEngine;

namespace Michsky.UI.FieldCompleteMainMenu
{
    public class SwitchManager : MonoBehaviour
    {
        [Header("SWITCH")]
        public bool isOn;
        public Animator switchAnimator;

        public GameObject house;

        private string onTransition = "Switch On";
        private string offTransition = "Switch Off";

        void Start()
        {
            if (isOn == true)
            {
                switchAnimator.Play(onTransition);
                house.SetActive(true);
            }

            else
            {
                switchAnimator.Play(offTransition);
                house.SetActive(false);
            }
        }

        public void AnimateSwitch()
        {
            if (isOn == true)
            {
                switchAnimator.Play(offTransition);
                house.SetActive(false);
                isOn = false;

            }

            else
            {
                switchAnimator.Play(onTransition);
                house.SetActive(true);
                isOn = true;
            }
        }
    }
}