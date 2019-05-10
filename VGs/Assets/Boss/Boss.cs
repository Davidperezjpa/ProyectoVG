using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public GameObject player;
    public GameObject pointL;
    public GameObject pointR;
    public GameObject projectile;
    public GameObject leftShoot;
    public GameObject rightShoot;
    public float speed;
    public bool switchTime;
    public float dashTime;
    public float shootTime;
    public Animator animator;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public AudioSource explosion;

    //Propiedades del Boss
    private int currentHealth;
    private int maxHealth = 3000;



    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        switchTime = true;
        StartCoroutine(dashCooldown());
        StartCoroutine(shootCooldown());

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > 1.5) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), Time.deltaTime * 3);
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }

        if (transform.position.x - player.transform.position.x < 0)
        {
            sr.flipX = true;
        } else
        {
            sr.flipX = false;
        }
        if (switchTime == true)
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < 2.5)
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Melee", true);
                StartCoroutine(meleeCooldown());
            }
            else
            {
                animator.SetFloat("Speed", speed);
                animator.SetBool("Melee", false);
            }
        }
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

    IEnumerator meleeCooldown()
    {
        switchTime = false;
        yield return new WaitForSeconds(1f);
        animator.SetFloat("Speed", speed);
        animator.SetBool("Melee", false);
        yield return new WaitForSeconds(2f);
        switchTime = true;
    }
    
    IEnumerator dashCooldown()
    {
        while (true)
        {
            switchTime = true;
            yield return new WaitForSeconds(9f);
            switchTime = false;
            animator.SetBool("Dash", true);
            if (transform.position.x - player.transform.position.x < 0)
            {
                sr.flipX = true;
                transform.position = Vector2.Lerp(transform.position, new Vector2(pointR.transform.position.x, transform.position.y), Time.deltaTime * 20);
            } else
            {
                sr.flipX = false;
                transform.position = Vector2.Lerp(transform.position, new Vector2(pointL.transform.position.x, transform.position.y), Time.deltaTime * 20);
            }
            yield return new WaitForSeconds(1f);
            animator.SetBool("Dash", false);
        }
    }

    IEnumerator shootCooldown()
    {
        while (true)
        {
            switchTime = true;
            yield return new WaitForSeconds(25f);
            switchTime = false;
            animator.SetBool("Shoot", true);
            GameObject bulletI1 = Instantiate(projectile, leftShoot.transform.position, leftShoot.transform.rotation);
            GameObject bulletI2 = Instantiate(projectile, rightShoot.transform.position, rightShoot.transform.rotation);
            Destroy(bulletI1, 5);
            Destroy(bulletI2, 5);
            bulletI1.GetComponent<Rigidbody2D>().AddForce(new Vector2(-30,0),ForceMode2D.Impulse);
            bulletI2.GetComponent<Rigidbody2D>().AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
            yield return new WaitForSeconds(2f);
            animator.SetBool("Shoot", false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        //When a proyectile hits the enemy
        if (collider.gameObject.layer == 11) {
            ModifyHealth(-25);
            print("Pego proyectil current Hp: " + currentHealth);
            if (currentHealth <= 0) {
                this.explosion.Play();
                player.GetComponent<Player>().GainExperience(1000);        //Gana 5 puntos de experiencia
                //Instantiate(drop, transform.position, transform.localRotation);
                Destroy(collider.gameObject);
                Destroy(transform.parent.gameObject);       //destruye el enemigo con todo y sus waypoints
                SceneManager.LoadScene("UI", LoadSceneMode.Single);
            }
            else {
                Destroy(collider.gameObject);
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //When Sword hits the enemy
        if (collision.gameObject.layer == 16) {
            print("Pego espada current Hp: " + currentHealth);
            ModifyHealth(-50);
            if (currentHealth <= 0) {
                this.explosion.Play();
                player.GetComponent<Player>().GainExperience(1000);        //Gana 10 puntos de experiencia
                //Instantiate(transform.position, transform.localRotation);
                Destroy(this.gameObject);
                SceneManager.LoadScene("UI", LoadSceneMode.Single);
            }
            else {

            }

        }
    }
}
