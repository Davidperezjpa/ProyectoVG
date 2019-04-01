using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private float distance;
    private Vector2 targetPosition;
    private float lasthook,x,y;
    private Rigidbody2D playerRb;
    private LayerMask lm;
    private bool hooking, canHook;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        distance = 10f;
        lasthook = 0;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        lm = ~(1 << 12);
        hooking = false;
        canHook = true;
    }

    // Update is called once per frame
    void Update()
    {
        float hook = Input.GetAxisRaw("Hook");
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (hook == 1 && lasthook == 0 && (h!=0 || v!=0) && canHook)
        {
            hit = Physics2D.Raycast(transform.position,new Vector2(h,v), distance,lm);
            if (hit.collider != null)
            {
                canHook = false;
                Player.nothooking = false;
                hooking = true;
                x = hit.point.x - transform.position.x;
                y = hit.point.y - transform.position.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    y = y / Mathf.Abs(x);
                    if (x >= 0) x = 1; else x = -1;
                }
                else if (Mathf.Abs(y) > Mathf.Abs(x))
                {
                    x = x / Mathf.Abs(y);
                    if (y >= 0) y = 1; else y = -1;
                }
                else
                {
                    x = y = 1;
                }
            }
            else print(null);
        }

        if (hooking)
        {
            print(x + "......" + y);
            playerRb.velocity = new Vector2(x*10,y*10);
            if (hook == 0 && lasthook == 1)
            {
                Player.nothooking = true;
                playerRb.velocity = Vector2.up;
                hooking = false;
            }
        }
        lasthook = hook;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player.nothooking = true;
        hooking = false;
        if (collision.gameObject.layer == 8)
        {
            canHook = true;
        }
    }
}