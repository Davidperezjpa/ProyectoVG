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

    //Hook

    // Start is called before the first frame update
    void Start()
    {
        nothooking = true;
        this.canMove = true;
        this.lookR = true;
        rb = GetComponent<Rigidbody2D>();
        this.enemyDrop = 0;
        this.canShoot = true;
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
            print(h + ".." + v);
            if (Mathf.Abs(h) > Mathf.Abs(v) || v==0)
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
        if (b ==1 && canShoot)
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
        if (h!=0 && d>=0.5 && lastd<0.5 && !isDashing && canDash && canMove)
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
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
        {
            this.isJumping = false;
            this.isGrounded = true;
            this.canDash = true;

            RaycastHit2D hit = Physics2D.Raycast(collision.GetContact(0).point,(Vector2)collision.transform.position- collision.GetContact(0).point);

            float y = hit.point.y - hit.transform.position.y;
            float limit =(float) collision.transform.lossyScale.y*0.95f / 2;
            print(limit);
            if (y >= limit) {
                print("top");
            }
            else if( y <= -limit) {
                print("bottom");
            }
            else {
                print("side");
            }
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
}
