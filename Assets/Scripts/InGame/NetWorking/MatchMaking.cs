// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to connect, and join/create room automatically
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

#pragma warning disable 649
namespace Mannaz.InGame
{
    /// <summary>
    /// Launch manager. Connect, join a random room or create one if none or all full.
    /// </summary>
    public class MatchMaking : MonoBehaviourPunCallbacks
    {
        #region Attributes

        public MatchmakingUIController uiMatchmaking;
        
        private byte maxPlayersPerRoom = 2;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        #endregion

        #region Photon CallBacks        
        // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


        /// <summary>
        /// Called after the connection to the master is established and authenticated
        /// </summary>
        public override void OnConnectedToMaster()
        {            
            if (isConnecting)
            {
                uiMatchmaking.LogFeedback("Connected to Master... ");
                Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
                JoinRoom();
            }
        }

        /// <summary>
        /// Called after disconnecting from the Photon server.
        /// </summary>
        public override void OnDisconnected(DisconnectCause cause)
        {
            uiMatchmaking.LogFeedback("<color=#DB1E1E>OnDisconnected</color> " + cause);
            Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

            // #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.

            isConnecting = false;

        }




        /// <summary>
        /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
        /// </summary>
        /// <remarks>
        /// This method is commonly used to instantiate player characters.
        /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
        ///
        /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
        /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
        /// enough players are in the room to start playing.
        /// </remarks>
        public override void OnJoinedRoom()
        {
            //Ready to play

            uiMatchmaking.LogFeedback("<color=#31E524>Current Room: </color> " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

            // #Critical: We only load if we are the first player, else we rely on PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
            NetworkManager.instance.PlayerConected();

            //For next conexion start trying from room 0
            roomNum = 0;
        }

        /// <summary>
        /// When room are full or ther are an error in the conection, twy next room.
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            roomNum++;

            Debug.LogError("Join failed, " + message);
            uiMatchmaking.LogFeedback("Trying to joining " + roomNum + " Room...");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinOrCreateRoom(roomNum.ToString(), new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }

        #endregion

        #region Public Methods
      
        static int roomNum = 1;//TODO: ordenar esto y no creo que sirva para cientos de jugadores, sería muy lenta la conexión...                            
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            roomNum++;

            Debug.LogError("Join failed, " + message);
            uiMatchmaking.LogFeedback("Creating " + roomNum + " Room...");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinOrCreateRoom(roomNum.ToString(), new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }

        #endregion

        #region Private Methods
        public void Connect()
        {

            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;

            // Conecting effects
            uiMatchmaking.gameObject.SetActive(true);


            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)//Lobby
            {
                JoinRoom();
            }
            else
            {

                uiMatchmaking.LogFeedback("Connecting...");

                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = Application.version;

                //Conect to Master
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void JoinRoom()
        {
            uiMatchmaking.LogFeedback("Trying to joining Random Room...");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }

        #endregion


    }
}