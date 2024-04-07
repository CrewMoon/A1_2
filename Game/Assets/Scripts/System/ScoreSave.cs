using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class ScoreSave
{
    private static string saveFileName = "RankingData";
    
    public static void SaveByJson(GameRecord data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName + ".json");
        File.WriteAllText(path, json);
        Debug.Log("Save to " + path);
    }

    public static List<GameRecord> LoadByJson()
    {
        List<GameRecord> loadedRecords = new List<GameRecord>();

        try
        {
            string path = Path.Combine(Application.persistentDataPath, saveFileName + ".json");

            if (File.Exists(path))
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    while (file.ReadLine() is string line)
                    {
                        GameRecord record = JsonConvert.DeserializeObject<GameRecord>(line);
                        loadedRecords.Add(record);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No save file found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading game records: " + ex.Message);
        }

        return loadedRecords;
    }
}
