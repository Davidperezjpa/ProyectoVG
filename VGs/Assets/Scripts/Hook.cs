using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private float distance;
    private Vector2 targetPosition;
    private float lasthook;
    private Rigidbody2D rb;
    private LayerMask lm;
    private bool hooking, canHook;
    private LineRenderer lr;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.endWidth = lr.startWidth = 0.3f;
        
        distance = 10f;
        lasthook = 0;
        rb = GetComponent<Rigidbody2D>();
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
                
                lr.enabled = true;
            }
            else print(null);
        }

        if (hooking)
        {
            Vector3[] vect = { transform.position, hit.point };
            lr.SetPositions(vect);
            rb.velocity = (hit.point - (Vector2) transform.position).normalized * 10;
            if (hook == 0 && lasthook == 1)
            {
                lr.enabled = false;
                Player.nothooking = true;
                rb.velocity = Vector2.up;
                hooking = false;
            }
        }
        lasthook = hook;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player.nothooking = true;
        hooking = false;
        lr.enabled = false;

        if (collision.gameObject.layer == 8)
        {
            canHook = true;
        }
    }
}