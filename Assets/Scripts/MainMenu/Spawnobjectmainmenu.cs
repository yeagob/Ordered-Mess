using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnobjectmainmenu : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject cubeToSpawn;
    [SerializeField] GameObject sphereToSpawn;

    [SerializeField] bool readyToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        readyToSpawn = true;
        Time.timeScale = 0.4f;
        bucleCorrutina();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator CorrutineSpawnObject()
    {
        yield return new WaitForSeconds(0.5f);
        int random = Random.Range(0, 50);

        if (random < 25)
        {
            Invoke("SpawnCube", 0);
        }
        else if (random > 25)
        {
            Invoke("SpawnSphere", 0);
        }


        bucleCorrutina();
    }

    public void bucleCorrutina()
    {
        StartCoroutine("CorrutineSpawnObject");
    }

    public void SpawnCube()
    {
        if (readyToSpawn)
        {
            Instantiate(cubeToSpawn, transform.position, Quaternion.identity);
            Debug.Log(transform.position);
            

        }
    }
    public void SpawnSphere()
    {
        if (readyToSpawn)
        {
            Instantiate(sphereToSpawn, transform.position, Quaternion.identity);
            Debug.Log(transform.position);


        }
    }
}
