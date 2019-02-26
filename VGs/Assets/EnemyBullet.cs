using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destroy - used to delete a whole game object or a particular component
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
