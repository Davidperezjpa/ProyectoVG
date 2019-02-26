using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;

    private int sentido = -1;

    public GameObject drop,
                      corpse;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(this.sentido * 7 * Time.deltaTime, 0, 0);
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
        //When an enemy collides with another enemy
        if (collision.gameObject.layer == 10)
        {
            this.sentido *= -1;
        }
    }


}
