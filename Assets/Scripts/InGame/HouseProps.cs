using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class HouseProps : MonoBehaviour
    {
        
          #region Attributes
          [Header("Name")]
          public string _nameObject;
          [Header("Enum")]
          public HousePropType _housePropEnum;
          [Header("Point")]
          public int _amountPoints;
          [Header("Size")]
          public Vector2 _vectSize;
          [Header("Bools")]
          public bool _toPickUp;
          public bool _floorObject;
          public bool _wallObject;
          #endregion
          
          #region UnityCalls
          void Start()
          {

          }
          #endregion

          #region Methods
          #endregion
    }

}
