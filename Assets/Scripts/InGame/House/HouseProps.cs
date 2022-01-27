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
    public HousePropType _propType;
    public HousePropType _saveRomeType;
    [Header("Point")]
    public /*internal*/ int _amountPoints = 0;
    [Header("Size")]
    public Vector2 _baseSize;

    [Header("Bools")]
    public  bool _objetctPicked;
    public bool _floorObject;
    public bool _wallObject;
    [HideInInspector] public bool _realiseObject;

  //int 
  internal int _countRomeType = 0;
    // Singlentons
    [System.NonSerialized]
    public PhotonView photonView;
    #endregion
          
    #region UnityCalls
    void Start()
    {   
        _amountPoints = 0;
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        PointsProp();
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

    private void PointsProp()
    {

        if (_countRomeType < _roomType.Count && _roomType[_countRomeType] != _propType )
        {
          _countRomeType++;
        }
        else
        {
            if (_propType != HousePropType.none && _countRomeType != _roomType.Count)
            {
                _saveRomeType = _roomType[_countRomeType];
            }
            if (_propType != HousePropType.none && _saveRomeType == _propType)
            {
                _amountPoints = 100;
            }
            else
            {
                _amountPoints = 0;
            }
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
