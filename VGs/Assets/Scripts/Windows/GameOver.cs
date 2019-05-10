using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : GenericWindow
{
    public Player player;
    public AudioSource gameOverSource;
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        this.backgroundMusic.Stop();
        this.gameOverSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Continuar el juego desde el ultimo spawn point disponible
    public void Continue()
    {
        if (SceneManager.GetActiveScene().name == "Nivel1")
        {
            //Reestablecer el tiempo
            Time.timeScale = 1;

            //Reactivar música
            this.gameOverSource.Stop();
            this.backgroundMusic.Play();

            //Aparecer en el ultimo spawn point
            player.respawn();
            manager.Open(0);
        }
        else if (SceneManager.GetActiveScene().name == "BossFigth")
        {
            Time.timeScale = 1;

            //Reactivar música
            this.gameOverSource.Stop();
            this.backgroundMusic.Play();
            SceneManager.LoadScene("BossFigth", LoadSceneMode.Single);
        }
    }

    //Regresar al menu principal
    public void ExitGame()
    {
        //Reestablecer el tiempo
        Time.timeScale = 1;

        //Parar música
        this.gameOverSource.Stop();

        //Cerrar esta ventana
        this.Close();

        //Abrir Menu Principal
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }
}