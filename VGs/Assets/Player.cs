using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        transform.Translate(h *5* Time.deltaTime, 0, 0, Space.World);
        float jump = Input.GetAxisRaw("Jump");
        if(jump==1 && isGrounded)
        {
            rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }
}
