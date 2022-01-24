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
        if (Input.GetButtonUp("Fire1"))
        {
            if (objectPicked)
            {
                print("Object released!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO = null;

                objectPicked = false;

                if (OnReleaseEvent != null)
                    OnReleaseEvent(pickupGO.GetComponent<HouseProps>());
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (objectPicked)
            {
                print("Object throwed!");

                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;
                pickupGO.GetComponent<Rigidbody>().AddForce(throwForce, throwForce, throwForce, ForceMode.Impulse);
                pickupGO = null;

                //Delay to not pick up again the object while pressing Fire1 and throwing
                Invoke(nameof(DelayObjectPicked), 0.2f);

                if(OnThrowEvent != null)
                    OnThrowEvent(pickupGO.GetComponent<HouseProps>());
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
        throwForce = PlayerProfile.instance.throwObjectsForce;
    }

}
