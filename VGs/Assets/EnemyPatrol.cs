using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;

    private int sentido = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(this.sentido * 7 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9)
        {
            this.sentido *= -1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When a proyectile hits the enemy
        if (collision.gameObject.layer == 11)
        {
            Destroy(collision.gameObject);
            Destroy(this);
        }
        //When an enemy collides with player
        else if (collision.gameObject.layer == 12)
        {

        }
    }
}
