using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class HouseProps : MyMonoBehaviour
{

    #region Attributes
    
    [Header("Puntos")]
    public GameObject _pointUiPrefab;
    public GameObject _particlePrefab;

    [Header("Name")]
    [HideInInspector]public string _nameObject;
    [Header("Enum")]
    public List<RoomType> _roomType;
    /*[HideInInspector]*/public RoomType _currentRoom;
    [Header("Points")]
    public int _maxAmountPoints = 1000;
    internal int _amountPoints = 0;
    [Header("Size")]
    public Vector2 _baseSize;

    //Instantiated Points
    GameObject instantiatedPoints = null;
    bool releasedObjectPoints = false;    
    //Instantiated Particle
    GameObject instantiatedParticle = null;


    Rigidbody rb;

    [Header("Bools")]
    public  bool  _objetctPicked;
    bool placed = false;
    [SerializeField]private bool  _inRightPlace;
    public bool onlyRound2;
    [HideInInspector] public bool _realiseObject;
    
      
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
        
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if (_objetctPicked)
        {
            //Nos aseguramos que siempre esté en el centro de la mano el objeto
            transform.localPosition = Vector3.zero;

            //Si el objeto está cogido y le das a la rueda del ratón, lo giras
            if (Input.GetAxis("Mouse ScrollWheel") > 0.1)
                transform.Rotate(Random.Range(0, 45), 0, Random.Range(0, 45));
            if (Input.GetAxis("Mouse ScrollWheel") < -0.1)
                transform.Rotate(Random.Range(-45,0), 0, Random.Range(-45,0));
        }

        //Update points on object stop
        if (releasedObjectPoints && rb.IsSleeping() && !rb.isKinematic)
        {
            releasedObjectPoints = false;
            
            //Actualizamos los puntos
            UpdatePoints();

            placed = true;
        }
        if (placed && rb.velocity.magnitude > 0.1f)
            releasedObjectPoints = true;
    }
    private void OnDestroy()
    {
    }
    #endregion

    #region Methods

    internal bool Grab(Transform targetPickup)
    {
        if (onlyRound2 && roundControl.round1)
            return false;

        rb.isKinematic = true;
        transform.position = targetPickup.position;
        transform.parent = targetPickup;

        _objetctPicked = true;
        gameObject.layer = LayerMask.NameToLayer("Grabbed");
        uiController.objectNameText.text = _nameObject;

        //CheckPropsRoom(_currentRoom);

        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);

        return true;
    }

    internal void Release(float throwForce)
    {
        _objetctPicked = false;

        //Phisics
        rb.isKinematic = false;
        transform.parent = null;
        if (throwForce > 0)
            rb.AddForce(-InGameController.instance.hip.transform.forward * throwForce, ForceMode.Impulse);
        gameObject.layer = LayerMask.NameToLayer("Furniture");

        //UI Points
        uiController.objectNameText.text = "";
        uiController.SetGoodColor(false);
        releasedObjectPoints = true;
        
        //Network
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
    }

    private void UpdatePoints()
    {
        float angle = Vector3.Angle(this.transform.forward, Vector3.up);
        if (roundControl.round1)
        {
            if (_inRightPlace)
            {
                if (angle < 45)
                    _amountPoints = _maxAmountPoints;

                if (angle > 45 && angle < 135)
                    _amountPoints = _maxAmountPoints / 4;

                if (angle > 135)
                    _amountPoints = _maxAmountPoints / 8;
            }
            else
            {
                if (angle < 45)
                    _amountPoints = _maxAmountPoints/10;
                else
                    _amountPoints = 0;
            }
        }
        //ROUND 2
        else
        {
            if (_inRightPlace && _currentRoom != RoomType.none)
            {
                if (angle > 135)
                        _amountPoints = _maxAmountPoints / 10;
                else
                    _amountPoints = 0;
            }
            else
            {
                if (angle > 135)
                    _amountPoints = _maxAmountPoints;

                if (angle > 45 && angle < 135)
                    _amountPoints = _maxAmountPoints / 4;

                if (angle < 45)
                    _amountPoints = _maxAmountPoints / 8;
            }
        }

        //EVENTs
        if (OnPointsRefresh != null)
            OnPointsRefresh(_amountPoints);

        if (OnAnyPointsRefresh != null)
            OnAnyPointsRefresh();
        //PArticles & points
        SpawnParticles(_amountPoints);
        SpawnUiPoints();

        print("objeto " + _nameObject + " con angulo "+ angle  + " en la room " + _currentRoom + " da " + _amountPoints + " del maximo " + _maxAmountPoints + " que podría dar");

        
    }

    internal void SpawnUiPoints()
    {
        if (instantiatedPoints == null)
            instantiatedPoints = Instantiate(_pointUiPrefab, transform.position, Quaternion.identity);

        instantiatedPoints.transform.position = transform.position + Vector3.up * 1 - Vector3.forward * 1;
        instantiatedPoints.transform.forward = Camera.main.transform.forward;
        instantiatedPoints.GetComponentInChildren<Text>().text = _amountPoints.ToString();
        instantiatedPoints.GetComponentInChildren<Text>().color = new Color(_amountPoints/ _maxAmountPoints, _amountPoints / _maxAmountPoints,0);
        instantiatedPoints.GetComponent<Animator>().Play("CanvasPanelPointAnim");
    }

    internal void CheckPropsRoom( RoomType room)
    {
        _currentRoom = room;
            
        if (_roomType.Contains(_currentRoom))
        {
           _inRightPlace = true;
            if (_objetctPicked)
                uiController.SetGoodColor(true);
        }
        else
        {
           _inRightPlace = false;
            if (_objetctPicked)
            uiController.SetGoodColor(false);
        }
    }

    private void SpawnParticles(int point)
    {
        if (_inRightPlace)
        {
            instantiatedParticle = Instantiate(_particlePrefab, transform.position, Quaternion.identity);

            //Desactive máx poitns particles.
            if (point != _maxAmountPoints)
                instantiatedParticle.transform.GetChild(1).gameObject.SetActive(false);

            ColorParticle(point);
        }
    }
    
    private void ColorParticle(int point)
    {
        if (point < _maxAmountPoints / 8)
        {
            ParticleSystem. MainModule settings = instantiatedParticle.GetComponentInChildren<ParticleSystem>().main;
            settings. startColor = new ParticleSystem. MinMaxGradient(Color.red);
        }
        if (point > _maxAmountPoints / 8 && point < _maxAmountPoints / 4)
        {
            ParticleSystem. MainModule settings = instantiatedParticle.GetComponentInChildren<ParticleSystem>().main;
            settings. startColor = new ParticleSystem. MinMaxGradient(Color.yellow);
        }
        if (point > _maxAmountPoints / 4)
        {
            ParticleSystem. MainModule settings = instantiatedParticle.GetComponentInChildren<ParticleSystem>().main;
            settings. startColor = new ParticleSystem. MinMaxGradient(Color.green);
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
