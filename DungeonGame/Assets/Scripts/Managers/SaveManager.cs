using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static readonly string saveFilePath = Application.persistentDataPath + "/gamesave.json";
    public static bool isFirstTimeLoading = true;

    public static void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(saveFilePath, json);
    }

    public static GameSaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            return data;
        }
        else
        {
            return new GameSaveData();
        }
    }
}