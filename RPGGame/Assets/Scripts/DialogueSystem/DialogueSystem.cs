using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
   public static DialogueSystem instance;

    public GameObject dialoguePanel;
    public List<string> dialogueLines = new List<string>();
    public string nameOfNPC;
    public Button continueButton;
    public TextMeshProUGUI dialogueText,nameText;
    private int dialogueIndex;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        else
            instance = this;

        dialoguePanel.SetActive(false);
    }

    public void AddDialogues(string[] _lines, string _npcName)
    {
        dialogueIndex = 0;
        dialogueLines = new List<string>(_lines.Length);
        dialogueLines.AddRange(_lines);
        this.nameOfNPC = _npcName;
        CreateDialogue();
    }

    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = nameOfNPC;
        dialoguePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueDialogues()
    {
        if (dialogueIndex < dialogueLines.Count - 1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
            Time.timeScale = 1;
        }
        AudioManager.instance.PlaySFX(38, null);
    }
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false); 
        Time.timeScale = 1; 

    }
}
