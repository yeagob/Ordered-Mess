using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MyMonoBehaviour
{
    public bool sortingOut;
    public event Action OnGameCompleted;

    // Start is called before the first frame update
    void Start()
    {
        crono.OnCronoCompleted += CronoCompleted;
        uiController.mensajeInicioRonda.text = "Now sort out the house!";
    }

    private void CronoCompleted()
    {
        if (sortingOut)
        {
            StartCoroutine(uiController.mostrarMnsInicial());

            //Message
            if(PhotonNetwork.IsMasterClient)
                uiController.mensajeInicioRonda.text = "Now make a mess!" + InGameController.instance.pointFirstRoundPlayer1;
            else
                uiController.mensajeInicioRonda.text = "Now make a mess!" + InGameController.instance.pointFirstRoundPlayer2;

            uiController.rondaText.text = "Round: 2/2";
            crono.totalTimer = ProjectSettings.countdownRoundTime;
            crono.timerIsRunning = true;
            sortingOut = !sortingOut;
        }
        else
        {
            StartCoroutine(uiController.mostrarMnsInicial());
            uiController.mensajeInicioRonda.text = "Game Over";

            if (OnGameCompleted != null)
                OnGameCompleted();
        }
    }
}
