using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MyMonoBehaviour
{
    internal bool round1 = true;
    public static event Action OnGameCompleted;
    public static event Action On10SeconsLeft;

    // Start is called before the first frame update
    void Start()
    {
        crono.OnCronoCompleted += CronoCompleted;
        uiController.mensajeInicioRonda.text = "Now sort out the house!";
    }

    private void Update()
    {
        if (crono.totalTimer <= 10)
            if (On10SeconsLeft != null)
                On10SeconsLeft();
    }

    private void CronoCompleted()
    {
        if (round1)
        {
            StartCoroutine(uiController.mostrarMnsInicial());

            //Message
            uiController.mensajeInicioRonda.text = "Now make a mess!";
            StartCoroutine(uiController.ShowTotalRoundPointsText());
            if (PhotonNetwork.IsMasterClient)
                uiController.totalRoundPointsText.text = "" + InGameController.instance.pointFirstRoundPlayer1 + " points";
            else
                uiController.totalRoundPointsText.text = "" + InGameController.instance.pointFirstRoundPlayer2 + " points";

            uiController.rondaText.text = "Round: 2/2";
            crono.totalTimer = ProjectSettings.countdownRoundTime;
            crono.timerIsRunning = true;
            round1 = false;
        }
        else
        {
            StartCoroutine(uiController.mostrarMnsInicial());
            uiController.mensajeInicioRonda.text = "Game Over";

            StartCoroutine(uiController.ShowTotalRoundPointsText());
            if (PhotonNetwork.IsMasterClient)
                uiController.totalRoundPointsText.text = "" + InGameController.instance.pointSecondRoundPlayer1 + " points";
            else
                uiController.totalRoundPointsText.text = "" + InGameController.instance.pointSecondRoundPlayer2 + " points";

            StartCoroutine(uiController.ShowGameOverPanel());

            if (OnGameCompleted != null)
                OnGameCompleted();
        }
    }
}
