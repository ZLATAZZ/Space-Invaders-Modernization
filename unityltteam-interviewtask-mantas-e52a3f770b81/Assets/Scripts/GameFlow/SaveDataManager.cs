using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float effectsVolume;
    [HideInInspector] public int score;
    public static SaveDataManager Instance{get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    [Serializable]
    class DataToSave
    {
        public float musicVolume;
        public float effectsVolume;
        public int score;
    }

    public void SaveData()
    {
        DataToSave savedData = new DataToSave();
        savedData.musicVolume = musicVolume;
        savedData.effectsVolume = effectsVolume;
        savedData.score = score;

        string json = JsonUtility.ToJson(savedData);
        File.WriteAllText(Application.persistentDataPath + "saveData.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "saveData.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DataToSave savedData = JsonUtility.FromJson<DataToSave>(json);
            musicVolume = savedData.musicVolume;
            effectsVolume = savedData.effectsVolume;
            score = savedData.score;
        }
    }

}
