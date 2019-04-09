using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject player;
    public Dialogue dialogue;
    private bool dialogueActive;




    // Start is called before the first frame update
    void Start()
    {
        dialogueActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Inventory") == 1)
        {
            double distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 5)
            {
                dialogueActive = true;

                if (dialogueActive == true)
                {
                    dialogueActive = false;
                    TriggerDialogue();

                }
                

                
                  

                



            }
            else
            {
                //dialogueActive = false;
            }
            


        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
