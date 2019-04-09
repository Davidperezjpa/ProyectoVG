using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : GenericWindow
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Funcion para boton Continue
    //Oculta la vista de pausa y reanuda el juego
    public void Continue()
    {
        Time.timeScale = 1;
        manager.Open(0);
    }

    //funcion para el boton de Options
    //Abre la vista de Options
    public void Options()
    {

    }

    //funcion para el boton de Exit Game
    //Guarda el estado del juego, cierra esta escena y abre el menu principal
    public void ExitGame()
    {
        //Reestablecer tiempo
        Time.timeScale = 1;

        //Guardar estado del juego
        

        //Cerrar esta escena


        //Abrir Menu Principal
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }

}
