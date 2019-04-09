using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    //Objeto Jugador
    public Transform lookAt;

    //Array con los nombres de los diferentes espacios/escenas
    public string[] scenes = {"Hub", "Level1", "Level2", "Level3", "Level4", "FinalBoss", "Tienda"};

    //Boundaries de la camara
    private float boundX;
    private float boundY;

    //Velocidad
    private float speed = 0.2f;

    //Vector de posicion deseada
    private Vector3 desiredPosition;
    

    void Start()
    {
        //Obtener el nombre de la escena actual
        string sceneName = SceneManager.GetActiveScene().name;

        //Setter de boundaries actuales accediendo al array de escenas
        if (sceneName == scenes[0])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[1])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[2])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[3])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[4])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[5])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
        else if (sceneName == scenes[6])
        {
            this.boundX = 1f;
            this.boundY = 1f;
        }
    }

    //Es Late porque se ejecuta despues del movimiento del personaje
    void LateUpdate()
    {
        //Vector de cambio
        Vector3 delta = Vector3.zero;

        //Variables de cambio entre posicion de objeto y camara
        float dx = lookAt.position.x - transform.position.x;
        float dy = lookAt.position.y - transform.position.y;

        //X Axis
        if (dx > boundX || dx < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = dx - boundX;
            }
            else
            {
                delta.x = dx + boundX;
            }
        }

        //Y Axis
        if (dy > boundY || dy < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = dy - boundY;
            }
            else
            {
                delta.y = dy + boundY;
            }
        }

        //Movimiento de a camara
        desiredPosition = transform.position + delta;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed);
    }
}
