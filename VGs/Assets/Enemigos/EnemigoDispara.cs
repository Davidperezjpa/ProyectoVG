using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDispara : MonoBehaviour
{
    public Waypoint[] path;
    public float treshold;
    private int current;

    private int currentHealth;
    private int maxHealth = 100;

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
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 20)
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

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;

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
            Destroy(bulletI, 5);
            bulletI.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x - bulletI.transform.position.x, player.transform.position.y - bulletI.transform.position.y).normalized * 7, ForceMode2D.Impulse);

        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //When a proyectile hits the enemy
        if (collider.gameObject.layer == 11)
        {
            ModifyHealth(-25);
            print("Pego proyectil current Hp: " + currentHealth);
            if (currentHealth < 0)
            {
                player.GetComponent<Player>().GainExperience(5);        //Gana 5 puntos de experiencia
                Instantiate(drop, transform.position, transform.localRotation);
                Destroy(collider.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(collider.gameObject);
            }

        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When Sword hits the enemy
        if (collision.gameObject.layer == 16)
        {
            print("Pego espada current Hp: " + currentHealth);
            ModifyHealth(-50);
            if (currentHealth < 0)
            {
                player.GetComponent<Player>().GainExperience(10);        //Gana 10 puntos de experiencia
                Instantiate(drop, transform.position, transform.localRotation);
                Destroy(this.gameObject);
            }
            else
            {

            }

        }
    }
}
