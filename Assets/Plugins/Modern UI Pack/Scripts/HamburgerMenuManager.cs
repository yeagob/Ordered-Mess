using UnityEngine;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class HamburgerMenuManager : MonoBehaviour
    {
        private Animator menuAnimator;

        [Header("RESOURCES")]
        public Animator animatedButton;
        public TextMeshProUGUI title;

        [Header("SETTINGS")]
        public bool openAtStart = false;
        public string titleAtStart;

        public bool isOpen;

        private void Awake()
        {
            menuAnimator = gameObject.GetComponent<Animator>();
        }

        void Start()
        {

            if (titleAtStart != string.Empty)
                title.text = titleAtStart;

            if(openAtStart == true)
            {
                menuAnimator.Play("Expand");
                if (animatedButton != null)
                    animatedButton.Play("HTE Expand");
                isOpen = true;
            }

            else
            {
                menuAnimator.Play("Minimize");
                if (animatedButton != null)
                    animatedButton.Play("HTE Hamburger");
                isOpen = false;
            }
        }

        public void Animate()
        {
            if (isOpen == true)
            {
                menuAnimator.Play("Minimize");
                if (animatedButton != null)
                    animatedButton.Play("HTE Hamburger");
                isOpen = false;
            }

            else
            {          
                menuAnimator.Play("Expand");
                if (animatedButton != null)
                    animatedButton.Play("HTE Exit");
                isOpen = true;
            }
        }

        public void ChangeText(string titleText)
        {
            title.text = titleText;
        }
    }
}