using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    //Player stats
    //public int health = 10;
    private bool lookR;

    //Jumping
    private bool isJumping;
    private float lastj;

    //Dashing
    private bool isDashing, canDash;
    private float lastd;
    private Coroutine dashCoroutine;

    //Enemy drop score
    public Text textito;
    private int enemyDrop;
    
    //Sword
    public GameObject sword;
    public static bool nothooking;
    private bool canMove;
    private bool isGrounded;
    private float lasts;

    //Hands
    public Transform handR, handL, up, down;

    //Bullet
    public GameObject bullet;
    private bool canShoot;
    private float lastb;

    //Hook
    private bool canHook;

    // Start is called before the first frame update
    void Start()
    {
        nothooking = true;
        this.canMove = true;
        this.lookR = true;
        rb = GetComponent<Rigidbody2D>();
        this.enemyDrop = 0;
        this.canShoot = true;
        this.canHook = true;
        this.canDash = true;

        this.isDashing = false;
        this.isGrounded = false;
        this.isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs Ponganlos aqui cabrones y en string
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float j = Input.GetAxisRaw("Jump");
        float s = Input.GetAxisRaw("Sword");
        float b = Input.GetAxisRaw("Bullet");
        float d = Input.GetAxisRaw("Dash");
        float pause = Input.GetAxisRaw("Pause");


        //Texto souls
        textito.text = "Soul: " + enemyDrop;

        //Horizontal movement
        if (h > 0.6) lookR = true;
        else if (h < -0.6) lookR = false;


        if((canMove|| !isGrounded)&& Mathf.Abs(h)>0.6 &&nothooking)
        {
            transform.Translate(h * 5 * Time.deltaTime, 0, 0, Space.World);
        }

        //Jumping movement
        if (j==1 && lastj==0 && !this.isJumping && canMove)
        {
            this.isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
        }
        this.lastj = j;
        this.isGrounded = false;

        //Sword Attack
        if (s==1 &&lasts==0&& canMove)
        {
            if (Mathf.Abs(h) >= Mathf.Abs(v))
            {
                if (lookR) //Right
                {
                    Instantiate(sword, handR.position, handR.rotation);

                }
                else if (!lookR) //Left
                {
                    Instantiate(sword, handL.position, handL.rotation);

                }
            }
            else if (v > 0) //Up
            {
                Instantiate(sword, up.position, up.rotation);
            }
            else if (v < 0) //Down
            {
                Instantiate(sword, down.position, down.rotation);
            }
            StartCoroutine(swordCooldown());
            lasts = s;
        }
        lasts = s;

        //Shooting
        if (b ==1 && lastb ==0 && canShoot)
        {

            if (lookR) // Right
            {
                Instantiate(bullet, handR.position, handR.rotation);
                StartCoroutine(bulletCooldown());
            }
            else if (!lookR) //Left
            {
                Instantiate(bullet, handL.position, handL.rotation);
                StartCoroutine(bulletCooldown());
            }
        }

        //Dash movement
        if (d>=0.5 && lastd<0.5 && !isDashing && canDash && canMove)
        {
            canDash = false;
            if (lookR) //Derecha
            {
                this.isDashing = true;
                rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
                dashCoroutine = StartCoroutine(dashCooldown());
            }
            else if (!lookR) // Izquierda
            {
                this.isDashing = true;
                rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
                dashCoroutine = StartCoroutine(dashCooldown());
            }
            lastd = d;
        }
        if (d < 0.5)
        {
            lastd = d;
        }

        //Pause Menu
        if (pause==1)
        {
            Time.timeScale = 0;

        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 13)
        {
            this.enemyDrop++;
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.layer == 14)
        {
            if (collider.gameObject.transform.position.x < transform.position.x)
            {
                rb.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
            }
            else if (collider.gameObject.transform.position.x < transform.position.x)
            {
                rb.AddForce(new Vector2(10 * -1, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When an enemy collides with player
        if (collision.gameObject.layer == 10)
        {
            if (collision.gameObject.transform.position.x < transform.position.x)
            {
                rb.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
            }
            else if (collision.gameObject.transform.position.x < transform.position.x)
            {
                rb.AddForce(new Vector2(10 * -1, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            }
        }
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            this.isJumping = false;
            this.isGrounded = true;
            this.canDash = true;
        }
    }

    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        rb.velocity = new Vector2(0,rb.velocity.y);
        yield return new WaitForSeconds(0.4f);
        this.isDashing = false;
        StopCoroutine(dashCooldown());
    }

    IEnumerator swordCooldown()
    {
        canMove = false;
        if (isGrounded)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.1f);
        canMove = true;
        StopCoroutine(swordCooldown());
    }
    IEnumerator bulletCooldown()
    {
        this.canShoot = false;
        yield return new WaitForSeconds(1);
        this.canShoot = true;
        StopCoroutine(bulletCooldown());
    }

    IEnumerator hookCooldown()
    {
        this.canHook = false;
        yield return new WaitForSeconds(1);
        this.canHook = true;
        StopCoroutine(hookCooldown());
    }
}
