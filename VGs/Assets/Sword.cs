using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.SetParent(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.layer == 15)
        {
            Destroy(collider.gameObject);
        }
    }
}
