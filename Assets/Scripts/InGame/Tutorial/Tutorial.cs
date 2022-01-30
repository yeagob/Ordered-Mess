using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    internal int phase = 0;

    public GameObject player;

    public GameObject puerta;

    public Transform tpPosition;

    public ObjectsSpawner _ObjectSpawner;

    public static Tutorial instance;

    //Menu
    public TextMeshProUGUI tutorialLabel;


    public event Action OnPhase1Collision;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        GoNextPhase();

    }

    public void GoNextPhase()
    {
        phase++;

        switch (phase)
        {
            case 1:

                tutorialLabel.text = "First,use W A S D and fo to the superior wal";

                break;
            case 2:

                tutorialLabel.text = "Try to run to the other side of the yard using Shift";

                break;
            case 3:

                tutorialLabel.text = "Grab the object using left click and take it to the Main Room";
                puerta.SetActive(false);
                _ObjectSpawner.SpawnTutorialObject();

                break;
            case 4:

                tutorialLabel.text = "Rotate the chair with the mouse wheel and release the click to drop it";

                break;
            case 5:

                StartCoroutine("waitOneSecond");
                tutorialLabel.text = "The number floating is the ammount of points you will get at the end of the round.";

                break;

            case 6:

                StartCoroutine("waitOneSecond");
                tutorialLabel.text = "The ammount depends of the position of the chair. Now it's time to make some chaos";

                break;

            case 7:

                player.GetComponent<CharacterController>().hipTransform.position = tpPosition.position;

                tutorialLabel.text = "Grab the coffe table and throw it to other room with the Right Click";
                break;

            case 8:

                tutorialLabel.text = "Congratulations!!! You are ready.";

                break;


        }


    }

    IEnumerator waitOneSecond()
    {

        Debug.Log("Entro Corutina");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Salgo Coruitna");

        GoNextPhase();

    }



}
