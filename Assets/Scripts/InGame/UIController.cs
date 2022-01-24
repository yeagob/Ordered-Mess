using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("Texts")]
    public TextMeshProUGUI rondaText;
    public TextMeshProUGUI cronoText;
    public TextMeshProUGUI mensajeInicioRonda;
    public TextMeshProUGUI localPlayerNameText;
    public TextMeshProUGUI rivalPlayerNameText;

    [Header("Buttons")]
    public Button exitButton;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string localPlayer, string otherPlayer)
    {
        localPlayerNameText.text = localPlayer;
        rivalPlayerNameText.text = otherPlayer;
    }
}
