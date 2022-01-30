using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform targetPickup;
    public bool grabbingObject = false;

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
            if (grabbingObject)
            {
                //print("Object released!");

                pickupGO.GetComponent<HouseProps>().Release(0);
                pickupGO.GetComponent<Rigidbody>().isKinematic = false;
                pickupGO.transform.parent = null;

                grabbingObject = false;

                if (OnReleaseEvent != null)
                    OnReleaseEvent(pickupGO.GetComponent<HouseProps>());

                pickupGO = null;
            }
        }

        //Object throwed!
        if (Input.GetButtonDown("Fire2"))
        {
            if (grabbingObject)
            {
                //print("Object throwed!");
                pickupGO.GetComponent<HouseProps>().Release(throwForce);

                //Delay to not pick up again the object while pressing Fire1 and throwing
                Invoke(nameof(DelayObjectPicked), 0.2f);

                if (OnThrowEvent != null)
                    OnThrowEvent(pickupGO.GetComponent<HouseProps>());

                pickupGO = null;
            }
        }
    }

    internal void CheckObject(RoomType room)
    {
        if (pickupGO != null)
            pickupGO.GetComponent<HouseProps>().CheckPropsRoom(room);
    }

    //TODO: refactor. To Update
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pickable")
        {

            //Object grabbed!
            if (Input.GetButton("Fire1") && !grabbingObject && !other.gameObject.GetComponent<HouseProps>()._objetctPicked)
            {
              //  print("Object grabbed!");

                pickupGO = other.gameObject;

                if (pickupGO.GetComponent<HouseProps>().Grab(targetPickup))
                {
                    grabbingObject = true;

                    if (OnGrabEvent != null)
                        OnGrabEvent(other.GetComponent<HouseProps>());
                }
            }
        }
    }

    private void DelayObjectPicked()
    {
        grabbingObject = false;
    }

    private void LoadPlayerProfile()
    {
        throwForce = ProfileControl.playerProfile.throwObjectsForce;
    }

}
