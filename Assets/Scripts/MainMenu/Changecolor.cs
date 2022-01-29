using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changecolor : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer rendPlayer;
    [SerializeField] Material pinkToon;
    [SerializeField] Material blueToon;

    // Azul : True      Rosa : False 
    public bool setMaterial;
    // Start is called before the first frame update
    void Start()
    {
        setMaterial = true;
        bucleCorrutina();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(2);
        if (setMaterial)
        {
            rendPlayer.material = pinkToon;
            setMaterial = false;
        }
        else if(!setMaterial)
        {
            rendPlayer.material = blueToon;
            setMaterial = true;
        }

        bucleCorrutina();
    }

    private void bucleCorrutina()
    {
        StartCoroutine("ChangeColor");
    }
}
