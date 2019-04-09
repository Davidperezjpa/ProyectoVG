using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject player;
    public Dialogue dialogue;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Inventory") == 1)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 5) 
            {
                StopCoroutine("OpenStore");
                StartCoroutine("OpenStore");
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    public void CloseDialogue() {
        FindObjectOfType<DialogueManager>().StopDialogue();
    }

    IEnumerator OpenStore() {
        while (distance<=5) {
            distance = Vector2.Distance(transform.position, player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
        CloseDialogue();
    }
}
