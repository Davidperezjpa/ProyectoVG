using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : GenericWindow
{
    public Button continueButton;

    public override void Open()
    {
        bool canContinue = false;
        continueButton.gameObject.SetActive(canContinue);

        if (continueButton.gameObject.activeSelf)
        {
            firstSelected = continueButton.gameObject;
        }

        base.Open();
    }

    public void Continue()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Test", LoadSceneMode.Single);
    }

    public void Options()
    {

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
