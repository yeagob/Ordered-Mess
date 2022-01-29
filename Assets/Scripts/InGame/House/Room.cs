using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MyMonoBehaviour
{
    
    #region Attributes
    [Header("Enum")]
    public HousePropType _roomType;
    public List<GameObject> roomHouseProps = new List<GameObject>();
    

    //event
    public static event Action OnTriggerEnterProp;
    public static event Action OnTriggerExitProp;
    #endregion

    #region UnityCalls
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && other.GetComponent<HouseProps>() != null && !other.GetComponent<HouseProps>()._realiseObject)
        {
            roomHouseProps.Remove(other.gameObject);
            other.GetComponent<HouseProps>()._realiseObject = true;
            if (_roomType == HousePropType.Center)
            {
                int auxObjetosActuales = uiController.objetosTotales;
                auxObjetosActuales++;
                uiController.SetObjsTotalesText(auxObjetosActuales);
            }

            if (OnTriggerEnterProp != null)
            {
               OnTriggerEnterProp();
            }
        }

        if (other.CompareTag("Player"))
        {
           // print(_roomType);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && other.GetComponent<HouseProps>() != null && !other.GetComponent<HouseProps>()._objetctPicked)
        {
            roomHouseProps.Add(other.gameObject);
            other.GetComponent<HouseProps>()._realiseObject = false;
            other.GetComponent<HouseProps>()._propType = _roomType;

            if (_roomType == HousePropType.Center)
            {
                int auxObjetosActuales = uiController.objetosTotales;
                auxObjetosActuales--;
                uiController.SetObjsTotalesText(auxObjetosActuales);
            }

            
            if (OnTriggerExitProp != null)
            {
              OnTriggerExitProp();
            }
        }
    }
    #endregion

    #region Methods
   
    #endregion
    }
