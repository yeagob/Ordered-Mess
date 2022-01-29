using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mainmenumanager : MonoBehaviour
{

    [SerializeField] Button findGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button exitButton;
    private void Awake()
    {
        findGameButton.onClick.AddListener(FindGame);
        optionsButton.onClick.AddListener(options);
        tutorialButton.onClick.AddListener(tutorial);
        exitButton.onClick.AddListener(exit);
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
        SceneManager.LoadScene("InGame");
         

    }
    
    private void exit()
    {
        Application.Quit();
    }

    private void tutorial()
    {

    }

    private void options()
    {

    }

}
