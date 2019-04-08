using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public Waypoint[] path;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);

        Gizmos.color = Color.green;
        foreach (Waypoint current in path)
        {
            Gizmos.DrawLine(transform.position, current.transform.position);
        }
    }
}