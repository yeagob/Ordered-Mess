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
    public GameObject _particlePrefab;

    [Header("Name")]
    [HideInInspector]public string _nameObject;
    [Header("Enum")]
    public List<HousePropType> _roomType;
    /*[HideInInspector]*/public HousePropType _propType;
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
    [SerializeField]private bool  _inRightPlace;
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
        //events
        Room.OnTriggerExitProp += CheckPropsRoom;
        OnPointsRefresh += SpawnParticles;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        print(Input.GetAxis("Mouse ScrollWheel"));
        if (_objetctPicked)
        {
            //Nos aseguramos que siempre esté en el centro de la mano el objeto
            transform.localPosition = Vector3.zero;

            //Si el objeto está cogido y le das a la rueda del ratón, lo giras
            if (Input.GetAxis("Mouse ScrollWheel") > 0.1)
                transform.Rotate(45, 0, 0);
            if (Input.GetAxis("Mouse ScrollWheel") < -0.1)
                transform.Rotate(-45, 0, 0);

            if (_inRightPlace)
                uiController.SetGoodColor(true);
        }

        //Update points on object stop
        if (releasedObjectPoints && rb.velocity.magnitude < 0.1f)
        {
            releasedObjectPoints = false;
            
            //Actualizamos los puntos
            UpdatePoints();
        }
        //if (rb.velocity.magnitude > 0.1f)
        //    releasedObjectPoints = true;
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
        uiController.objectNameText.text = _nameObject;
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);
    }

    internal void Release()
    {
        _objetctPicked = false;
        uiController.objectNameText.text = "";
        uiController.SetGoodColor(false);
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (networkManager.multiplayerOn)
            photonView.RPC(nameof(RPCPickedTrue), RpcTarget.Others);

        releasedObjectPoints = true;
    }

    private void UpdatePoints()
    {
        if (roundControl.round1)
        {

            if (_inRightPlace && _propType != HousePropType.none)
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
        }
        //ROUND 2
        else
        {
            if (_inRightPlace && _propType != HousePropType.none)
            {
                _amountPoints = 0;
            }
            else
            {
                if (Vector3.Angle(this.transform.forward, Vector3.up) > 135)
                    _amountPoints = _maxAmountPoints;

                if (Vector3.Angle(this.transform.forward, Vector3.up) > 45 && Vector3.Angle(this.transform.forward, Vector3.up) < 135)
                    _amountPoints = _maxAmountPoints / 4;

                if (Vector3.Angle(this.transform.forward, Vector3.up) < 45)
                    _amountPoints = _maxAmountPoints / 8;
            }
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

        instantiatedPoints.transform.position = transform.position + Vector3.up * 5 - Vector3.forward;
        instantiatedPoints.transform.forward = Camera.main.transform.forward;
        instantiatedPoints.GetComponentInChildren<Text>().text = _amountPoints.ToString();
        instantiatedPoints.GetComponent<Animator>().Play("CanvasPanelPointAnim");
    }

    void CheckPropsRoom()
    {
        _inRightPlace = false;
        uiController.SetGoodColor(false);
        if (_roomType.Contains(_propType))
        {
           _inRightPlace = true;
        }
    }

    private void SpawnParticles(int point)
    {
        if (point > 0)
        {
            instantiatedParticle = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
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
