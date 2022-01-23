using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    public UIController uIController;
    public NetworkManager networkManager;
    public CharacterController player;
    public AudioManager audioManager;

    public static InGameController instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
