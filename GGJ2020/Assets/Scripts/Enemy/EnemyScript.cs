using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //for audio
    public AudioSource audioSource;
    public AudioClip EnemyShootSound;
    public AudioClip EnemyDestroy;
    public AudioClip HitHurt;

    //for pickup
    public GameObject pickUp;
    public PlayerControlScript playerScript;
    public GameObject pickUpObject;


    //info for enemy bullets
    public GameObject bullet;
    public float enemyBulletVelocity;
    public GameObject enemyBullet;
    public GameObject enemyBullet1;
    public Transform enemyBulletTransform;
    public int shootTimer = 0;
    public int timeToShoot;

    //player object
    public GameObject player;

    //movement behaviour for enemy
    public bool inAttackRange;
    public int attackStyle = 0;
    public bool sideMovement;
    public bool retreatMovement;
    public bool normalMovement;

    //more movement info for enemy
    private float latestDirectionChangeTime;
    public float directionChangeTime = 3f;
    private float characterVelocity = 2f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private Rigidbody2D rb;

    //for orbital enemy movement
    public float posX, posY, angle;

    //gets size of enemy
    public Transform enemyTransform;
    public float enemyScaleX = 0.02f;
    public float enemyScaleY = 0.02f;

    //enemy health
    public float enemyHP;
    public int hitCounter;
    public bool runDeath = false;

    //tracks the enemy's original strength
    public float startingEnemyHP;
    public Vector3 startingBulletSize;
    public float startingMoveSpeed;


    //GET PLAYERS HEALTH, if their health is half of that of the enemy, their bullets do not damage the player and the 
    //player cannot damage the enemy
    //enemies will also ignore players who are half their health or lower
    //add item drops
    //spawner 
    public void Start()
    {
        timeToShoot = Random.Range(40, 60);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();
        enemyBulletTransform = enemyBullet1.transform;
        enemyBulletTransform.localScale = new Vector3(0.25f, 0.25f, 0);
        enemyTransform = transform;
        enemyBulletVelocity = playerScript.bulletVelocity / 1f;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        attackStyle = 0;
        if (attackStyle == 0)
        {
            normalMovement = true;
        }

        rb = this.GetComponent<Rigidbody2D>();
        startingEnemyHP = enemyTransform.localScale.x;
        //for enemy normal movement
        latestDirectionChangeTime = -5f;
        NormalMovement();
        startingBulletSize = playerScript.bulletTransform.localScale;
    }

    public void Update()
    {
        if (enemyTransform.localScale.x < playerScript.playerTransform.localScale.x / 2 &&
            hitCounter == 0)
        {
            Destroy(this.gameObject);
        }
        if(hitCounter >= 3)
        {
            GotShot();
        }
        //enemy health
        enemyHP = enemyTransform.localScale.x;

        if (this.enemyTransform.localScale.x < 0 && runDeath == false)
        {
            Destroy(this.gameObject);
            runDeath = true;
            GotShot();
        }

        if (normalMovement == true)
        {
            NormalMovement();
        }

        if (sideMovement == true)
        {
            SideMovement();
        }

        if (retreatMovement == true)
        {
            RetreatMovement();
        }

        if (shootTimer >= timeToShoot && inAttackRange == true)
        {
            EnemyShoot();
            shootTimer = 0;
            timeToShoot = Random.Range(40, 60);
        }
        else
        {
            if (shootTimer <= 60)
            {
                shootTimer++;
            }
        }
    }

    public void EnemyShoot()
    {
        Vector3 playerPosition = player.transform.position;
        Vector2 direction = (Vector2)((playerPosition - transform.position));
        direction.Normalize();

        // Creates the bullet locally
        bullet = (GameObject)Instantiate(
                        this.enemyBullet1,
                        transform.position + (Vector3)(direction * 0.5f),
                        Quaternion.identity);
        if (bullet != null)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = direction * enemyBulletVelocity;
            audioSource.clip = EnemyShootSound;
            audioSource.Play();
            this.bullet.transform.localScale = startingBulletSize;
        }
        /*
        enemyBulletTransform = this.enemyBullet1.transform;

        //make enemy smaller when shooting
        enemyTransform.localScale -= new Vector3(enemyScaleX / 0.25f, enemyScaleY / 0.25f, 0);
        enemyBulletTransform.localScale -= new Vector3(enemyScaleX / 1f, enemyScaleY / 1f, 0);
        enemyBulletVelocity--;
        */
    }

    public void resetMovement()
    {
        attackStyle = 0;
        normalMovement = true;
        retreatMovement = false;
        sideMovement = false;
    }
    public void MovementType()
    {
        attackStyle = Random.Range(1, 3);
        //attackStyle = 3;

        if (attackStyle == 0)
        {
            normalMovement = true;
            retreatMovement = false;
            sideMovement = false;
        }
        if (attackStyle == 1)
        {
            sideMovement = true;
            retreatMovement = false;
            normalMovement = false;
            //side to side movement
        }
        if (attackStyle == 2)
        {
            retreatMovement = true;
            sideMovement = false;
            normalMovement = false;
        }
    }

    public void NormalMovement()
    {
        //after a certain amount of time, change direction of enemy movement
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            movementPerSecond = movementDirection * characterVelocity;
        }

        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
        transform.position.y + (movementPerSecond.y * Time.deltaTime));
    }

    public void SideMovement()
    {
        Vector3 direction = player.transform.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        movementDirection = direction;

        rb.MovePosition(transform.position + (direction / 2 * Time.deltaTime));
    }

    public void RetreatMovement()
    {
        Vector3 direction = player.transform.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        movementDirection = direction;

        rb.MovePosition(transform.position - (direction / 2.5f * Time.deltaTime));
    }

    public void GotShot()
    {
        if (enemyTransform != null)
        {
            this.enemyTransform.localScale -= new Vector3(enemyScaleX / 0.125f, enemyScaleY / 0.125f, 0);
        }
        //this.enemyBulletTransform.localScale -= new Vector3(enemyScaleX / 0.5f, enemyScaleY / 0.5f, 0);
        if (bullet != null)
        {
            this.bullet.transform.localScale -= new Vector3(enemyScaleX / 0.5f, enemyScaleY / 0.5f, 0);
        }
        this.startingBulletSize -= new Vector3(enemyScaleX / 0.5f, enemyScaleY / 0.5f, 0);
        this.enemyBulletVelocity--;
        if (hitCounter <= 2)
        {
            audioSource.clip = HitHurt;
            audioSource.Play();
        }
        if (hitCounter >= 3 || runDeath == true)
        {
            for (int i = 0; i < Random.Range(8, 16); ++i)
            {
                pickUpObject = (GameObject)Instantiate(
                       pickUp,
                       this.gameObject.transform.position += new Vector3 (Random.Range(-1f,1f),
                                                                           Random.Range(-1f, 1f), 0),                                                
                       Quaternion.identity);
                //this.pickUpObject.transform.localScale = playerScript.bulletTransform.localScale;
                this.pickUpObject.transform.localScale += startingBulletSize * 16;
            }
            audioSource.clip = EnemyDestroy;
            AudioSource.PlayClipAtPoint(EnemyDestroy, gameObject.transform.position );
            //add item drop
            Invoke("DestroyObject", 1f);
            this.gameObject.SetActive(false);
        }
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "PlayerBullet" && this.gameObject.tag == "Enemy")
        {
            hitCounter++;
            GotShot();
            Destroy(col.gameObject);
        }
    }
}