using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
