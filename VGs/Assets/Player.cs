using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    //Player stats
    //public int health = 10;

    //Jumping
    private bool isJumping;

    //Dashing
    private bool isDashing;
    private Coroutine dashCoroutine;

    //Enemy drop score
    public Text textito;
    private int enemyDrop;

    //Sword
    public GameObject sword;
    private bool canMove;
    private bool isGrounded;

    //Hands
    public Transform handR, handL, up, down;

    //Bullet
    public GameObject bullet;
    private bool canShoot;

    //Hook
    private bool canHook;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.enemyDrop = 0;
        this.canMove = true;
        this.canShoot = true;
        this.canHook = true;

        this.isDashing = false;
        this.isGrounded = false;
        this.isJumping = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Enemy drop score
        textito.text = "Soul: " + enemyDrop;

        //Horizontal movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(canMove|| !isGrounded)
        {
            transform.Translate(h * 5 * Time.deltaTime, 0, 0, Space.World);
        }
        
        //Jumping movement
        if (Input.GetKeyDown(KeyCode.Space) && !this.isJumping)
        {
            this.isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
        }
        this.isGrounded = false;

        //Sword Attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && canMove)
        {
            if (h > 0  && v==0) //Right
            {
                Instantiate(sword, handR.position, handR.rotation);
                
            }
            else if(h < 0 && v == 0) //Left
            {
                Instantiate(sword, handL.position, handL.rotation);
                
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
        }

        //Shooting
        if (Input.GetKeyDown(KeyCode.Mouse1) && canShoot)
        {
            if (h > 0 && v == 0) // Right
            {
                Instantiate(bullet, handR.position, handR.rotation);
                StartCoroutine(bulletCooldown());
            }
            else if (h < 0 && v == 0) //Left
            {
                Instantiate(bullet, handL.position, handL.rotation);
                StartCoroutine(bulletCooldown());
            }
            else if (v > 0) //Up
            {
                Instantiate(bullet, up.position, up.rotation);
                StartCoroutine(bulletCooldown());
            }
            else if (v < 0) //Down
            {
                Instantiate(bullet, down.position, down.rotation);
                StartCoroutine(bulletCooldown());
            }
        }

        //Dash movement
        if (Input.GetKey(KeyCode.LeftShift) && !isDashing)
        {
            if (h > 0) //Derecha
            {
                this.isDashing = true;
                rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
                dashCoroutine = StartCoroutine(dashCooldown());
            }
            else if (h < 0) // Izquierda
            {
                this.isDashing = true;
                rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
                dashCoroutine = StartCoroutine(dashCooldown());
            }
        }

        //Hook
        if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            this.isJumping = false;
            this.isGrounded = true;
        }
    }

    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        rb.velocity = new Vector2(0,rb.velocity.y);
        yield return new WaitForSeconds(1f);
        this.isDashing = false;
        StopCoroutine(dashCooldown());
    }

    IEnumerator swordCooldown()
    {
        this.canMove = false;
        if (isGrounded)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.1f);
        this.canMove = true;
        StopCoroutine(swordCooldown());
    }
    IEnumerator bulletCooldown()
    {
        this.canShoot = false;
        yield return new WaitForSeconds(3);
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
