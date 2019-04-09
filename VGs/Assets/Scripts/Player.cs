﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //Player stats

    //Health
    public int health;

    private bool lookR;

    //Jumping
    private bool isSide;
    private bool sideLeft;
    private bool isJumping;
    private float lastj;

    //Dashing
    private bool isDashing, canDash;
    private float lastd;
    private Coroutine dashCoroutine;

    //Enemy drop score
<<<<<<< HEAD
    public int enemyDrop;
=======
    private int enemyDrop;
>>>>>>> 615d7e277024eaf2ab2dc36dce6e9be5c684dc93
    
    //Sword
    public GameObject sword;
    private bool canMove;
    private bool isGrounded;
    private float lasts;

    //Hands
    public Transform handR, handL, up, down;

    //Bullet
    public GameObject bullet;
    private bool canShoot;

    //Hook
    public static bool nothooking;
    private float distance;
    private float lasthook;
    private LayerMask lm;
    private bool hooking, canHook;
    private LineRenderer lr;
    private RaycastHit2D hit;


    //Setters y getters de salud y souls
    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public int GetSouls()
    {
        return enemyDrop;
    }

    public void SetEnemyDrop(int newEnemyDrop)
    {
        enemyDrop = newEnemyDrop;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        nothooking = true;
        this.canMove = true;
        this.lookR = true;
        this.health = 100;
        this.enemyDrop = 0;
        this.canShoot = true;
        this.canDash = true;
        this.isSide = false;
        this.isDashing = false;
        this.isGrounded = false;
        this.isJumping = false;

        lr = GetComponent<LineRenderer>();
        lr.endWidth = lr.startWidth = 0.3f;

        distance = 10f;
        lasthook = 0;
        lm = ~(1 << 12);
        hooking = false;
        canHook = true;
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
        float hook = Input.GetAxisRaw("Hook");
        float pause = Input.GetAxisRaw("Pause");


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
            if (isSide) {
                if(sideLeft)
                    rb.AddForce(new Vector2(-6, 16), ForceMode2D.Impulse);
                else {
                    rb.AddForce(new Vector2(6, 16), ForceMode2D.Impulse);
                }
            }
            else {
                rb.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
            }
        }
        this.lastj = j;
        this.isGrounded = false;

        //Sword Attack
        if (s==1 &&lasts==0&& canMove)
        {
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

        // Hook
        if (hook == 1 && lasthook == 0 && (h != 0 || v != 0) && canHook) {
            hit = Physics2D.Raycast(transform.position, new Vector2(h, v), distance, lm);
            if (hit.collider != null) {
                canHook = false;
                nothooking = false;
                hooking = true;

                lr.enabled = true;
            }
            else print(null);
        }

        if (hooking) {
            Vector3[] vect = { transform.position, hit.point };
            lr.SetPositions(vect);
            rb.velocity = (hit.point - (Vector2)transform.position).normalized * 10;
            if (hook == 0 && lasthook == 1) {
                lr.enabled = false;
                nothooking = true;
                rb.velocity = Vector2.up;
                hooking = false;
            }
        }
        lasthook = hook;

<<<<<<< HEAD
        //FUnciones de UI
        //Activar Menu de Pausa
        if (pause == 1)
        {
            GenericWindow.manager.Open(1);
            Time.timeScale = 0;
        }

        //Muerte por vida = 0
        if (health == 0)
        {
            GameOver();
        }
    }

    //Guarda el estado del juego y manda a ventana Game Over
    private void GameOver()
    {
        //Congelar el tiempo por pausa
        Time.timeScale = 0;

        //Guarda estado del juego

        //Abre ventana de GameOver
        GenericWindow.manager.Open(2);
=======
        //Pause Menu
        if (pause==1)
        {
            Time.timeScale = 0;

        }
>>>>>>> 615d7e277024eaf2ab2dc36dce6e9be5c684dc93
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Contact with drop
        if (collider.gameObject.layer == 13)
        {
            this.enemyDrop++;
            Destroy(collider.gameObject);
        }

        // Contact with enemy proyectile
        if (collider.gameObject.layer == 14)
        {
            StopCoroutine("takeDamage");
            StartCoroutine("takeDamage");
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        // collision with enemy
        if (collision.gameObject.layer == 10)
        {
            StopCoroutine("takeDamage");
            StartCoroutine("takeDamage");
        }

        // collision with floors or fake floors
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15) {
            nothooking = true;
            hooking = false;
            lr.enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(collision.GetContact(0).point, (Vector2)collision.transform.position - collision.GetContact(0).point);

            float y = hit.point.y - hit.transform.position.y;
            float limit = (float)collision.transform.lossyScale.y * 0.95f / 2;
            if (y >= limit) { // top
                this.canHook = true;
            }
            else if (y <= -limit) { // bottom

            }
            else { // side
                this.canHook = true;
            }
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // collision with floors or fake floors
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
        {
            

            RaycastHit2D hit = Physics2D.Raycast(collision.GetContact(0).point,(Vector2)collision.transform.position- collision.GetContact(0).point);

            float y = hit.point.y - hit.transform.position.y;
            float limit =(float) collision.transform.lossyScale.y*0.95f / 2;
            if (y >= limit) { // top
                this.isJumping = false;
                this.isGrounded = true;
                this.canDash = true;
            }
            else if( y <= -limit) { // bottom

            }
            else { // side
                if (hit.point.x > hit.transform.position.x) {
                    print("right");
                    sideLeft = false;
                }
                else {
                    print("left");
                    sideLeft = true;
                }
                this.isSide = true;
                this.isJumping = false;
                this.isGrounded = true;
                this.canDash = true;
            }
        }    
    }

    private void OnCollisionExit2D(Collision2D collision) {
<<<<<<< HEAD
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
            this.isSide = false;
=======
        print("exit");
        this.isSide = false;
>>>>>>> 615d7e277024eaf2ab2dc36dce6e9be5c684dc93
    }

    IEnumerator takeDamage() {
        sr.color = Color.red;
        this.health -= 10;
        yield return new WaitForSeconds(1);
        sr.color = Color.blue;
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
