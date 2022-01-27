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
    public Crono crono;
    private int pointFirstRoundPlayer1;
    private int pointFirstRoundPlayer2;
    



    [Header("Key Objects")]
    public GameObject playerPrefab;
    public Transform spawnPlayer1;
    public Transform spawnPlayer2;

    public static InGameController instance;

    public event Action OnStartGame;

    [Header("List")]
    [SerializeField] private List<Room> roomsPlayer1 = new List<Room>();
    [SerializeField] private List<Room> roomsPlayer2 = new List<Room>();

    //Main tools
    [SerializeField] public RoundController roundController;
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

        //events
        crono.OnCronoCompleted += LoadPointsPlayers;
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

    public void LoadPointsPlayers()
    {
        int calculatePointPlayer1 = 0;
        int calculatePointPlayer2 = 0;
        foreach(Room room in roomsPlayer1)
        {
            foreach (GameObject prop in room.roomHouseProps) 
            {
                  calculatePointPlayer1 += prop.GetComponent<HouseProps>()._amountPoints;
                  pointFirstRoundPlayer1 = calculatePointPlayer1;
            }
        }        
            print("Player_1 total points" + calculatePointPlayer1);
        foreach(Room room in roomsPlayer2)
        {
            foreach (GameObject prop in room.roomHouseProps) 
            { 
                  calculatePointPlayer2 += prop.GetComponent<HouseProps>()._amountPoints;
                  pointFirstRoundPlayer2 = calculatePointPlayer2;
            }
        }

            print("Player_2 total points" + calculatePointPlayer2);
        if (roundController.sortingOut && pointFirstRoundPlayer1 != 0 && pointFirstRoundPlayer2 != 0)
        {
           
            calculatePointPlayer1 = pointFirstRoundPlayer1 - calculatePointPlayer1;
            calculatePointPlayer2 = pointFirstRoundPlayer2 - calculatePointPlayer2;
        }
    }
}
