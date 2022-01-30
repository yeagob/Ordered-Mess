using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MyMonoBehaviour
{
    [SerializeField] HouseProps[] objectList;
    [SerializeField] Vector3 size;
    [SerializeField] float objectOffseT = 0.1f;
    
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
                currentPos += (objectList[rnd]._baseSize.y + objectOffseT) * Vector3.forward;
                biggerX = objectList[rnd]._baseSize.x > biggerX ? objectList[rnd]._baseSize.y : biggerX;
                GameObject prop = null;
                if (networkManager.multiplayerOn)
                {
                    if(PhotonNetwork.IsMasterClient)
                        prop = PhotonNetwork.Instantiate(objectList[rnd].name, currentPos + (Vector3.right * (biggerX / 2)), objectList[rnd].transform.rotation);
                }
                else
                    prop = Instantiate(objectList[rnd].gameObject, currentPos + (Vector3.right * (biggerX / 2)), objectList[rnd].transform.rotation);

                if (prop != null)
                    prop.transform.parent = this.transform;

                //TODO: RPC al otro jugador de la cantidad de muebles.

            }

            currentPos = new Vector3(currentPos.x + biggerX + objectOffseT, transform.position.y, initPos.z);
        }
    }
    public void SpawnTutorialObject()
    {
        GameObject prop = null;

        prop = Instantiate(objectList[0].gameObject, new Vector3(0, 0.5f, 0), objectList[0].transform.rotation);

        prop = null;

        prop = Instantiate(objectList[1].gameObject, new Vector3(-9.758f, 0.1f, 4.875f), objectList[1].transform.rotation);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, size);
    }
}
