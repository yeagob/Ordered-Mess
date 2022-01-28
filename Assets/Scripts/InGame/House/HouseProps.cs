using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HouseProps : MyMonoBehaviour
{

    #region Attributes
    [Header("Puntos")]
    public GameObject _pointUiPrefab;

    [Header("Name")]
    [HideInInspector]public string _nameObject;
    [Header("Enum")]
    public List<HousePropType> _roomType;
    [HideInInspector]public HousePropType _propType;
    [HideInInspector]private HousePropType _saveRomeType;
    [Header("Points")]
    public int _maxAmountPoints = 1000;
    internal int _amountPoints = 0;
    [Header("Size")]
    public Vector2 _baseSize;

    //Instantiated Points
    GameObject instantiatedPoints = null;

    [Header("Bools")]
    public  bool  _objetctPicked;
    public  bool  _floorObject;
    public  bool  _wallObject;
    private bool  _endCorrutineCheckPropRoom;
    [HideInInspector] public bool _realiseObject;
    
    //int 
    internal int _countRomeType = 0;
      
    // Singlentons
    [System.NonSerialized]
    public PhotonView photonView;

    //Event
    public event Action<int> OnPointsRefresh;//<-Particle System
    public static event Action OnAnyPointsRefresh;//<-Sount
    #endregion

    #region UnityCalls
    void Start()
    {   
        _amountPoints = 0;
        photonView = GetComponent<PhotonView>();
        //events
        Room.OnTriggerExitProp += CheckPropsRoom;
    }

    void Update()
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
        gameObject.layer = LayerMask.NameToLayer("Grabbed");
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
    }
    internal void Release()
    {
        _objetctPicked = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);

        //Actualizamos los puntos
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        if (_endCorrutineCheckPropRoom && _saveRomeType == _propType && _saveRomeType != HousePropType.none)
        {

            if (Vector3.Angle(this.transform.forward, Vector3.up) < 45)
                _amountPoints = _maxAmountPoints;
            
            if (Vector3.Angle(this.transform.forward, Vector3.up) > 45 && Vector3.Angle(this.transform.forward, Vector3.up) < 135)
                _amountPoints = _maxAmountPoints / 4;
            
            if (Vector3.Angle(this.transform.forward, Vector3.up) > 135)
                _amountPoints = _maxAmountPoints / 8;
        }
        else
        {
            _amountPoints = 0;
        }

        //EVENTs
        if (OnPointsRefresh != null)
            OnPointsRefresh(_amountPoints);
        if (OnAnyPointsRefresh != null)
            OnAnyPointsRefresh();


        SpawnUiPoints();
    }

    internal void SpawnUiPoints()
    {
        if (instantiatedPoints == null)
            instantiatedPoints = Instantiate(_pointUiPrefab, transform.position, Quaternion.identity);

        instantiatedPoints.transform.position = transform.position + Vector3.up;
        instantiatedPoints.transform.forward = Camera.main.transform.forward;
        instantiatedPoints.GetComponentInChildren<Text>().text = _amountPoints.ToString();
        instantiatedPoints.GetComponent<Animator>().Play("CanvasPanelPointAnim");
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
