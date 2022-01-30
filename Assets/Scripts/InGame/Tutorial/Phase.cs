using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour
{

    public int capaCollision;
    public int phaseNumber;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision: " + collision.gameObject.name);
        if (collision.gameObject.layer == capaCollision && Tutorial.instance.phase == phaseNumber)
        {

            GetComponent<BoxCollider>().enabled = false;

            if (phaseNumber == 6)
            {

                collision.gameObject.GetComponent<Rigidbody>().velocity = 2 * collision.gameObject.GetComponent<Rigidbody>().velocity;


            }

            Tutorial.instance.GoNextPhase();
        }

    }
}
