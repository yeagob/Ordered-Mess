using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mannaz.InGame
{
    public class MatchmakingUIController : MonoBehaviour
    {

        //TODO: add cancel Button.

        [Tooltip("The Ui Text to inform the user about the connection progress")]
        [SerializeField]
        private TextMeshProUGUI feedbackText;

        [Header("Buttons")]
        [SerializeField] Button cancelRoomBtn;

        [SerializeField]
        private ScrollRect searchTextScroll;

        [SerializeField] private ParticleSystem backgroundParticles;
        [SerializeField] private GameObject explosionRuneParticles;
        [SerializeField] private GameObject endExplosionParticles;
        [SerializeField] private GameObject chargingParticles;

        [Header("UI Versus")]
        [SerializeField] GameObject panelVersus;
        [SerializeField] GameObject particlesAndEffectsForVersus;


        private void Start()
        {
            backgroundParticles.gameObject.SetActive(true);
            particlesAndEffectsForVersus.SetActive(false);
            panelVersus.SetActive(false);
            cancelRoomBtn.onClick.AddListener(NetworkManager.instance.BackToLobby);
        }

        #region Animation Events
        public void ShowExplosion()
        {
            explosionRuneParticles.SetActive(true);
            //TODO: Añadir sonido
        }

        public void ShowEndExplosion()
        {
            endExplosionParticles.SetActive(true);
        }

        public void ShowChargingParticles()
        {
            chargingParticles.SetActive(true);
        }

        public void AccelerateBGParticles()
        {
            ParticleSystem.ForceOverLifetimeModule forceOverLifetime = backgroundParticles.forceOverLifetime;
            forceOverLifetime.y = 20;
        }

        public void ResetBGParticles()
        {
            ParticleSystem.ForceOverLifetimeModule forceOverLifetime = backgroundParticles.forceOverLifetime;
            forceOverLifetime.y = 3.23f;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
        /// </summary>
        /// <param name="message">Message.</param>
        internal void LogFeedback(string message)
        {
            // we do not assume there is a feedbackText defined.
            if (feedbackText == null)
            {
                return;
            }

            Debug.Log(message);

            // add new messages as a new line and at the bottom of the log.
            feedbackText.text += System.Environment.NewLine + message;

            // make the scroll go to the bottom position so the new line of text is visible and the other lines go up.
            Canvas.ForceUpdateCanvases();
            //UI trick  to center text
            searchTextScroll.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
        }


        internal IEnumerator StartAnimationAndShowVersus()
        {
            cancelRoomBtn.gameObject.SetActive(false);
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("StartGame");
            float animationDuration = anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime + 1f;
            
            yield return new WaitForSeconds(animationDuration);
            
            gameObject.SetActive(false);
            //particlesAndEffectsForVersus.SetActive(true);
           
            //TODO: Refactor!!
            //Start Versus processs Animation & StartGame
            panelVersus.SetActive(true);

        }
        #endregion
    }
}