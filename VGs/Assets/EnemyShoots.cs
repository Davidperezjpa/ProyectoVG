using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoots : MonoBehaviour
{
    private Rigidbody2D rb;

    private float lastJ;
    private int sentido = -1;

    private GameObject player;
    public GameObject bullet;


    public GameObject drop,
                      corpse;


    private Coroutine shoot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        //Patrol-like movement
        transform.Translate(5 * this.sentido * Time.deltaTime, 0, 0);

    }

    //Shoot at player every X seconds
    IEnumerator shooting() {

        while (true)
        {

            yield return new WaitForSeconds(2f);
            GameObject bulletI = Instantiate(bullet, transform.position, transform.rotation);
            bulletI.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x - bulletI.transform.position.x, player.transform.position.y - bulletI.transform.position.y), ForceMode2D.Impulse);

        }
        
    }

    private void OnBecameVisible()
    {
        shoot = StartCoroutine(shooting());
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            this.sentido *= -1;
        }
        //When a proyectile hits the enemy
        if (collider.gameObject.layer == 11)
        {
            Destroy(Instantiate(corpse, transform.position, transform.localRotation), 2);
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When a proyectile hits the enemy
        if (collision.gameObject.layer == 11)
        {
            StopCoroutine(shooting());
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        //When an enemy collides with another enemy
        else if (collision.gameObject.layer == 10)
        {
            this.sentido *= -1;
        }
    }
}
