using System.Collections;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Mannaz.InGame;
using System;

    [RequireComponent(typeof(PhotonView))]
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Attributes

        /// <summary>
        /// Singleton Static instance of NetworkManager
        /// </summary>
        public static NetworkManager instance;               


        public bool multiplayerOn;
        public MatchMaking matchMaking;
        public RpcTarget rpcTarget;
        
        MatchmakingUIController uiMatchmaking => matchMaking.uiMatchmaking;
        GameObject panelMatchmajing => uiMatchmaking.gameObject;

        private IEnumerator disconectCountdown;

        public event Action OnNetworkStartGame;

    PhotonView photonView;

        #endregion

        #region Unity Callbacks
        /// <summary>
        /// Simples singleton.
        /// </summary>
        private void Awake()
        {
            instance = this;

            photonView = GetComponent<PhotonView>();
            panelMatchmajing.SetActive(false);

            if (GameManager.singlePlayer)
                multiplayerOn = true;
            else 
                multiplayerOn = false;
        }

        private void Start()
        {
            if (multiplayerOn)
            {
                matchMaking.Connect();
                panelMatchmajing.SetActive(true);
            }
            
        }

        /// <summary>
        /// Called when mi conection to the room is completed and im ready to play
        /// </summary>
        public void PlayerConected()
        {
            Debug.Log("PLAYER CONECTED");

            //PLAYER 1
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PLAYER 1");
                uiMatchmaking.LogFeedback("Waiting for player 2...");                                                                                                                                                           
            }

            //SI HAY 2 PLAYERS START GAME!!
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                Debug.Log("PLAYER 2");
                photonView.RPC(nameof (StartGame), NetworkManager.instance.rpcTarget, ProfileControl.playerProfile.playerName);
                StartLocalGame();
            }
        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when a Photon Player got connected. We need to then load a bigger scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            //SI LA CORUTINA NO ESTA ASIGNADA, SE PARA Y SE PASA A NULL
            if(disconectCountdown != null)
            {
                StopCoroutine(disconectCountdown);
                disconectCountdown = null; 
            }

        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            disconectCountdown = DisconectAferSeconds();

            StartCoroutine(disconectCountdown);
        }


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            BackToLobby();
        }

        internal void BackToLobby()
        {
            PhotonNetwork.LeaveRoom();
            GameManager.instance.ChangeScene(Scenes.MainMenu);
        }


        #endregion

        #region Methods

        

        void StartLocalGame()
        {
            panelMatchmajing.SetActive(false);
            Debug.Log("RPC Start Game");            
            //Event
            if (OnNetworkStartGame != null)
                OnNetworkStartGame();
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        IEnumerator DisconectAferSeconds()
        {

            yield return new WaitForSeconds(ProjectSettings.countdownAfterLeaveMatch);

            BackToLobby();

        }

    internal void EndGame(int points)
    {
        photonView.RPC(nameof(EndGameRPC), RpcTarget.Others, points);
    }

    #endregion

    #region RPC Methods



    [PunRPC]
    public void EndGameRPC(int points)

    {
        if (InGameController.instance.calculatePointPlayer1 > points)
            InGameController.instance.uIController.playerWonText.text = "YOU WIN";
        else
            InGameController.instance.uIController.playerWonText.text = "YOU LOSE";
    }
        [PunRPC]
        public void StartGame(string oponentName)
        {
            //InGameController.instance.uiController.ShowOponentName(oponentName, oponentSchoolName);

            photonView.RPC( nameof(SendInfoBack), rpcTarget, ProfileControl.playerProfile.playerName);
            panelMatchmajing.SetActive(false);
            Debug.Log("RPC Start Game");
            //Event
            if (OnNetworkStartGame != null)
                OnNetworkStartGame();          
        }

        [PunRPC]
        public void SendInfoBack(string oponentName)
        {
            //TODO: show eneme name
            //StartCoroutine(uiMatchmaking.StartAnimationAndShowVersus());
            //InGameController.instance.uiController.ShowOponentName(oponentName);

        }
        #endregion

    }
