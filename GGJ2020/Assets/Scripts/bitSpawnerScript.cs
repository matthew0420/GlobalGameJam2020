using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bitSpawnerScript : MonoBehaviour
{
    public PlayerControlScript playerScript;
    public int spawnTimer = 0;
    public int timeToSpawn;
    public int randomSpawnPoint;
    public GameObject bitObject;
    private GameObject bitToSpawn;
    public GameObject spawnerObject;
    public GameObject playerObject;
    public Vector3 spawnPoint;
    public int playerSafeSpawnDistance;
    public Transform bitSize;
    void Start()
    {
        spawnerObject = this.gameObject;
        timeToSpawn = Random.Range(40, 60);
        playerSafeSpawnDistance = 6;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer >= timeToSpawn)

        {
            BitSpawn();
            spawnTimer = 0;
            timeToSpawn = Random.Range(150, 250);
        }
        else
        {
            if (spawnTimer <= timeToSpawn)
            {
                spawnTimer++;
            }
        }
    }

    public void BitSpawn()
    {
        spawnPoint = playerObject.transform.position;
        spawnPoint = playerObject.transform.position + new Vector3(Random.Range(-20f, 20f),
                                                                    Random.Range(-20f, 20f), 0);
        if (Vector3.Distance(spawnPoint, playerObject.transform.position) > 12f)
        {
            bitToSpawn = GameObject.Instantiate(
                bitObject, spawnPoint/*+= new Vector3(Random.Range(-10f, 10f),
                                               Random.Range(-10f, 10f), 0)*/, Quaternion.identity);
            bitSize = playerScript.bulletTransform;
            this.bitToSpawn.transform.localScale = playerScript.playerTransform.localScale * 8;
        }
        else
        {
            BitSpawn();
        }
    }
}
