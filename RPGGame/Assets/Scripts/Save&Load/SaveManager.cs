using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private GameData gameData;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;



    private FileDataHandler fileDataHandler;
    private List<ISaveManager> saveManagers;

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath , fileName,encryptData);
        fileDataHandler.Delete();
    }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath,fileName, encryptData);
        saveManagers=FindAllSaveManagers();

        LoadGame();
    }
    public void NewGame()
    {
        gameData= new GameData();
    }

    public void LoadGame()
    {

        gameData = fileDataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No saved data found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers() // Tum scriptlerde ISaveManagera ulasmanin kolay hali
    {
        IEnumerable<ISaveManager> saveManagers= FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
