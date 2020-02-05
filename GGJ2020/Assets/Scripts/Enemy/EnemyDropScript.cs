using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropScript : MonoBehaviour
{
    public PlayerControlScript playerScript;
    //make pickups same size as player bullets
    public void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerScript.PlayerGrow();
            Destroy(this.gameObject);
        }
    }
}
