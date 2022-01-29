using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectmainmenuu : MonoBehaviour
{

    public GameObject punto;
    public GameObject player;
    public Rigidbody rd;
    public MeshRenderer rend;


    public bool CheckInv = false;
    // Start is called before the first frame update
    void Start()
    {
        punto = GameObject.Find("Point");
        Vector3 vdestino = (punto.transform.position - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = vdestino * 40;
        StartCoroutine("StartDelete");
        
        //rigidbody = player.GetComponentInChildren<Rigidbody>();
        //rigidbody.velocity = transform.forward * 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckInv)
        {
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, Mathf.Clamp(rend.material.color.a - 0.1f, 0, 1));
            
        }

        if(rend.material.color.a == 0)
        {
            Destroy(gameObject);
        }
        
    }

    IEnumerator StartDelete()
    {
        yield return new WaitForSeconds(2);
        
        CheckInv = true;
    }
}
