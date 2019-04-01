using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    private Rigidbody2D rb;

    private int sentido = 1;
    private int lastSentido = 1;
    private GameObject player;
    public GameObject drop,
                      corpse;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player.transform.position.x > this.transform.position.x)
        {
            this.sentido = this.lastSentido = 1;
        }
        else if (player.transform.position.x < this.transform.position.x)
        {
            this.sentido = this.lastSentido = -1;
        }
        else
        {
            this.sentido = this.lastSentido;
        }
       
        transform.Translate(3 * this.sentido * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //When a proyectile hits the enemy
        if (collider.gameObject.layer == 11)
        {
            Destroy(Instantiate(corpse, transform.position, transform.localRotation), 2);
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
       
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 16)
        {
            Destroy(Instantiate(corpse, transform.position, transform.localRotation), 2);
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(this.gameObject);
        }
    }



}
