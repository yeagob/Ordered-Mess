using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FurnitureProps : MyMonoBehaviour
{
    
    #region Attributes
    [Header("Bools")]
    public  bool  _objetctPicked;
    //Main tools
    RoundController roundController;

    // Singlentons
    [System.NonSerialized]
    public PhotonView photonView;
    #endregion

    #region UnityCalls
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        //Events 
        crono.OnCronoCompleted += ChangeTag;
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
    }
    void ChangeTag()
    {
          if (!gameController.roundController.round1)
          {
            this.gameObject.tag = "Pickable";
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
