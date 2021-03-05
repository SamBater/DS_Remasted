using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static GameManager instance;
    public List<ActorManager> PersistObjs = new List<ActorManager>();
    private void Awake() {
        CheckSingleton();
        LoadJsonData();
    }
    
    public static GameManager Instance()
    {
        return instance;
    }

    private void CheckSingleton()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }

    public static void LoadScene()
    {
        //执行后会加载原有物件、不删除的物体就不用管了
        //重要的时如何删除一个物品
        //SceneManager.LoadScene("Scenes/BossBattle",LoadSceneMode.Single);
    }

    private void SaveJsonData()
    {
        SaveData saveData = new SaveData();

        for (int i = 0; i < PersistObjs.Capacity; i++)
        {
            PersistObjs[i].PopulateSaveData(saveData);
        }
        
        if (FileManager.WriteToFile("saveData.dat",saveData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    private void LoadJsonData()
    {
        if (FileManager.ReadFromFile("saveData.dat", out var json))
        {
            Debug.Log("Load successful");
            SaveData saveData = new SaveData();
            saveData.ReadFromJson(json);
            for (int i = 0; i < PersistObjs.Capacity; i++)
            {
                PersistObjs[i].LoadFromSaveData(saveData);
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveJsonData();
    }
    
    
}
