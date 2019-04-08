using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDispara : MonoBehaviour
{
    public Waypoint[] path;
    public float treshold;
    private int current;

    private Rigidbody2D rb;

    public GameObject drop,
                      proyectile;

    private GameObject player;

    private bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        shooting = false;
        current = 0;
        StartCoroutine("Move");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 10)
        {
            if (!shooting)
            {
                StartCoroutine("Shoot");
                shooting = true;
            }
        }
        else
        {
            if (shooting)
            {
                StopCoroutine("Shoot");
                shooting = false;
            }
        }
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

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject bulletI = Instantiate(proyectile, transform.position, transform.rotation);
            bulletI.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x - bulletI.transform.position.x, player.transform.position.y - bulletI.transform.position.y), ForceMode2D.Impulse);

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
