using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MyMonoBehaviour
{
    
    #region Attributes
    [Header("Enum")]
    public HousePropType _roomType;
    public List<GameObject> roomHouseProps = new List<GameObject>();
    #endregion

    #region UnityCalls
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && !other.GetComponent<HouseProps>()._realiseObject)
        {
            roomHouseProps.Remove(other.gameObject);
            other.GetComponent<HouseProps>()._realiseObject = true;
            if (_roomType == HousePropType.Center)
            {
                int auxObjetosActuales = uiController.objetosTotales;
                auxObjetosActuales++;
                uiController.SetObjsTotalesText(auxObjetosActuales);
            }
        }

        if (other.CompareTag("Player"))
        {
           // print(_roomType);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && !other.GetComponent<HouseProps>()._objetctPicked)
        {
            roomHouseProps.Add(other.gameObject);
            other.GetComponent<HouseProps>()._realiseObject = false;

            if (_roomType == HousePropType.Center)
            {
                int auxObjetosActuales = uiController.objetosTotales;
                auxObjetosActuales--;
                uiController.SetObjsTotalesText(auxObjetosActuales);
            }
        }
    }
    #endregion

    #region Methods
        #endregion
    }
