using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessRange : MonoBehaviour
{

    public EnemyScript parentScript;
    // Start is called before the first frame update
    public void Start()
    {
        EnemyScript parentScript = this.GetComponentInParent<EnemyScript>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player" && parentScript.retreatMovement == false)
        {
            parentScript.inAttackRange = true;
            //set attack movement style
            parentScript.MovementType();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player" && parentScript.retreatMovement == false)
        {
            parentScript.inAttackRange = false;
            //set non-attack movement style
            parentScript.resetMovement();
        }

        if (col.gameObject.name == "Player" && parentScript.retreatMovement == true)
        {
            Invoke("Retreating", 2f);
        }
    }

    public void Retreating()
    {
        parentScript.inAttackRange = false;
        //set non-attack movement style
        parentScript.resetMovement();
    }
}