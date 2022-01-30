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

                tutorialLabel.text = "Use W A S D and reach the north wall";

                break;
            case 2:

                tutorialLabel.text = "Try running to the other side of the yard using Shift";

                break;
            case 3:

                tutorialLabel.text = "Grab the object holidng left click and take it to the Main Room";
                puerta.SetActive(false);
                _ObjectSpawner.SpawnTutorialObject();

                break;
            case 4:

                tutorialLabel.text = "You can rotate the chair with the mouse wheel and leave it realeasing the Left Click";

                break;
            case 5:

                StartCoroutine("waitPhase5");
                tutorialLabel.text = "When the object stop moving the points will appear on the top of it";

                break;

            case 6:

                StartCoroutine("waitPhase6");
                tutorialLabel.text = "The ammount depends on how the object is placed. Now it's time to make some chaos";

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

    IEnumerator waitPhase5()
    {

        Debug.Log("Entro Corutina");
        yield return new WaitForSeconds(4);

        Debug.Log("Salgo Coruitna");

        GoNextPhase();

    }

    IEnumerator waitPhase6()
    {

        Debug.Log("Entro Corutina");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Salgo Coruitna");

        GoNextPhase();

    }

}
