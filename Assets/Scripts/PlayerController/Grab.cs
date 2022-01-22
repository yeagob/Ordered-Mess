using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform targetPickup;
    public bool objectPicked = false;

    private GameObject pickupGO;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (objectPicked)
            {
                print("Object throwed!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO = null;

                objectPicked = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pickable")
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                print("Object grabbed!");

                pickupGO = other.gameObject;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = targetPickup.position;
                other.transform.parent = targetPickup;

                objectPicked = true;
            }
        }
    }
}
