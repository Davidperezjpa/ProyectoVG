using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //Player stats
    //public int health = 10;
    private bool lookR;
    private bool canWalk;

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
    public Text textito;
    private int enemyDrop;
    
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        nothooking = true;
        this.canWalk = true;
        this.canMove = true;
        this.lookR = true;
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


        //Texto souls
        textito.text = "Soul: " + enemyDrop;

        //Horizontal movement
        if (h > 0.6) lookR = true;
        else if (h < -0.6) lookR = false;
        
        if((canMove|| !isGrounded)&& Mathf.Abs(h)>0.6 &&nothooking && canWalk)
        {
            transform.Translate(h * 5 * Time.deltaTime, 0, 0, Space.World);
        }

        //Jumping movement
        if (j==1 && lastj==0 && !this.isJumping && canMove)
        {
            this.isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if (isSide) {
                if (sideLeft) {
                    StartCoroutine(sideMove(-1));
                }
                else {
                    StartCoroutine(sideMove(1));
                }
            }
            rb.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);
            
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
        // Contact with ground
        nothooking = true;
        hooking = false;
        lr.enabled = false;
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15) {
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
                    sideLeft = false;
                }
                else {
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
        this.isSide = false;
    }
    IEnumerator sideMove(int dir) {
        canMove = canWalk = false;
        float time = Time.time;
        while (time+0.3f>Time.time) {
            transform.Translate(0.15f*dir-(float)(Time.time-time)/4, 0, 0, Space.World);
            yield return new WaitForFixedUpdate();
        }
        canMove = canWalk = true;
    }

    IEnumerator takeDamage() {
        sr.color = Color.red;
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
