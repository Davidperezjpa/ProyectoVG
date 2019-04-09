using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{

    public GameObject player;
    private Player pscript;
    
    // Start is called before the first frame update
    void Start()
    {
        pscript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MejoraVida()
    {
        pscript.SetHealth(10);
    }

    public void MejoraEspada() {
    }

    public void MejoraSpeed()
    {
            pscript.SetSpeed(1);
    }
}
