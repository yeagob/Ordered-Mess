using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    [Header("Controllers")]
    public UIController uIController;
    public NetworkManager networkManager;
    public CharacterController player;
    public AudioManager audioManager;
    public CinemachineVirtualCamera cinemachine;

    [Header("Key Objects")]
    public GameObject playerPrefab;
    public Transform spawnPlayer1;
    public Transform spawnPlayer2;

    public static InGameController instance;

    internal event Action OnStartGame;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        networkManager.OnNetworkStartGame += StartGame;

        if (networkManager.multiplayerOn)
            Destroy(player.gameObject);
    }

    internal void StartGame()
    {
        //Player Instantiation
        if (PhotonNetwork.IsMasterClient)
            player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer1.transform.position, playerPrefab.transform.rotation).GetComponent<CharacterController>();
                else
            player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer2.transform.position, playerPrefab.transform.rotation).GetComponent<CharacterController>();

        //Cinemachine target

        if (OnStartGame != null)
            OnStartGame();

    }
}
