using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : GenericWindow
{
    //Stats del jugador
    public Player player;

    //Elementos del panel
    public Text enemyDropText,
                healthText,
                levelText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.health;
        enemyDropText.text = "Soul: " + player.enemyDrop;
        levelText.text = "Lvl: " + player.level;

    }
}
