using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform targetPickup;
    public bool objectPicked = false;

    private GameObject pickupGO;

    //Events
    internal static event Action grabEvent;
    internal static event Action throwEvent;

    private void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            if (objectPicked)
            {
                print("Object released!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO = null;

                objectPicked = false;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (objectPicked)
            {
                print("Object throwed!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO.GetComponent<Rigidbody>().AddForce(10, 10, 10, ForceMode.Impulse);
                pickupGO = null;

                //Delay to not pick up again the object while pressing Fire1 and throwing
                Invoke(nameof(DelayObjectPicked), 0.2f);

                if(throwEvent != null)
                throwEvent();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pickable")
        {
            if (Input.GetButton("Fire1") && !objectPicked)
            {
                print("Object grabbed!");

                pickupGO = other.gameObject;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = targetPickup.position;
                other.transform.parent = targetPickup;

                objectPicked = true;

                if (grabEvent != null)
                    grabEvent();
            }
        }
    }

    private void DelayObjectPicked()
    {
        objectPicked = false;
    }
}
