using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string[] dialogue;

    public string nameOfNPC;

    private bool isPlayerInRange = false;

    public GameObject interactionText;


    private void Start()
    {
        interactionText.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //DialogueSystem.instance.AddDialogues(dialogue,nameOfNPC);
            isPlayerInRange = true;
            interactionText.SetActive(true);
            Debug.Log("Alana girdi");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionText.SetActive(false);
            Debug.Log("Alandan cikti");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!DialogueSystem.instance.dialoguePanel.activeSelf)
            {
                DialogueSystem.instance.AddDialogues(dialogue, nameOfNPC); 
                interactionText.SetActive(false);
                AudioManager.instance.PlaySFX(41, null);
            }
            else
            {
                DialogueSystem.instance.CloseDialogue(); 
            }
        }
    }
}
