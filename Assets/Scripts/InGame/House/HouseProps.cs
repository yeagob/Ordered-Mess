using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseProps : MyMonoBehaviour
{
        
    #region Attributes
    [Header("Name")]
    [HideInInspector]public string _nameObject;
    [Header("Enum")]
    public List<HousePropType> _roomType;
    [HideInInspector]public HousePropType _propType;
    [HideInInspector]private HousePropType _saveRomeType;
    [Header("Point")]
    public int _amountPoints = 0;
    [Header("Size")]
    public Vector2 _baseSize;

    [Header("Bools")]
    public  bool  _objetctPicked;
    public  bool  _floorObject;
    public  bool  _wallObject;
    private bool  _endCorrutineCheckPropRoom;
    [HideInInspector] public bool _realiseObject;
    
    //int 
    internal int _countRomeType = 0;
      
    //camare
    [SerializeField]private GameObject _cameraMain;
    // Singlentons
    [System.NonSerialized]
    public PhotonView photonView;
    #endregion
          
    #region UnityCalls
    void Start()
    {   
        _amountPoints = 0;
        _cameraMain = FindObjectOfType<Camera>().gameObject;
        photonView = GetComponent<PhotonView>();
        //events
        Room.OnTriggerExitProp += CheckPropsRoom;
    }

    void Update()
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
        if (_endCorrutineCheckPropRoom && _saveRomeType == _propType && _saveRomeType != HousePropType.none)
        {

            if (this.transform.rotation.x == _cameraMain.transform.rotation.x)
            {
                _amountPoints = 100;
            }            
            if (this.transform.rotation.x  < _cameraMain.transform.rotation.x)
            {
                _amountPoints = 100 / 2;
            }            
            if ( this.transform.rotation.x > _cameraMain.transform.rotation.x)
            {
                _amountPoints = 100 / 4;
            }
        }
        else
        {
            _amountPoints = 0;
        }
    }   
     void CheckPropsRoom()
    {
        _endCorrutineCheckPropRoom = false;
        _countRomeType = 0;
        StartCoroutine(CorrutineCheckPropRoom());
        
    }

    
    IEnumerator CorrutineCheckPropRoom()
    {
        while (!_endCorrutineCheckPropRoom)
        {   
            
            if (_countRomeType < _roomType.Count && _roomType[_countRomeType] != _propType )
            {
              _countRomeType++;
            }
            else
            {
            if (_propType != HousePropType.none)
                {
                    _saveRomeType = _roomType[_countRomeType];
                    if (_saveRomeType == _propType)
                    {
                        _endCorrutineCheckPropRoom = true;
                    }
                    else
                    {
                        _amountPoints = 0;
                        _endCorrutineCheckPropRoom = true;
                    }
                  
                }
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
