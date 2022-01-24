using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    #region Attributes
    [Header("Enum")]
    public HousePropType _roomType;
    #endregion
          
    #region UnityCalls
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {

        }
    }
    #endregion

    #region Methods
    #endregion
}
