using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
    public class PBFilled : MonoBehaviour
    {
        public ProgressBar proggresBar;

        [Header("SETTINGS")]
        public Animator barAnimatior;
        [Range(0, 1)] public float transitionAfter = .5f;
        private string animText = "Radial PB Filled";
        private string emptyAnimText = "Radial PB Empty";

        void Update()
        {
            if (proggresBar.specifiedValue >= transitionAfter)
            {
                barAnimatior.Play(animText);
            }

            if (proggresBar.specifiedValue <= transitionAfter)
            {
                barAnimatior.Play(emptyAnimText);
            }
        }
    }
}