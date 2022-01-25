using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform targetPickup;
    public bool objectPicked = false;

    private GameObject pickupGO;
    private float throwForce = 10;

    //Events
    internal static event Action<HouseProps> OnGrabEvent;
    internal static event Action<HouseProps> OnThrowEvent;
    internal static event Action<HouseProps> OnReleaseEvent;

    private void Start()
    {
        InGameController.instance.OnStartGame += LoadPlayerProfile;
    }


    private void Update()
    {
        //Object released!
        if (Input.GetButtonUp("Fire1"))
        {
            if (objectPicked)
            {
                //print("Object released!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO = null;

                objectPicked = false;
                pickupGO.GetComponent<HouseProps>()._objetctPicked = true;

                if (OnReleaseEvent != null)
                    OnReleaseEvent(pickupGO.GetComponent<HouseProps>());
            }
        }

        //Object throwed!
        if (Input.GetButtonDown("Fire2"))
        {
            if (objectPicked)
            {
                //print("Object throwed!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO.GetComponent<Rigidbody>().AddForce(throwForce, throwForce, throwForce, ForceMode.Impulse);
                pickupGO = null;

                //Delay to not pick up again the object while pressing Fire1 and throwing
                Invoke(nameof(DelayObjectPicked), 0.2f);

                //
                pickupGO.GetComponent<HouseProps>()._objetctPicked = false;

                if (OnThrowEvent != null)
                    OnThrowEvent(pickupGO.GetComponent<HouseProps>());
            }
        }
    }

    //TODO: refactor.
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pickable")
        {
            //Object grabbed!
            if (Input.GetButton("Fire1") && !objectPicked && !other.gameObject.GetComponent<HouseProps>()._objetctPicked)
            {
              //  print("Object grabbed!");

                pickupGO = other.gameObject;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = targetPickup.position;
                other.transform.parent = targetPickup;

                objectPicked = true;

                pickupGO.GetComponent<HouseProps>()._objetctPicked = true;

                if (OnGrabEvent != null)
                    OnGrabEvent(other.GetComponent<HouseProps>());
            }
        }
    }

    private void DelayObjectPicked()
    {
        objectPicked = false;
    }

    private void LoadPlayerProfile()
    {
        throwForce = ProfileControl.playerProfile.throwObjectsForce;
    }

}
