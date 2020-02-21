using System.IO;
using UnityEngine;

public static class DataSaver {

    public static void SaveData(SaveData data) {
        string json = JsonUtility.ToJson(data);

        string _path = Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json";
        File.WriteAllText(_path, json);
    }

    public static SaveData LoadData() {
        string _path = Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json";
        if(File.Exists(_path)) {
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(_path));
        }
        return null;
    }
}