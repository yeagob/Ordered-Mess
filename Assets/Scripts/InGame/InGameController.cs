using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    [Header("Controllers")]
    //Main tools
    public UIController uIController;
    public NetworkManager networkManager;
    public CharacterController player;
    public AudioManager audioManager;
    public CinemachineVirtualCamera cinemachine;
    public RoundController roundController;
    public Crono crono;
    internal GameObject hip => player.hipTransform.gameObject;

    [Header("Key Objects")]
    public GameObject playerPrefab;
    public Transform spawnPlayer1;
    public Transform spawnPlayer1Round2;
    public Transform spawnPlayer2;
    public Transform spawnPlayer2Round2;

    [Header("Key Objects")]
    public GameObject housePlayer2;
    public GameObject wallHousePlayer2;
    public GameObject doorPlayer1;
    public GameObject doorPlayer2;

    [Header("Player Rooms Lists")]
    [SerializeField] private List<Room> roomsPlayer1 = new List<Room>();
    [SerializeField] private List<Room> roomsPlayer2 = new List<Room>();

    [Header("Player Colors")]
    public Material pinkMat;
    public Material blueMat;

    //POINTS
    internal int pointFirstRoundPlayer1;
    internal int pointFirstRoundPlayer2;
    internal int pointSecondRoundPlayer1;
    internal int pointSecondRoundPlayer2;
    internal int calculatePointPlayer1 = 0;
    internal int calculatePointPlayer2 = 0;


    internal bool singlePlayer => !networkManager.multiplayerOn;

    public static InGameController instance;

    public event Action OnStartGame;

    public event Action OnStartTutorial;

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
            if (PhotonNetwork.IsMasterClient)
            {
                doorPlayer2.SetActive(true);
                doorPlayer1.SetActive(false);
            }
            else
            {
                doorPlayer2.SetActive(false);
                doorPlayer1.SetActive(true);
            }
        }
        //SINGLE PLAYER
        else
        {
            doorPlayer1.SetActive(false);
            housePlayer2.SetActive(false);
            wallHousePlayer2.SetActive(true);

            StartGame();

        }

        //events
        crono.OnCronoCompleted += CalculatePointsPlayers;
        crono.OnCronoCompleted += ChangePosPlayersRound2;
    }

    private void ChangePosPlayersRound2()
    {
        doorPlayer2.SetActive(true);
        doorPlayer1.SetActive(true);
        if (singlePlayer)
        {
            player.hipTransform.transform.position = spawnPlayer2Round2.transform.position;
            return;
        }
        //Player1
        if (!networkManager.multiplayerOn || PhotonNetwork.IsMasterClient)
            player.hipTransform.position = spawnPlayer1Round2.transform.position;
        else
            //Player2
            player.hipTransform.position = spawnPlayer2Round2.transform.position;
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

    public void CalculatePointsPlayers()
    {
        //Round 1
        if (roundController.round1)
        {
            foreach(Room room in roomsPlayer1)
            {
                foreach (GameObject prop in room.roomHouseProps) 
                {
                    if (!prop.GetComponent<HouseProps>().onlyRound2)
                      calculatePointPlayer1 += prop.GetComponent<HouseProps>()._amountPoints;
                }
                pointFirstRoundPlayer1 = calculatePointPlayer1;
            }        
            print("Player_1 total points" + calculatePointPlayer1);
            
            //Multiplayer
            if (!singlePlayer)
            {
                foreach(Room room in roomsPlayer2)
                {
                    foreach (GameObject prop in room.roomHouseProps) 
                    { 
                        calculatePointPlayer2 += prop.GetComponent<HouseProps>()._amountPoints;
                        //Reset points
                        prop.GetComponent<HouseProps>()._amountPoints = 0;
                    }
                    pointFirstRoundPlayer2 = calculatePointPlayer2;
                }
                print("Player_2 total points" + calculatePointPlayer2);
            }
        }
        //Round 2
        else
        {
            if (singlePlayer)
            {
                foreach (Room room in roomsPlayer1)
                {
                    foreach (GameObject prop in room.roomHouseProps)
                    {
                        calculatePointPlayer1 += prop.GetComponent<HouseProps>()._amountPoints;
                    }
                    pointSecondRoundPlayer1 = calculatePointPlayer1;
                }
                print("Player_1 total points" + calculatePointPlayer1);
            }
            //Multiplayer
            else
            {
                foreach (Room room in roomsPlayer2)
                {
                    foreach (GameObject prop in room.roomHouseProps)
                    {
                        calculatePointPlayer1 += prop.GetComponent<HouseProps>()._amountPoints;
                    }
                    pointSecondRoundPlayer1 = calculatePointPlayer1;
                }
                print("Player_1 total points" + calculatePointPlayer1);
                foreach (Room room in roomsPlayer1)
                {
                    foreach (GameObject prop in room.roomHouseProps)
                    {
                        calculatePointPlayer2 += prop.GetComponent<HouseProps>()._amountPoints;
                    }
                    pointSecondRoundPlayer2 = calculatePointPlayer2;
                }
                print("Player_2 total points" + calculatePointPlayer2);
                
                calculatePointPlayer2 = pointFirstRoundPlayer2 + pointSecondRoundPlayer2;
            }

            //Final Result!!
            calculatePointPlayer1 = pointFirstRoundPlayer1 + pointSecondRoundPlayer1;

            if (singlePlayer)
            {
                uIController.playerWonText.text = "GOOD GAME";
                
            }
            else
            {
                networkManager.EndGame(calculatePointPlayer1);
            }
        }
    }
}
