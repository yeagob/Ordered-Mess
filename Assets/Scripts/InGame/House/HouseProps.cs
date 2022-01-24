using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

    public class HouseProps : MonoBehaviour
    {
        
          #region Attributes
          [Header("Name")]
          public string _nameObject;
          [Header("Enum")]
          public List<HousePropType> _roomType;
          [Header("Point")]
          public int _amountPoints;
          [Header("Size")]
          public Vector2 _baseSize;
          [Header("Bools")]
          public bool _floorObject;
          public bool _wallObject;
          internal bool _objetctPicked;
          private bool _objetctPickedCheck;
          private bool _endCorrutine;
          // Singlentons
          [System.NonSerialized]
          public PhotonView photonView;
          #endregion
          
          #region UnityCalls
          void Start()
          {
              StartCoroutine(CorrutineUpdate());
          }
          private void OnDestroy()
          {
              _endCorrutine = true;
          }
          #endregion

          #region Methods
          IEnumerator CorrutineUpdate()
          {
              while (!_endCorrutine)
              {   
                  if(_objetctPicked != _objetctPickedCheck && !_objetctPickedCheck)
                  {
                      photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
                      _objetctPickedCheck = true;
                  }
                  if (_objetctPicked != _objetctPickedCheck && _objetctPickedCheck)
                  {
                      photonView.RPC(nameof(RPCPickedFalse), RpcTarget.Others);
                      _objetctPickedCheck = false;
                  }
                  yield return null;
              }
          } 
          #endregion

          #region RPCs
          [PunRPC]
          void RPCPickedTrue()
          {
              _objetctPicked = true;
          }          
          [PunRPC]
          void RPCPickedFalse()
          {
              _objetctPicked = false;
          }
          #endregion
    }
