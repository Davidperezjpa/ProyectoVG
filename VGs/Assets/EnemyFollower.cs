using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    private Rigidbody2D rb;

    private float lastJ;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h;
        if (Input.GetAxis("Horizontal") > 0)
        {
            h = 1;
            this.lastJ = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            h = -1;
            this.lastJ = -1;
        }
        else
        {
            h = this.lastJ;
        }

       
        transform.Translate(7 * h * Time.deltaTime, 0, 0);
    }

    public void OnCollisionEnter(Collision collision)
    {
        //When a proyectile hits the enemy
        if (collision.gameObject.layer == 11)
        {
            print("colision");
            Destroy(collision.gameObject);
            Destroy(this);
        }
        //When an enemy collides with player
        else if (collision.gameObject.layer == 12)
        {

        }
    }

}
