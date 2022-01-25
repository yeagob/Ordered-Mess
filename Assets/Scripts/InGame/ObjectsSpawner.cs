using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] HouseProps[] objectList;
    [SerializeField] Vector3 size;
    [SerializeField] float objectOffser = 0.1f;
    
    // Start is called before the first frame update
    void Awake()
    {
        InGameController.instance.OnStartGame += SpawnObjects;
    }

    private void SpawnObjects()
    {
        Vector3 initPos = transform.position - ((size.x * Vector3.right) / 2) - ((size.z * Vector3.forward) / 2);
        Vector3 currentPos = initPos;
        float biggerX = 0;
        for (int x = 0; currentPos.x < initPos.x + size.x; x++)
        {
            for (int z = 0; currentPos.z < initPos.z + size.z + size.x; z++)
            {
                int rnd = Random.Range(0, objectList.Length);
                currentPos += (objectList[rnd]._baseSize.y + objectOffser) * Vector3.forward;
                biggerX = objectList[rnd]._baseSize.x > biggerX ? objectList[rnd]._baseSize.y : biggerX;
                HouseProps prop = Instantiate(objectList[rnd], currentPos + (Vector3.right * (biggerX / 2)), objectList[rnd].transform.rotation);
                prop.transform.parent = this.transform;
            }
            currentPos = new Vector3(currentPos.x + biggerX + objectOffser, transform.position.y, initPos.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, size);
    }
}
