using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlScript : MonoBehaviour
{
    //audio stuff
    public AudioSource audioSource;
    public AudioClip PlayerShoot;
    public AudioClip HitHurt;
    public AudioClip Powerup;
    public AudioClip playerDieNoise;
    //info for bullets
    public float bulletVelocity;
    public GameObject bullet;
    public GameObject bullet1;
    public Transform bulletTransform;
    public string sceneToLoad;
    Vector2 moveDirection = new Vector2(0, 0);


    Rigidbody2D body;

    float horizontal;
    float vertical;

    public float moveSpeed = 20.0f;

    //gets size of player
    public Transform playerTransform;
    public float playerScaleX = 0.02f;
    public float playerScaleY = 0.02f;

    //other gameobjects to shrink or enlarge
    public GameObject backGround;
    public Transform backGroundTransform;
    public Camera playerCamera;

    //player health
    public float playerHP;
    //check players current bullet size to see if enemy bullets can hurt player
    //bullets %50 larger or more cannot hurt the player
    public float bulletSize;

    //death bool
    public bool runDeath = false;
    


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bulletTransform = bullet1.transform;
        bulletTransform.localScale = new Vector3(0.25f, 0.25f, 0);
        body = GetComponent<Rigidbody2D>();
        playerTransform = transform;
    }

    void Update()
    {
        if(playerTransform.localScale.x < 0 && runDeath == false)
        {
            AudioSource.PlayClipAtPoint(playerDieNoise, gameObject.transform.position);
            runDeath = true;
            Die();
        }
        //player health
        playerHP = playerTransform.localScale.x;

        //bullet size
        bulletSize = bulletTransform.localScale.x;

        //for movement of player
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();

        //If mouse is left-clicked, run shoot script
        if (Input.GetMouseButtonDown(0))
        {
            //run the scrip that shoots
            Debug.Log("Pressed primary button.");
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void Shoot()
    {
        //set up direction of bullet
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)((worldMousePos - transform.position));
        direction.Normalize();

        // Creates the bullet locally
        GameObject bullet = (GameObject)Instantiate(
                        bullet1,
                        transform.position + (Vector3)(direction * 0.5f),
                        Quaternion.identity);
        audioSource.clip = PlayerShoot;
        audioSource.Play();
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
        bulletTransform = this.bullet1.transform;

        //make player smaller when shooting
        playerTransform.localScale -= new Vector3(playerScaleX / 0.25f, playerScaleY / 0.25f, 0);
        bulletTransform.localScale -= new Vector3(playerScaleX / 2f, playerScaleY / 2f, 0);
        //backGroundTransform.localScale -= new Vector3(playerScaleX / 0.25f, playerScaleY / 0.25f, 0);
        playerCamera.orthographicSize += playerCamera.orthographicSize / -20f;
        bulletVelocity--;
        moveSpeed = moveSpeed + 0.50f;

    }

    public void PlayerGotShot()
    {
        playerTransform.localScale -= new Vector3(playerScaleX / 0.25f, playerScaleY / 0.25f, 0);
        bulletTransform.localScale -= new Vector3(playerScaleX / 2f, playerScaleY / 2f, 0);
        //backGroundTransform.localScale -= new Vector3(playerScaleX / 0.125f, playerScaleY / 0.125f, 0);
        playerCamera.orthographicSize += playerCamera.orthographicSize / -20f;
        bulletVelocity--;
        moveSpeed = moveSpeed + 0.50f;
        audioSource.clip = HitHurt;
        audioSource.Play();
    }

    public void PlayerGrow()
    {
        playerTransform.localScale += new Vector3(playerScaleX / 0.25f, playerScaleY / 0.25f, 0);
        bulletTransform.localScale += new Vector3(playerScaleX / 2f, playerScaleY / 2f, 0);
        //backGroundTransform.localScale += new Vector3(playerScaleX / 0.5f, playerScaleY / 0.5f, 0);
        playerCamera.orthographicSize -= playerCamera.orthographicSize / -20f;
        bulletVelocity++;
        moveSpeed = moveSpeed - 0.01f;
        audioSource.clip = Powerup;
        audioSource.Play();
    }

    public void Die()
    {
        //display death menu
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyBullet")
        {
            PlayerGotShot();
            Destroy(col.gameObject);
        }
    }
}
