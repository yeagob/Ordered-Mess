using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
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
        if (other.CompareTag("Pickable"))
        {
            roomHouseProps.Add(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            print(_roomType);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            roomHouseProps.Remove(other.gameObject);
        }
    }
        #endregion

        #region Methods
        #endregion
    }
