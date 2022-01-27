using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIController : MyMonoBehaviour
{

    public int objetosTotales;

    [Header("Panels")]
    public GameObject inGameUI;
    public GameObject networkUI;
    public GameObject gameOverPanel;
    public GameObject superiorPanel;
    public GameObject playersPanel;

    [Header("Texts")]
    public TextMeshProUGUI rondaText;
    public TextMeshProUGUI cronoText;
    public TextMeshProUGUI mensajeInicioRonda;
    public TextMeshProUGUI localPlayerNameText;
    public TextMeshProUGUI otherPlayerNameText;
    public TextMeshProUGUI localPlayerObjectsText;
    public TextMeshProUGUI otherPlayerObjectsText;
    public TextMeshProUGUI objetosTotalesText;
    public TextMeshProUGUI totalRoundPointsText;
    public TextMeshProUGUI playerWonText;

    [Header("Buttons")]
    public Button exitButton;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (networkManager.multiplayerOn)
        {
            inGameUI.SetActive(false);
            networkUI.SetActive(true);
        }
        else
        {
            StartGame();
            //Mostrar mesnaje inicio ronda. Corutina que se desactive a los 3 seg, el mensaje
            StartCoroutine("mostrarMnsInicial");
        }
    }

    public IEnumerator mostrarMnsInicial()
    {
        mensajeInicioRonda.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        mensajeInicioRonda.gameObject.SetActive(false);

    }

    internal void StartGame()
    {
        inGameUI.SetActive(true);
        networkUI.SetActive(false);
        mensajeInicioRonda.gameObject.SetActive(false);
    }

    public void SetPlayerName(string localPlayer, string otherPlayer)
    {
        localPlayerNameText.text = localPlayer;
        otherPlayerNameText.text = otherPlayer;
    }

    public void SetObjsTotalesText(int numObjects)
    {
        objetosTotales = numObjects;
        objetosTotalesText.text = objetosTotales.ToString();
    }

    public IEnumerator ShowTotalRoundPointsText()
    {
        totalRoundPointsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        totalRoundPointsText.gameObject.SetActive(false);
    }

    public IEnumerator ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 5);
        superiorPanel.GetComponent<CanvasGroup>().DOFade(0, 1);
        playersPanel.GetComponent<CanvasGroup>().DOFade(0, 1);

        totalRoundPointsText.transform.parent = transform;
        mensajeInicioRonda.transform.parent = transform;

        //TODO: playerWonText.text = "";

        yield return new WaitForSeconds(3);

        playerWonText.gameObject.SetActive(true);
        mensajeInicioRonda.gameObject.SetActive(true);
        totalRoundPointsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        //Back to main menu
        SceneManager.LoadScene(0);
    }
}
