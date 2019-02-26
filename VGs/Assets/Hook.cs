using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private DistanceJoint2D joint;
    private Vector3 targetPos;
    private RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    private Vector2 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            // for the use of controller where he is looking



            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            // for the use of controller where he is looking:
            // hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 1), distance, mask);

            // for the use of Mouse:
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

            if (hit.collider != null /*&&  hit.collider.gameObject.GetComponent<Rigidbody2D>() != null*/)
            {
                joint.enabled = true;
                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                //joint.distance = Vector2.Distance(transform.position, hit.point);  -> Works like a lazo
                joint.distance = 1/*hit.collider.bounds.size.y*/;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            joint.enabled = false;
        }

    }
}