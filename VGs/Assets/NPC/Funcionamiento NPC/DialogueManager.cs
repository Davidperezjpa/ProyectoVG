using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

   
    
    void Start()
    {

        
        sentences = new Queue<string>();        //instancia la queue

        

    }
    

    public void StartDialogue (Dialogue dialogue)   //Entra el dialogo.
    {
        
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.nameNPC;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)     //pone en un queue las oraciones
        {
            sentences.Enqueue(sentence);
        }


        DisplayNextSentences();

    }
    public void StopDialogue() {
        StartCoroutine("EndDialogue");
    }


    public void DisplayNextSentences()      //hace Dequeue
    {
        if (sentences.Count == 0)
        {
            
            StartCoroutine(EndDialogue());
            return;
        }

        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);                      para mostrar en consola el dialogo

        dialogueText.text = sentence;
        
        



    }

    IEnumerator EndDialogue()   //termina el dialogo
    {
        dialogueText.text = "End Of conversation.";
        yield return new WaitForSeconds(1f);
        
        animator.SetBool("IsOpen", false);

    }

    

}
