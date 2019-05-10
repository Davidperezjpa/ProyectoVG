using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : GenericWindow
{
    //public Button continueButton;


    //Abre esta ventana verificando si el boton de continue es necesario
    public override void Open()
    {
        /*
        bool canContinue = false;
        continueButton.gameObject.SetActive(canContinue);

        if (continueButton.gameObject.activeSelf)
        {
            firstSelected = continueButton.gameObject;
        }
        */

        base.Open();
    }
    /*
    //Continuar el juego
    public void Continue()
    {
        SceneManager.LoadScene("Test", LoadSceneMode.Single);
    }
    */

    //Abrir un nuevo juego
    public void NewGame()
    {
        SceneManager.LoadScene("Nivel1", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    protected override void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Open();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
