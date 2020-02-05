using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRogueProjectiles : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyBullet" || col.gameObject.tag == "PlayerBullet"
            || col.gameObject.tag == "Enemy" || col.gameObject.tag == "Pickup")
        {
            Destroy(col.gameObject);
        }
    }  
}
