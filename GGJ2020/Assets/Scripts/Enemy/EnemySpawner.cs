using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public PlayerControlScript playerScript;
    public int spawnTimer = 0;
    public int timeToSpawn;
    public int randomSpawnPoint;
    public GameObject enemyObject;
    private GameObject enemyToSpawn;
    public GameObject spawnerObject;
    public GameObject playerObject;
    public Vector3 spawnPoint;
    public int playerSafeSpawnDistance;
    public Transform enemySize;
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
            EnemySpawn();
            spawnTimer = 0;
            timeToSpawn = Random.Range(140, 250);
        }
        else
        {
            if (spawnTimer <= timeToSpawn)
            {
                spawnTimer++;
            }
        }
    }

    public void EnemySpawn()
    {
        spawnPoint = playerObject.transform.position;
        spawnPoint = playerObject.transform.position + new Vector3(Random.Range(-20f, 20f),
                                                                    Random.Range(-20f, 20f), 0);
        if (Vector3.Distance(spawnPoint, playerObject.transform.position) > 12f)
        {
            enemyToSpawn = GameObject.Instantiate(
                enemyObject, spawnPoint/*+= new Vector3(Random.Range(-10f, 10f),
                                               Random.Range(-10f, 10f), 0)*/, Quaternion.identity);
            enemySize = this.enemyToSpawn.transform;
            this.enemyToSpawn.transform.localScale = playerScript.playerTransform.localScale;
        } else
        {
            EnemySpawn();
        }
    }
}
