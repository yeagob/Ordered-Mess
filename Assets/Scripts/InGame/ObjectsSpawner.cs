using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] HouseProps[] objectList;
    [SerializeField] Vector3 sizeXY;
    
    // Start is called before the first frame update
    void Awake()
    {
        InGameController.instance.OnStartGame += SpawnObjects;
    }

    private void SpawnObjects()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                int rnd = Random.Range(0, objectList.Length);
                Instantiate(objectList[rnd], transform.position + (x * Vector3.right) + (y * Vector3.forward), objectList[rnd].transform.rotation);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, sizeXY);
    }
}
