using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Dialogue       //esta es la instancia del dialogo
{

    public string nameNPC;

    [TextArea(3, 10)]           //para hacer más grande los textbox en unity 
    public string[] sentences;  //aqui van las oraciones del dialogo


}
