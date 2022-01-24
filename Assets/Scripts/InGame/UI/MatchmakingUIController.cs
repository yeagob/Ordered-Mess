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


        private void Start()
        {
            cancelRoomBtn.onClick.AddListener(NetworkManager.instance.BackToLobby);
        }

        
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

        #endregion
    }
}