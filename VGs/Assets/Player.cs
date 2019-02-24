using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    //Jumping
    private bool isGrounded;
    private int isJumping = 0;

    //Dashing
    private bool isDashing = false;
    private Coroutine dashCoroutine;

    //Enemy drop score
    public Text textito;
    private int enemyDrop;

    //Sword
    public GameObject sword;

    //Hands
    public Transform handR, handL, up, down;

    //Bullet
    public GameObject bullet;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.enemyDrop = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy drop score
        textito.text = "Soul: " + enemyDrop;

        //Horizontal movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(h *5* Time.deltaTime, 0, 0, Space.World);

        //Jumping movement
        if (Input.GetKeyDown(KeyCode.Space) && isJumping < 1)
        {
            rb.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
            this.isGrounded = false;
            this.isJumping++;
            
        }

        //Sword Attack
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (h > 0  && v==0)
            {
                Instantiate(sword, handR.position, handR.rotation);
                
            }
            else if(h < 0 && v == 0)
            {
                Instantiate(sword, handL.position, handL.rotation);
                
            }
            else if (v > 0)
            {
                Instantiate(sword, up.position, up.rotation);
            }
            else if (v < 0)
            {
                Instantiate(sword, down.position, down.rotation);
            }
        }

        //Shooting
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (h > 0 && v == 0)
            {
                Instantiate(bullet, handR.position, handR.rotation);

            }
            else if (h < 0 && v == 0)
            {
                Instantiate(bullet, handL.position, handL.rotation);

            }
            else if (v > 0)
            {
                Instantiate(bullet, up.position, up.rotation);
            }
            else if (v < 0)
            {
                Instantiate(bullet, down.position, down.rotation);
            }

        }

        //Dash movement
        if (Input.GetKey(KeyCode.LeftShift) && isDashing==false)
        {
            this.isDashing = true;
            if (h > 0) //Derecha
            {
                rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
            }
            else if (h < 0) // Izquierda
            {
                rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
            }

            dashCoroutine = StartCoroutine(dashCooldown());

        }

        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            this.isGrounded = true;
            this.isJumping = 0;
        }
    }

    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        this.isDashing = false;
        StopCoroutine(dashCooldown());
    }

}
