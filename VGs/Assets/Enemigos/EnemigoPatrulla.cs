using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    public Waypoint[] path;
    public float treshold;
    private int current;

    private Rigidbody2D rb;

    public GameObject drop;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        StartCoroutine("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, path[current].transform.position, Time.deltaTime * 3);
    }

    IEnumerator Move()
    {
        while (true)
        {
            float distance = Vector2.Distance(transform.position, path[current].transform.position);

            if (distance < treshold)
            {
                current++;
                current %= path.Length;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //When a proyectile hits the enemy
        if (collider.gameObject.layer == 11)
        {
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            Instantiate(drop, transform.position, transform.localRotation);
            Destroy(this.gameObject);
        }
    }
}
