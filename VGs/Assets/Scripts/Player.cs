using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //Animation
    public GameObject render;
    private Animator animator;

    //Player stats
    [HideInInspector]
    public int health, speed, experience, level;
    private bool lookR;
    private bool canTakeDamage;
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
    [HideInInspector]
    public int enemyDrop;

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

    //Game Music
    public AudioSource backgroundMusic;

    //Checkpoints and habilities
    public Transform[] respawns;
    private Vector3 lastTouched;
    private Vector3 initialPosition;
    public bool dashIsObtained,hookIsObtained,gunIsObtained,climbIsObtained;
    public AudioSource foundIt;
    public AudioSource cantAffordIt;

    public GameObject effects;
    private AudioActions aa;
    private bool walkingAudio;
    //Boss
    public Transform portal;

    // Start is called before the first frame update
    void Start()
    {
        walkingAudio = true;
        aa = effects.GetComponent<AudioActions>();
        this.speed = 5;
        this.health = 100;
        this.experience = 0;
        this.level = 0;
        rb = GetComponent<Rigidbody2D>();
        sr = render.GetComponent<SpriteRenderer>();
        animator = render.GetComponent<Animator>();
        nothooking = true;
        this.canTakeDamage = true;
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

        this.lastTouched = this.initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //sr.size = new Vector2(1f, 1f);

        //Inputs Ponganlos aqui cabrones y en string
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float j = Input.GetAxisRaw("Jump");
        float s = Input.GetAxisRaw("Sword");
        float b = Input.GetAxisRaw("Bullet");
        float d = Input.GetAxisRaw("Dash");
        float hook = Input.GetAxisRaw("Hook");
        float pause = Input.GetAxisRaw("Pause");

        //PRUEBA EXP------
        //print("exp: " + experience + "   " + "lvl: " + level);

        //--------




        //Horizontal movement
        
        if (h > 0.6) lookR = true;
        else if (h < -0.6) lookR = false;

        if ((canMove || !isGrounded) && Mathf.Abs(h) > 0.6 && nothooking && canWalk)
        {
            animator.SetBool("IsRunning", true);
            if (walkingAudio) {
                aa.PlayWalk();
                walkingAudio = false;
            }
            transform.Translate(h * speed * Time.deltaTime, 0, 0, Space.World);
            if (lookR == true)
            {
                sr.flipX = false;
            }
            else if (lookR == false)
            {
                sr.flipX = true;

            }
        }
        else
        {
            aa.StopWalk();
            walkingAudio = true;
            animator.SetBool("IsRunning", false);
        }

        //Jumping movement
        if (j == 1 && lastj == 0 && !this.isJumping && canMove)
        {
            aa.PlayJump();
            this.isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if (isSide)
            {
                if (sideLeft)
                {
                    StartCoroutine(sideMove(-1));
                }
                else
                {
                    StartCoroutine(sideMove(1));
                }
            }
            animator.SetBool("IsJumping", true);
            rb.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);

        }


        this.lastj = j;
        this.isGrounded = false;


        //Sword Attack
        if (s == 1 && lasts == 0 && canMove)
        {
            if (Mathf.Abs(h) > Mathf.Abs(v) || v == 0)
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
            aa.PlaySword();
            StartCoroutine(swordCooldown());
            lasts = s;
        }
        lasts = s;

        //Shooting
        if (b == 1 && canShoot && this.gunIsObtained)
        {

            if (lookR) // Right
            {
                aa.PlayShoot();
                Instantiate(bullet, handR.position, handR.rotation);
                StartCoroutine(bulletCooldown());
            }
            else if (!lookR) //Left
            {
                aa.PlayShoot();
                Instantiate(bullet, handL.position, handL.rotation);
                StartCoroutine(bulletCooldown());
            }
        }

        //Dash movement
        if (h != 0 && d >= 0.5 && lastd < 0.5 && !isDashing && canDash && canMove && this.dashIsObtained)
        {
            canDash = false;
            if (lookR) //Derecha
            {
                aa.PlayDash();
                sr.flipX = false;
                animator.SetBool("IsDashing", true);
                this.isDashing = true;
                rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
                dashCoroutine = StartCoroutine(dashCooldown());

            }
            else if (!lookR) // Izquierda
            {
                aa.PlayDash();
                sr.flipX = true;
                animator.SetBool("IsDashing", true);
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
        if (hook == 1 && lasthook == 0 && (h != 0 || v != 0) && canHook && this.hookIsObtained)
        {
            aa.PlayHook();
            hit = Physics2D.Raycast(transform.position, new Vector2(h, v), distance, lm);
            if (hit.collider != null)
            {
                canHook = false;
                nothooking = false;
                hooking = true;

                lr.enabled = true;
            }
            else print(null);
        }

        if (hooking)
        {
            animator.SetBool("IsHooking", true);
            Vector3[] vect = { transform.position, hit.point };
            lr.SetPositions(vect);
            rb.velocity = (hit.point - (Vector2)transform.position).normalized * 10;
            if (hook == 0 && lasthook == 1)
            {
                animator.SetBool("IsHooking", false);
                lr.enabled = false;
                nothooking = true;
                rb.velocity = Vector2.up;
                hooking = false;
            }
        }
        lasthook = hook;

        //Menu de pausa
        if (pause == 1)
        {
            this.backgroundMusic.volume = 0.25f;
            Time.timeScale = 0;
            GenericWindow.manager.Open(1);

        }

        //Muerte por vida = 0
        if (health == 0)
        {
            this.GameOver();
        }

        //Boss Teletransport
        if (Vector2.Distance(transform.position, this.portal.position) < 1)
        {
            this.backgroundMusic.Stop();
            SceneManager.LoadScene("BossFigth", LoadSceneMode.Single);
        }

        //Interacción con spawners
        for (int i = 0; i < this.respawns.Length; i++)
        {
            if (Vector2.Distance(transform.position, this.respawns[i].position) < 1)
            {
                Vector3 touched = this.respawns[i].position;
                if (touched == this.respawns[0].position)
                {
                    if (this.dashIsObtained == false)
                    {
                        if (this.level >= 1)
                        {
                            this.lastTouched = touched;
                            this.level--;
                            this.foundIt.Play();
                            this.dashIsObtained = true;
                        }
                        else
                        {
                            this.cantAffordIt.Play();
                        }
                    }
                }
                else if (touched == this.respawns[1].position)
                {
                    if (this.hookIsObtained == false)
                    {
                        if (this.level >= 1)
                        {
                            this.lastTouched = touched;
                            this.level--;
                            this.foundIt.Play();
                            this.hookIsObtained = true;
                        }
                        else
                        {
                            this.cantAffordIt.Play();
                        }
                    }
                }
                else if (touched == this.respawns[2].position)
                {
                    if (this.gunIsObtained == false)
                    {
                        if (this.level >= 1)
                        {
                            this.lastTouched = touched;
                            this.level--;
                            this.foundIt.Play();
                            this.gunIsObtained = true;
                        }
                        else
                        {
                            this.cantAffordIt.Play();
                        }
                    }
                }
                else
                {
                    if (this.climbIsObtained == false)
                    {
                        if (this.level >= 1)
                        {
                            this.lastTouched = touched;
                            this.level--;
                            this.foundIt.Play();
                            this.climbIsObtained = true;
                        }
                        else
                        {
                            this.cantAffordIt.Play();
                        }
                    }
                }
            }

        }
    }

    public bool[] getHabilities()
    {
        bool[] tmp = new bool[4];
        tmp[0] = this.dashIsObtained;
        tmp[1] = this.hookIsObtained;
        tmp[2] = this.gunIsObtained;
        tmp[3] = this.climbIsObtained;
        return tmp;
    }

    public void setHabilities(bool[] habilidades)
    {
        this.dashIsObtained = habilidades[0];
        this.hookIsObtained = habilidades[1];
        this.gunIsObtained = habilidades[2];
        this.climbIsObtained = habilidades[3];
    }

    private void GameOver()
    {
        //Abre ventana de GameOver
        GenericWindow.manager.Open(2);
        //Congelar el tiempo por pausa
        Time.timeScale = 0;
    }

    public void respawn()
    {
        this.backgroundMusic.Play();
        this.health = 100;
        this.transform.position = this.lastTouched;
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
        if (collider.gameObject.layer == 14 && canTakeDamage)
        {
            StartCoroutine("takeDamage");
            Destroy(collider.gameObject);
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // collision with floors or fake floors
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
        {
            animator.SetBool("IsHooking", false);
            nothooking = true;
            hooking = false;
            lr.enabled = false;

            RaycastHit2D hit = Physics2D.Raycast(collision.GetContact(0).point, (Vector2)collision.transform.position - collision.GetContact(0).point);
            //print("enter");
            float y = hit.point.y - hit.transform.position.y;
            float limit = (float)collision.transform.lossyScale.y * 0.90f / 2;
            if (y >= limit)
            { // top
                //print("top");
                animator.SetBool("IsJumping", false);
                this.canHook = true;
            }
            else if (y <= -limit)
            { // bottom
                //print("bot");
            }
            else
            { // side
                this.canHook = true;
                //print("side");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // collision with enemy
        if (collision.gameObject.layer == 10 && canTakeDamage)
        {
            StartCoroutine("takeDamage");
        }
        // collision with floors or fake floors
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
        {
            RaycastHit2D hit = Physics2D.Raycast(collision.GetContact(0).point, (Vector2)collision.transform.position - collision.GetContact(0).point);

            float y = hit.point.y - hit.transform.position.y;
            float limit = (float)collision.transform.lossyScale.y * 0.95f / 2;
            if (y >= limit)
            { // top
                this.isJumping = false;
                this.isGrounded = true;
                this.canDash = true;
            }
            else if (y <= -limit)
            { // bottom

            }
            else
            { // side
                if (hit.point.x > hit.transform.position.x)
                {
                    sideLeft = false;
                }
                else
                {
                    sideLeft = true;
                }
                this.isSide = true;
                this.isJumping = false;
                this.isGrounded = true;
                this.canDash = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
            this.isSide = false;
    }

    public int GetHealth()
    {
        return this.health;
    }
    public void SetHealth(int health)
    {
        this.health += health;
        if (this.health > 100)
        {
            this.health = 100;
        }
    }

    public int GetSpeed()
    {
        return this.speed;
    }
    public void SetSpeed(int speed)
    {
        this.speed += speed;
        if (this.speed > 10)
        {
            this.speed = 10;
        }
    }

    public void GainExperience(int exp)
    {
        this.experience += exp;
        if (this.experience >= 100)
        {
            this.level += 1;
            this.experience = 0;

        }
    }

    public int GetExperience()
    {
        return this.experience;
    }


    IEnumerator sideMove(int dir)
    {
        canMove = canWalk = false;
        float time = Time.time;
        while (time + 0.3f > Time.time)
        {
            transform.Translate(0.15f * dir - (float)(Time.time - time) / 4, 0, 0, Space.World);
            yield return new WaitForFixedUpdate();
        }
        canMove = canWalk = true;
    }

    IEnumerator takeDamage()
    {
        canTakeDamage = false;
        //sr.color = Color.red;
        this.health -= 10;
        if (health < 0) health = 0;
        yield return new WaitForSeconds(1);
        //sr.color = Color.blue;
        canTakeDamage = true;
    }

    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(0.4f);
        this.isDashing = false;
        animator.SetBool("IsDashing", false);

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
    }
    IEnumerator bulletCooldown()
    {
        this.canShoot = false;
        yield return new WaitForSeconds(1);
        this.canShoot = true;
    }
}
