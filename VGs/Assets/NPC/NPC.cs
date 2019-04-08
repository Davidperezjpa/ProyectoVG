using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("Inventory"))
        {
            double distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 5)
            {



            }


        }
    }
}
