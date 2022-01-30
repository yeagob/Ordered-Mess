using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float speedRun = 100f;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Rigidbody hip;
    [SerializeField] private ParticleSystem dustParticle;
    [SerializeField] private SkinnedMeshRenderer baseRender;
    [SerializeField] private GameObject youTxt;

    [SerializeField] private Animator targetAnimator;

    float horizontal = 0;
    float vertical = 0;
    private bool walk = false;

    private PhotonView photonView;

    //Events
    internal static event Action walkEvent;
    internal static event Action walkStopEvent;

    //Hip for Cinemachine
    public Transform hipTransform;

    // Start is called before the first frame update
    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        InGameController.instance.OnStartGame += LoadPlayerProfile;
        InGameController.instance.OnStartGame += AsignPlayerColor;

        if (!photonView.IsMine)
            youTxt.SetActive(false);
    }

    
    private void Update()
    {
        //Authority
        if (NetworkManager.instance.multiplayerOn && !photonView.IsMine)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            //Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.speed = speedRun;

                if (!dustParticle.isPlaying)
                    dustParticle.Play();
            }
            else
            {
                this.speed = ProfileControl.playerProfile.playerSpeed;

                if (dustParticle.isPlaying)
                    dustParticle.Stop();
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Authority
        if (NetworkManager.instance.multiplayerOn && !photonView.IsMine)
            return;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            

            float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            this.hip.AddForce(direction * this.speed);

            this.walk = true;

            if (walkEvent != null)
                walkEvent();


        }  else {
            this.walk = false;

            if (walkStopEvent != null)
                walkStopEvent();

            if (dustParticle.isPlaying)
                dustParticle.Stop();
        }

        this.targetAnimator.SetBool("Walk", this.walk);


        if (Input.GetButton("Fire1"))
            this.targetAnimator.SetBool("Grab", true);

        if (Input.GetButtonUp("Fire1"))
            this.targetAnimator.SetBool("Grab", false);
    }

    private void LoadPlayerProfile()
    {
        speed = ProfileControl.playerProfile.playerSpeed;
        speedRun = ProfileControl.playerProfile.playerSpeedRun;
    }

    private void AsignPlayerColor()
    {
        if (PhotonNetwork.IsMasterClient)
            baseRender.material = InGameController.instance.pinkMat;
        else
            baseRender.material = InGameController.instance.blueMat;
    }
}
