using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : GenericWindow
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Continuar el juego desde el ultimo spawn point disponible
    public void Continue()
    {
        //Reestablecer el tiempo
        Time.timeScale = 1;

        //Aparecer en el ultimo spawn point

        //Permadeath
        SceneManager.LoadScene("Nivel1", LoadSceneMode.Single);
    }

    //Regresar al menu principal
    public void ExitGame()
    {
        //Reestablecer el tiempo
        Time.timeScale = 1;

        //Abrir Menu Principal
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }
}
