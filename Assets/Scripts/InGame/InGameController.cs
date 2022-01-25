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

    public event Action OnStartGame;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (networkManager.multiplayerOn)
        {
            networkManager.OnNetworkStartGame += StartGame;
        }
        else
            StartGame();
    }

    internal void StartGame()
    {
        if (networkManager.multiplayerOn)
        {
            //Destroy Editor Player
            Destroy(player.gameObject);

            //Netwokr Player Instantiation
            if (PhotonNetwork.IsMasterClient)
                player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer1.transform.position, playerPrefab.transform.rotation).GetComponent<CharacterController>();
                    else
                player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer2.transform.position, playerPrefab.transform.rotation).GetComponent<CharacterController>();
        }

        //Cinemachine target
        cinemachine.m_Follow = player.hipTransform;
        cinemachine.m_LookAt = player.hipTransform;

        //UI Init
        uIController.StartGame();

        if (OnStartGame != null)
            OnStartGame();

    }
}
