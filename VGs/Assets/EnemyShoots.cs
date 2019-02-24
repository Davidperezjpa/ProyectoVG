using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    private float lastJ;
    private int sentido = -1;

    public GameObject player;
    public GameObject bullet;
    public Transform lHand,
                     rHand;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement - leave one commented

        //Patrol-like movement
        transform.Translate(5 * this.sentido * Time.deltaTime, 0, 0);

        //Follower-like movement
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

        transform.Translate(5 * h * Time.deltaTime, 0, 0);

        //Shooting
        //Other posibilities:
        //-when on same Y axis
        //-at any direction every X seconds

        //Shoot every X seconds sideways
        if (this.sentido == 1)
        {
            Instantiate(bullet, rHand.position, rHand.rotation);
        }
        else
        {
            Instantiate(bullet, lHand.position, lHand.rotation);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 9)
        {
            this.sentido *= -1;
        }
    }
}
