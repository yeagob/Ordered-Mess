using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class HouseProps : MyMonoBehaviour
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
    public  bool _objetctPicked;
    public bool _floorObject;
    public bool _wallObject;

    // Singlentons
    [System.NonSerialized]
    public PhotonView photonView;
    #endregion
          
    #region UnityCalls
    void Start()
    {
    }
    private void OnDestroy()
    {
    }
    #endregion

    #region Methods

    internal void Grab()
    {
        _objetctPicked = true;
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
    }
    internal void Release()
    {
        _objetctPicked = false;
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
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
