using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmenumanager : MonoBehaviour
{

    [SerializeField] Button findGameButton;
    [SerializeField] Button singleButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button volverButton;


    [SerializeField] GameObject panelControl;
    [SerializeField] GameObject panelMain;

    private void Awake()
    {
        findGameButton.onClick.AddListener(FindGame);
        singleButton.onClick.AddListener(Singlemode);
        controlsButton.onClick.AddListener(controls);
        tutorialButton.onClick.AddListener(tutorial);
        exitButton.onClick.AddListener(exit);
        volverButton.onClick.AddListener(SetInvisibleControls);

    }

    private void SetInvisibleControls()
    {
        panelControl.SetActive(false);
        panelMain.SetActive(true);
        
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FindGame()
    {
        Time.timeScale = 1;
        GameManager.singlePlayer = true;
        SceneManager.LoadScene("InGame");
         

    }
    
    private void exit()
    {
        Application.Quit();
    }

    private void tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void controls()
    {
        panelMain.SetActive(false);
        panelControl.SetActive(true);
        controlsButton.enabled = true;
    }

    private void Singlemode()
    {
        Time.timeScale = 1;
        GameManager.singlePlayer = false;
        SceneManager.LoadScene("InGame");
    }
}
