using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MyMonoBehaviour
{

    public int objetosTotales;

    [Header("Panels")]
    public GameObject inGameUI;
    public GameObject networkUI;

    [Header("Texts")]
    public TextMeshProUGUI rondaText;
    public TextMeshProUGUI cronoText;
    public TextMeshProUGUI mensajeInicioRonda;
    public TextMeshProUGUI localPlayerNameText;
    public TextMeshProUGUI otherPlayerNameText;
    public TextMeshProUGUI localPlayerObjectsText;
    public TextMeshProUGUI otherPlayerObjectsText;
    public TextMeshProUGUI objetosTotalesText;

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

    IEnumerator mostrarMnsInicial()
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
        objetosTotalesText.text = numObjects.ToString();
    }
}
