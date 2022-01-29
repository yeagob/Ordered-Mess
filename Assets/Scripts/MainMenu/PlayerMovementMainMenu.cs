using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovementMainMenu : MonoBehaviour
{

    //[SerializeField] private ConfigurableJoint hipJoint;
    //[SerializeField] private Rigidbody hip;

    [SerializeField] private Animator targetAnimator;
    // Start is called before the first frame update
    void Start()
    {
        this.targetAnimator.SetBool("Walk", true);
    }

    // Update is called once per frame
    void Update()
    {
        //this.hip.AddForce(Vector3.forward * 50);
        //transform.RotateAroundLocal(new Vector3(0,transform.position.y,0), 5f * Time.deltaTime);

    }
}
