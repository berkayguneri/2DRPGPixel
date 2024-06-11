using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayController : MonoBehaviour
{
    public GameObject howToPlayPanel; // How to play panelini tutacak referans

    void Start()
    {
        // Oyun ba�lad���nda how to play panelini g�ster
        ShowHowToPlay();
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
