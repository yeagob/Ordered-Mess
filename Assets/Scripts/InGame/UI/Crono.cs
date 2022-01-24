using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crono : MonoBehaviour
{
    private float totalTimer = ProjectSettings.countdownRoundTime;
    public bool timerIsRunning = false;
    public TextMeshProUGUI cronoText;

    InGameController inGameController;

    void Awake()
    {
        Debug.Log("Awake");
        InGameController.instance.OnStartGame += StartCrono;
        // Starts the timer automatically
        //timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (totalTimer > 0)
            {
                totalTimer -= Time.deltaTime;
                DisplayTime(totalTimer);
            }
            else
            {
                Debug.Log("Time has run out!");
                totalTimer = 0;
                timerIsRunning = false;
            }
        }
    }

    public void StartCrono()
    {
        timerIsRunning = true;
        Debug.Log("Timer is: " + timerIsRunning);
    }
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        cronoText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
