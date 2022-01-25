using Cinemachine;
using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
   internal InGameController gameController => InGameController.instance;
   internal UIController uiController => gameController.uIController;
   internal CharacterController player => gameController.player;

   internal AudioManager audioManager => gameController.audioManager;
   
   internal CinemachineVirtualCamera cinemachine => gameController.cinemachine;

    internal NetworkManager networkManager => NetworkManager.instance;
    internal Crono crono => gameController.crono;


}
