using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : GenericWindow
{
    public AudioSource backgroundMusic;

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
        if (SceneManager.GetActiveScene().name == "Nivel1")
        {
            Time.timeScale = 1;
            this.backgroundMusic.volume = 0.5f;
            this.Close();
            manager.Open(0);
        }
        else if (SceneManager.GetActiveScene().name == "BossFigth")
        {
            Time.timeScale = 1;
            this.backgroundMusic.volume = 0.5f;
            this.Close();
        }
    }

    /*
    //funcion para el boton de Options
    //Abre la vista de Options
    public void Options()
    {

    }
    */

    //funcion para el boton de Exit Game
    //Guarda el estado del juego, cierra esta escena y abre el menu principal
    public void ExitGame()
    {
        //Reestablecer tiempo
        Time.timeScale = 1;

        //Parar música
        this.backgroundMusic.Stop();

        //Guardar estado del juego


        //Cerrar este panel
        this.Close();

        //Abrir Menu Principal
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }

}
