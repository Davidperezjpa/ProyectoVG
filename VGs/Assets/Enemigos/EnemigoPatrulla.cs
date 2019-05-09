using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    public Waypoint[] path;
    public float treshold;
    private int current;

    private int currentHealth;
    private int maxHealth = 100;

    private Rigidbody2D rb;

    public GameObject drop;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        StartCoroutine("Move");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, path[current].transform.position, Time.deltaTime * 3);
    }


    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;

    }
    public int GetCurrentHealth()
    {
        return currentHealth;
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
