using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckPointId;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        checkPoints=FindObjectsOfType<CheckPoint>();
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene=SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value == true)
                    checkPoint.ActivateCheckPoint();
            }
        }

        closestCheckPointId = _data.closestCheckPointID;
        Invoke("PlacePlayerAtClosestCheckpoint",0.1f);

        Debug.Log("Game manager loaded");
    }

    private void PlacePlayerAtClosestCheckpoint()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckPointId  == checkPoint.id)
                PlayerManager.instance.player.transform.position = checkPoint.transform.position;
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckPointID = FindClosestCheckPoint().id;
        _data.checkPoints.Clear();

        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoints.Add(checkPoint.id, checkPoint.activationStatus);
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance=Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach(var checkPoint in checkPoints)
        {
            float distanceToCheckpoint=Vector2.Distance(PlayerManager.instance.player.transform.position,checkPoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkPoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }
        return closestCheckpoint;
    }
}
